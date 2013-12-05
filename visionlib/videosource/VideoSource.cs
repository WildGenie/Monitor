using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using VisionSDK;
using System.Diagnostics;
using Config;
using System.Threading;
using System.Windows.Forms;
using Monitor;
using PTZ;
using MonitorSystem;
using Popedom;
using Common;
using VideoDevice;

namespace VideoSource
{
    public enum PlayState
    {
        None = -1,
        Open = 0,
        Play = 1,
        Pause = 2,
        Stop = 3,
        Close = 4,
        End = 5,
        Error = 6
    }

    public enum VideoSourceState
    {
        None = -1,
        Norme = 0,
        NoLink = 1,
        NoVideo = 2,
        KernelError = 3,
        FrameError = 20,
        OtherError = 90
    }

    //FPS����ģʽ
    public enum FPSTuneMode
    {
        None = 0,	//�޵���
        Auto = 1,	//�Զ�����
        Force = 2   //ǿ�Ƶ���
    }

    //����ģʽ
    public enum VideoSourceRunMode
    {
        Pull = 0,	//��ģʽ
        Push = 1	//��ģʽ
    }

    //����״̬��   -1 δ��ʼ���� 0 ׼�����ţ� 1 �����У� 2 ֹͣ�� 3 ��ͣ�� 4 ������ɣ�
    //��ƵԴ״̬��  0 ������ 1 ������ϣ� 2 ���ź�
    public delegate void PLAYSTATUS_CHANGED(IMonitorSystemContext context, string vsName, VideoSourceState vsStatus, PlayState playStatus);
    public delegate void KERNELSTATUS_CHANGED(IMonitorSystemContext context, string vsName, VideoSourceKernelState vsStatus);

    public enum TShowOSDType
    {
        Default = 0,
        Show = 1,
        Hide = 2
    }

    public interface IImageRecorder
    {
        int Record(string path);
        bool StopRecord(int hRecord);

        int RecordPlay(IntPtr hWnd);
        int RecordPlay(string path, IntPtr hWnd);

        bool IsPauseRecordPlay(int hPlay);
        bool PauseRecordPlay(int hPlay);
        bool ResumeRecordPlay(int hPlay);
        bool StopRecordPlay(int hPlay);

        event RECORD_PROGRESS OnRecordProgress;
    }

    public interface IKernelVideoSource
    {
        bool IsKernelInit { get; }

        bool KernelInit();
        bool KernelRefresh();
        bool KernelStart();
        bool SetKernelFrame(Bitmap frame);
        bool SetKernelFrame(Bitmap frame, bool autoFree);
        bool KernelStop();
        bool KernelCleanup();

        event KERNELSTATUS_CHANGED OnKernelStatus;
    }

    public interface IVideoSource : IPopedom, IImageRecorder, IDisposable
    {
        string Key { get; }
        string Name { get; }
        int Handle { get; }
        IntPtr HWnd { get; set; }

        PlayState PlayStatus { get; }
        VideoSourceState VideoSourceStatus { get; }

        object Target { get; }
        bool IsOpen { get; }
        bool IsPlay { get; }
        bool IsPause { get; }
        bool IsBackPlay { get; }

        event PLAYSTATUS_CHANGED OnPlayStatusChanged;

        bool Open(object target);
        bool Play();
        bool Stop();
        bool Close();
        Bitmap GetFrame();

        void Reset();
        void RefreshState();
        void RefreshPlay();

        IVideoDevice Device { get; }
        IVideoSourceConfig Config { get; }
        IVideoSourceFactory Factory { get; }
        IVideoSourceManager Manager { get; }
        IMonitorSystemContext SystemContext { get; }

        IProperty Property { get; }
    }

    public interface IRealPlayer : IVideoSource
    {
        IPTZCtrl PTZCtrl { get; }
    }

    public interface IBackPlayer : IVideoSource
    {
        int PlayFrame { get; }
        DateTime PlayTime { get; }
        bool IsMute { get; set; }

