using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using WIN32SDK;

namespace VisionSDK
{
    #region ���ݽṹ����

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    unsafe public struct GuardArea
    {
        public int index;
        public int type;
        public int level;
        public ushort opt;
        public ushort sensitivity;
        //public int param;
        public int wanderCount; //�ǻ�����
        public int stayTime;	//��������
        public int assembleCount; //�ۼ�Ŀ����
        public int interval;
        public int count;
        public win32.POINT* points;
        public win32.RECT r;
        public win32.POINT minsize;	//��С��С
        public win32.POINT maxsize;	//����С
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DepthArea
    {
        public int x1;
        public int y1;      //���Ͻ�����
        public int x2;
        public int y2;      //���½�����
        public int height;  //�ڿ�ĸ߶�
        public int width;   //�ڿ�Ŀ��
        public int IsDepth; //�����˾���ĸ�������ΪTURE ��ʼ��Ϊfalse       
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    unsafe public struct Configuration
    {
        ////////////////////////////////////////////////////////////////////////// 
        public win32.POINT AvailableMinSize;	//��Ч������С��С
        public win32.POINT AvailableMaxSize;	//��Ч��������С
        public double AvailableMinSpeed;		//��Ч������С�ٶ�
        public double AvailableMaxSpeed;		//��Ч��������ٶ�
        public int TimeThreshold;               //�������ƶ�ʱ�䷧ֵ

        public ushort GuardAlert;				//���䱨��ѡ�� �����
        public int DensityMinNum;			    //�ܶȱ�����Сֵ

        //public bool FaceDetectTwoStep;		//�沿���,�Ƿ���μ��   //longbool
        //public bool FaceDetectForeground;	    //�沿���,�Ƿ�ǰ�����   //longbool

        public int WanderAlertMinTimes;	        //�ǻ�����ʱ��ѡ��:����
        public int StayAlertMinTimes;		    //ͣ������ʱ��ѡ��:����

        public int ProcessMode;			        //ץ֡���������ģʽ:0�첽��1ͬ��

        //public win32.POINT SensorPosition;	//�����λ������X,Y	 2007-07-18
        //public string FaceDetectXmlFile;		//�沿���,ѵ���ļ�,��  move and change

        public int GuardAreaCount;			    //��������
        public GuardArea* GuardAreas;			//��������

        public int DepthAreaCount;			    //������������
        public DepthArea* DepthAreas;			//������������
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CVisionEvent
    {        
        public int eventType;                   //�¼�����
        public int guardLevel;                  //���伶��
        public int areaIndex;                   //��������	 
        public int areaType;                    //��������	
        public ushort alertOpt;                 //����ѡ��
        public IntPtr /*IplImage* */ image;		//�¼�ͼƬ
    }

    //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    //public struct FaceKernelConfig
    //{
    //    //public string mSampleLib;
    //}

    //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    //public struct CFaceEvent
    //{
    //    public IntPtr /*IplImage* */ image;	        //�¼�ͼƬ
    //    public IntPtr /*IplImage* */ faceImage;		//����ͼƬ
    //    public string faceSampleIdList;             //�������ͼƬ��Ӧ������ID����ID���÷ֺš�;���ָ�
    //}

    public enum VideoSourceKernelState
    {
	    VSState_OtherProblem=-100, 
	    VSState_TimerFailed=-10,
        VSState_FrameFailed=-3,
	    VSState_FrameNull=-2, 
	    VSState_LockLost=-1, 
	    VSState_Init=0, 
	    VSState_Start=1,
        VSState_Active=2,
	    VSState_Stop=3
    }

    #endregion

    public class VisionSDKWrap
    {
        protected const String SDKDll = "CheckKernel.dll";
    }

    public class VideoSourceSDKWrap : VisionSDKWrap
    {
        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool CreateVideoSource(string name, GetFrameFunPtr getFrame);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool SetVideoSourceStateChangedCallback(string name, VideoSourceKernelStateChanged callback);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool SetVideoSourceParams(string name, int fps, int runMode, bool autoTune, uint threadAffinityMask);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool SetVideoSourceFrame(string name, IntPtr hBmp);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool StartVideoSource(string name);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool StopVideoSource(string name);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool FreeVideoSource(string name);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool ClearVideoSource();
    }

    public class VisionUserSDKWrap : VisionSDKWrap
    {
        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool CreateVisionUser(string name, string className);        

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool RegisterMessageCallback(string name, MessageCallbackFunPtr callback);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool RegisterVisionStatisticCallback(string name, VisionUserStatisticInfo callback);
                          
        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool SetConfigParams(string name, string videoSourceName, string processorParams, IntPtr config);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool IsActive(string name);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool SetActive(string name, bool active);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool FreeVisionUser(string name);

        [DllImport(SDKDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool ClearVisionUser();
    }

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    public delegate IntPtr GetFrameFunPtr();

    [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
    public delegate void MessageCallbackFunPtr(string id, string sender, IntPtr message);

    [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
    public delegate void VideoSourceKernelStateChanged(string name, VideoSourceKernelState state);

    [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
    public delegate void VisionUserStatisticInfo(string name, int vsfps, int vpfps, int frames);
}