        bool Fast();    //���
        bool Slow();    //����
        bool Frame();   //��֡����
        bool Normal();  //��������
        bool Pause();   //��ͣ����
        bool Resum();   //�ָ�����
        bool Locate(DateTime time);
    }

    public abstract class CVideoSource : CPopedom, IVideoSource, IKernelVideoSource
    {
        private static int mRootKey = 0;

        private int mHandle = Interlocked.Increment(ref mRootKey);

        protected IProperty mProperty = new CProperty();

        private IVideoDevice mDevice = null;
        private IVideoSourceFactory mFactory = null;
        private IVideoSourceConfig mConfig = null;

        private IPTZCtrl mPTZCtrl = null;

        private object mTarget = null;
        private IntPtr mHWnd = IntPtr.Zero;

        private PlayState mPlayStatus = PlayState.None;
        private VideoSourceState mVideoSourceStatus = VideoSourceState.Norme;

        private bool mIsOpen = false;
        private bool mIsPlay = false;

        private bool mIsKernelInit = false;

        private IRecordManager mRecorder = null;
        private GetFrameFunPtr mGetFrameFun = null;
        private VideoSourceKernelStateChanged mVideoSourceKernelStateChanged = null;

        public event KERNELSTATUS_CHANGED OnKernelStatus = null;
        public event PLAYSTATUS_CHANGED OnPlayStatusChanged = null;
        public event RECORD_PROGRESS OnRecordProgress = null;

        public CVideoSource(IVideoSourceConfig config, IVideoDevice device, IVideoSourceFactory factory)
            : base()
        {            
            mConfig = config;
            mDevice = device;
            mFactory = factory;
        }

        ~CVideoSource()
        {
            KernelCleanup();

            Close();

            if (mRecorder != null)
            {
                mRecorder.Dispose();
                mRecorder = null;
            }
        }

        public virtual void Dispose()
        {
            KernelCleanup();

            Close();

            if (mRecorder != null)
            {
                mRecorder.Dispose();
                mRecorder = null;
            }

            GC.SuppressFinalize(this);
        }

        public override bool Verify(ACOpts acopt, bool isQuiet)
        {
            return mConfig != null ? mConfig.Verify(acopt, isQuiet) : true;
        }

        public int Handle
        {
            get { return mHandle; }
        }

        public string Name
        {
            get { return mConfig.Name; }
        }

        public string Key
        {
            get { return mConfig.Key; }
        }

        public IProperty Property
        {
            get { return mProperty; }
        }

        public IPTZCtrl PTZCtrl
        {
            get { return mPTZCtrl; }
            protected set
            {
                mPTZCtrl = value;
            }
        }

        public IMonitorSystemContext SystemContext
        {
            get { return mFactory.Manager.SystemContext; }
        }

        public IVideoSourceManager Manager
        {
            get { return mFactory.Manager; }
        }

        public IVideoSourceFactory Factory
        {
            get { return mFactory; }
        }

        public IVideoDevice Device
        {
            get { return mDevice; }
        }

        public IVideoSourceConfig Config
        {
            get { return mConfig; }
        }

        public object Target
        {
            get { return mTarget; }
            protected set
            {
                mTarget = value;
            }
        }

        public virtual IntPtr HWnd
        {
            get { return mHWnd; }
            set { mHWnd = value; }
        }

        public PlayState PlayStatus
        {
            get { return mPlayStatus; }
            set
            {
                if (value != mPlayStatus)
                {
                    CLocalSystem.WriteDebugLog(string.Format("{0} CVideoSource({1}): PlayStatus({2}-->{3})", Config.Desc, Name, mPlayStatus, value));

                    mPlayStatus = value;

                    if (mPlayStatus == PlayState.Open)
                        mVideoSourceStatus = VideoSourceState.Norme;

                    DoPlayStatusChange(Name, VideoSourceStatus, mPlayStatus);
                }
            }
        }

        public VideoSourceState VideoSourceStatus
        {
            get { return mVideoSourceStatus; }
            set
            {
                if (value != mVideoSourceStatus)
                {
                    CLocalSystem.WriteDebugLog(string.Format("{0} CVideoSource({1}): VideoSourceStatus({2}-->{3})", Config.Desc, Name, mVideoSourceStatus, value));

                    mVideoSourceStatus = value;
                    DoPlayStatusChange(Name, mVideoSourceStatus, PlayStatus);
                }
            }
        }

        public void RefreshState()
        {
            DoPlayStatusChange(Name, VideoSourceStatus, PlayStatus);
        }

        public bool IsOpen
        {
            get { return mIsOpen; }
            private set
            {
                mIsOpen = value;
            }
        }

        public bool IsPlay
        {
            get { return mIsPlay; }
            private set
            {
                mIsPlay = value;
            }
        }

        public bool IsPause
        {
            get { return PlayStatus == PlayState.Pause; }
        }

        public bool IsBackPlay
        {
            get { return this is IBackPlayer; }
        }

        public bool Open(object target)
        {
            if (!IsOpen && Verify(ACOpts.Exec_Init))
            {
                if (target == null)
                    target = Target;

                if (target != null)
                {
                    if (PrepOpen(target))
                    {
                        Target = target;

                        IsOpen = true;
                        PlayStatus = PlayState.Open;
                    }
                }
            }
            return IsOpen;
        }

        public bool Play()
        {
            if (IsOpen && (!IsPlay || IsPause) && Verify(ACOpts.Exec_Start))
            {
                if (PrepPlay())
                {
                    IsPlay = true;
                    PlayStatus = PlayState.Play;
                }
            }
            return IsPlay;
        }

        public bool Stop()
        {
            if (IsPlay && Verify(ACOpts.Exec_Stop))
            {
                if (PrepStop())
                {
                    IsPlay = false;
                    PlayStatus = PlayState.Stop;
                }
            }
            return !IsPlay;
        }

        public bool Close()
        {
            if (IsOpen && Verify(ACOpts.Exec_Cleanup))
            {
                Stop();

                if (PrepClose())
                {
                    IsOpen = false;
                    PlayStatus = PlayState.Close;

                    if (mRecorder != null)
                    {
                        mRecorder.Clear();
                    }
                }
            }
            return !IsOpen;
        }

        public virtual void Reset()
        {
            CLocalSystem.WriteInfoLog(string.Format("CVideoSource({0}).Reset", Name));

            PrepStop();
            PrepClose();

            this.VideoSourceStatus = VideoSourceState.Norme;

            if (IsOpen)
            {
                if (PrepOpen(Target))
                {
                    if (IsPlay)
                    {
                        if (!PrepPlay())
                            Stop();
                    }
                }
                else Close();
            }
        }

        public virtual void RefreshPlay()
        {
            //
        }

        protected abstract bool PrepOpen(object target);
        protected abstract bool PrepPlay();
        protected abstract bool PrepStop();
        protected abstract bool PrepClose();
        protected abstract Bitmap GetCurFrame();

        public Bitmap GetFrame()
        {
            if (IsOpen)
            {
                try
                {
                    //DateTime begin = DateTime.Now;
                    Bitmap bmp = GetCurFrame();
                    //System.Console.Out.WriteLine(string.Format("ץ֡��ʱ��{0}", DateTime.Now.Millisecond - begin.Millisecond));

                    if (mConfig.IsRecord && bmp != null)
                    {
                        if (mRecorder == null)
                            initImageRecorder();

                        if (mRecorder != null)
                            mRecorder.Append(new Bitmap(bmp), false);
                    }
                    return bmp;
                }
                catch (Exception e)
                {
                    CLocalSystem.WriteErrorLog(string.Format("{0} GetFrame Exception: {1}", Name, e));

                    Process curproc = Process.GetCurrentProcess();

                    StringBuilder sb = new StringBuilder(curproc.ProcessName + "������Ϣ��\n");
                    sb.Append("���̷���������ڴ���=" + curproc.WorkingSet64 + "�ֽ�\n");
                    sb.Append("���̷���������ڴ���=" + curproc.VirtualMemorySize64 + "�ֽ�\n");
                    sb.Append("���̷����ר���ڴ���=" + curproc.PrivateMemorySize64 + "�ֽ�\n");
                    sb.Append("���̷���ķ�ҳ�ڴ���=" + curproc.PagedMemorySize64 + "�ֽ�\n");
                    sb.Append("���̷���Ŀɷ�ҳϵͳ�ڴ���=" + curproc.PagedSystemMemorySize64 + "�ֽ�\n");
                    sb.Append("����ʹ�õ������ڴ��ҳ�ļ��е�����ڴ���=" + curproc.PeakPagedMemorySize64 + "�ֽ�\n");
                    sb.Append("����ʹ�õ���������ڴ���=" + curproc.PeakVirtualMemorySize64 + "�ֽ�\n");
                    sb.Append("���̴򿪵ľ����=" + curproc.HandleCount + "��\n");

                    CLocalSystem.WriteDebugLog(string.Format("{0} GetFrame Exception: �ڴ���Ϣ={1}", Name, sb.ToString()));
                }
            }
            return null;
        }

        protected bool initImageRecorder()
        {
            if (mRecorder == null)
            {
                if (mConfig.IsRecord)
                {
                    if (mConfig.RecordLimit <= 0)
                        mConfig.RecordLimit = mConfig.FPS * 10;

                    //mRecorder = new CImageRecorder(Name, mConfig.RecordLimit);

                    mRecorder = new CRecordManager(Name, mConfig.RecordLimit);
                    mRecorder.OnRecordProgress += new RECORD_PROGRESS(DoRecordProgress);
                    return true;
                }
                else return false;
            }
            else return true;
        }

        public int Record(string path)
        {
            if (mConfig.IsRecord && initImageRecorder())
            {
                return mRecorder.Save(path);
            }
            return -1;
        }

        public bool StopRecord(int hRecord)
        {
            if (mConfig.IsRecord && initImageRecorder())
            {
                return mRecorder.Stop(hRecord);
            }
            return false;
        }

        public int RecordPlay(IntPtr hWnd)
        {
            if (mConfig.IsRecord && initImageRecorder())
            {
                return mRecorder.Play(hWnd);
            }
            return -1;
        }

        public int RecordPlay(string path, IntPtr hWnd)
        {
            if (mConfig.IsRecord && initImageRecorder())
            {
                return mRecorder.Play(path, hWnd);
            }
            return -1;
        }

        public bool IsPauseRecordPlay(int hPlay)
        {
            if (mConfig.IsRecord && initImageRecorder())
            {
                return mRecorder.IsPause(hPlay);
            }
            return false;
        }

        public bool PauseRecordPlay(int hPlay)
        {
            if (mConfig.IsRecord && initImageRecorder())
            {
                return mRecorder.Pause(hPlay);
            }
            return false;
        }

        public bool ResumeRecordPlay(int hPlay)
        {
            if (mConfig.IsRecord && initImageRecorder())
            {
                return mRecorder.Resume(hPlay);
            }
            return false;
        }

        public bool StopRecordPlay(int hPlay)
        {
            if (mConfig.IsRecord && initImageRecorder())
            {
                return mRecorder.Stop(hPlay);
            }
            return false;
        }

        protected void DoPlayStatusChange(string vsName, VideoSourceState vsStatus, PlayState playStatus)
        {
            try
            {
                if (IsPlay && vsStatus > VideoSourceState.Norme)
                {
                    this.Reset();
                }

                if (OnPlayStatusChanged != null)
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            OnPlayStatusChanged(SystemContext, vsName, vsStatus, playStatus);
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnPlayStatusChanged(SystemContext, vsName, vsStatus, playStatus);
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CVideoSource.DoPlayStatusChange Exception:{0}", e));
            }
        }

        protected void DoRecordProgress(int hRecord, int progress)
        {
            try
            {
                if (OnRecordProgress != null)
                {
                    if (CLocalSystem.MainForm != null)
                    {
                        MethodInvoker form_invoker = delegate
                        {
                            OnRecordProgress(hRecord, progress);
                        };
                        CLocalSystem.MainForm.Invoke(form_invoker);
                    }
                    else OnRecordProgress(hRecord, progress);
                }
            }
            catch (Exception e)
            {
                CLocalSystem.WriteErrorLog(string.Format("CVideoSource.OnRecordProgress Exception:{0}", e));
            }
        }

        private IntPtr OnGetFrameFun()
        {
            Bitmap bmp = GetFrame();
            if (bmp != null)
            {
                Thread.Sleep(1);

                IntPtr hBmp = bmp.GetHbitmap();

                bmp.Dispose();

                return hBmp;
            }
            return IntPtr.Zero;
        }

        public bool IsKernelInit
        {
            get { return mIsKernelInit; }
            private set
            {
                mIsKernelInit = value;
            }
        }

        public void KernelReset()
        {
            if (IsKernelInit)
            {
                if (CLocalSystem.MainForm != null)
                {
                    MethodInvoker form_invoker = delegate
                    {
                        try
                        {
                            KernelCleanup();
                            KernelInit();
                        }
                        catch (Exception e)
                        {
                            CLocalSystem.WriteErrorLog(string.Format("CVideoSource.KernelReset Exception:{0}", e));
                        }
                    };
                    CLocalSystem.MainForm.Invoke(form_invoker);
                }
                else
                {
                    try
                    {
                        KernelCleanup();
                        KernelInit();
                    }
                    catch (Exception e)
                    {
                        CLocalSystem.WriteErrorLog(string.Format("CVideoSource.KernelReset Exception:{0}", e));
                    }
                }
            }
        }

        public bool KernelInit()
        {
            if (IsOpen && !IsKernelInit)
            {
                if (mGetFrameFun == null)
                    mGetFrameFun = new GetFrameFunPtr(OnGetFrameFun);

                if (VideoSourceSDKWrap.CreateVideoSource(Name, mGetFrameFun))
                {
                    IsKernelInit = true;

                    if (mVideoSourceKernelStateChanged == null)
                        mVideoSourceKernelStateChanged = new VideoSourceKernelStateChanged(DoVideoSourceKernelStateChanged);

                    VideoSourceSDKWrap.SetVideoSourceStateChangedCallback(Name, mVideoSourceKernelStateChanged);

                    VideoSourceSDKWrap.SetVideoSourceParams(Name, mConfig.FPS, (int)mConfig.RunMode, mConfig.IsAutoTune, mConfig.CPU);
                }
            }
            return IsKernelInit;
        }

        public bool KernelRefresh()
        {
            if (IsKernelInit)
            {
                return VideoSourceSDKWrap.SetVideoSourceParams(Name, mConfig.FPS, (int)mConfig.RunMode, mConfig.IsAutoTune, mConfig.CPU);
            }
            return false;
        }

        public bool KernelStart()
        {
            if (IsKernelInit)
            {
                return VideoSourceSDKWrap.StartVideoSource(Name);
            }
            return false;
        }

        public bool SetKernelFrame(Bitmap frame)
        {
            return SetKernelFrame(frame, true);
        }

        public bool SetKernelFrame(Bitmap frame, bool autoFree)
        {
            if (frame != null)
            {
                if (IsKernelInit)
                {
                    IntPtr hBmp = IntPtr.Zero;
                    if (autoFree)
                    {
                        hBmp = frame.GetHbitmap();
                        frame.Dispose();
                        frame = null;
                    }

                    if (hBmp == IntPtr.Zero)
                    {
                        Bitmap bmp = new Bitmap(frame);
                        hBmp = bmp.GetHbitmap();
                        bmp.Dispose();
                    }
                    return VideoSourceSDKWrap.SetVideoSourceFrame(Name, hBmp);
                }
                else if (autoFree)
                {
                    frame.Dispose();
                    frame = null;
                }
            }
            return false;
        }

        public bool KernelStop()
        {
            if (IsKernelInit)
            {
                return VideoSourceSDKWrap.StopVideoSource(Name);
            }
            return false;
        }

        public bool KernelCleanup()
        {
            if (IsKernelInit)
            {
                KernelStop();

                SystemContext.MonitorManager.ClearFromVSName(Name);

                if (VideoSourceSDKWrap.FreeVideoSource(Name))
                {
                    IsKernelInit = false;
                }
            }
            return !IsKernelInit;
        }

        private void DoVideoSourceKernelStateChanged(string name, VideoSourceKernelState state)
        {
            CLocalSystem.WriteDebugLog(string.Format("{0} CVideoSource({1}).DoKernelStateChanged KernelState={2}", Config.Desc, name, state));

            if (OnKernelStatus != null)
            {
                if (CLocalSystem.MainForm != null)
                {
                    MethodInvoker form_invoker = delegate
                    {
                        OnKernelStatus(SystemContext, name, state);
                    };
                    CLocalSystem.MainForm.Invoke(form_invoker);
                }
                else OnKernelStatus(SystemContext, name, state);
            }

            if (state < 0)
            {
                if (VideoSourceStatus != VideoSourceState.NoLink)
                    VideoSourceStatus = VideoSourceState.KernelError;

                switch (state)
                {
                    case VideoSourceKernelState.VSState_FrameFailed:
                        //System.Windows.Forms.MessageBox.Show(name + " ��ƵԴ֡����ʧЧ��", "��ƵԴ�ں�״̬��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CLocalSystem.WriteErrorLog(string.Format("\"{0}\"��ƵԴ֡����ʧЧ��", name));
                        break;
                    case VideoSourceKernelState.VSState_FrameNull:
                        //System.Windows.Forms.MessageBox.Show(name + " ��ƵԴץ֡����Ϊ�գ�", "��ƵԴ�ں�״̬��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CLocalSystem.WriteErrorLog(string.Format("\"{0}\"��ƵԴץ֡����Ϊ�գ�", name));
                        break;
                    case VideoSourceKernelState.VSState_LockLost:
                        //System.Windows.Forms.MessageBox.Show(name + " ��ƵԴץ֡������", "��ƵԴ�ں�״̬��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CLocalSystem.WriteErrorLog(string.Format("\"{0}\"��ƵԴץ֡������", name));
                        break;
                    case VideoSourceKernelState.VSState_TimerFailed:
                        //System.Windows.Forms.MessageBox.Show(name + " ��ƵԴץ֡ʧЧ��", "��ƵԴ�ں�״̬��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CLocalSystem.WriteErrorLog(string.Format("\"{0}\"��ƵԴץ֡ʧЧ��", name));
                        break;
                    case VideoSourceKernelState.VSState_OtherProblem:
                        //System.Windows.Forms.MessageBox.Show(name + " ��ƵԴ�����������⣡", "��ƵԴ�ں�״̬��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CLocalSystem.WriteErrorLog(string.Format("\"{0}\"��ƵԴ�����������⣡", name));
                        break;
                    default:
                        //System.Windows.Forms.MessageBox.Show(name + " ��ƵԴ����δ֪����", "��ƵԴ�ں�״̬��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CLocalSystem.WriteErrorLog(string.Format("\"{0}\"��ƵԴ����δ֪����", name));
                        //KernelReset();
                        break;
                }
            }
        }
    }
}
