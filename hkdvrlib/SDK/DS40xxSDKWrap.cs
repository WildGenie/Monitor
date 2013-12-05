using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using WIN32SDK;
using HKDevice;

namespace HKSDK
{
    #region  �������Ͷ���

    //��Ƶ��׼����
    public enum VideoStandard_t : uint
    {
        StandardNone = 0x80000000,
        StandardNTSC = 0x00000001,
        StandardPAL = 0x00000002,
        StandardSECAM = 0x00000004
    };

    public enum TypeVideoFormat : uint
    {
        vdfRGB8A_233 = 0x00000001,
        vdfRGB8R_332 = 0x00000002,
        vdfRGB15Alpha = 0x00000004,
        vdfRGB16 = 0x00000008,
        vdfRGB24 = 0x00000010,
        vdfRGB24Alpha = 0x00000020,

        vdfYUV420Planar = 0x00000040,
        vdfYUV422Planar = 0x00000080,
        vdfYUV411Planar = 0x00000100,
        vdfYUV420Interspersed = 0x00000200,
        vdfYUV422Interspersed = 0x00000400,
        vdfYUV411Interspersed = 0x00000800,
        vdfYUV422Sequence = 0x00001000,   /* U0, Y0, V0, Y1:  For VO overlay */
        vdfYUV422SequenceAlpha = 0x00002000,
        /* U0, Y0, V0, Y1:  For VO overlay, with low bit for alpha blending */
        vdfMono = 0x00004000,  /* 8 bit monochrome */

        vdfYUV444Planar = 0x00008000
    };

    //OSD����ʱ������
    public enum OSDDateTimeType : ushort
    {
        OSD_YEAR4 = 0x9000,
        OSD_YEAR2,
        OSD_MONTH3,
        OSD_MONTH2,
        OSD_DAY,
        OSD_WEEK3,
        OSD_CWEEK1,
        OSD_HOUR24,
        OSD_HOUR12,
        OSD_MINUTE,
        OSD_SECOND
    };

    #endregion

    public class DS40xxSDKWrap
    {
        private const String SDKDll = "DS40xxSDK.dll";

        #region ��ʼ��         

        //��ʼ��ϵͳ�еİ忨
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int InitDSPs();

        ///�ر�ϵͳ�а忨�ϵĹ���
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int DeInitDSPs();

        #endregion

        #region ͨ������

        //
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int GetTotalChannels();

        //��ͨ����ChannelNum��0��ʼ���ɹ�������Ч�����ֵ����Ϊ 0����ʧ�ܷ��� 0xFFFFFFFF
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static IntPtr ChannelOpen(int ChannelNum);

        //�ر�ͨ�� �ɹ����� 0��ʧ�ܷ��ش����
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int ChannelClose(IntPtr hChannelHandle);

        #endregion

        #region ��ƵԤ��

        //������ƵԤ��
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int StartVideoPreview(IntPtr hChannelHandle, IntPtr WndHandle, ref win32.RECT rect, bool bOverlay, TypeVideoFormat VideoFormat, int FrameRate);

        //ֹͣ��ƵԤ��
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int StopVideoPreview(IntPtr hChannelHandle);

        #endregion

        #region ��Ƶ�ź�����

        //����ϵͳĬ�ϵ���Ƶ��ʽ
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int SetDefaultVideoStandard(VideoStandard_t VideoStandard);

        //��ȡ��Ƶ�ź��������
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int GetVideoSignal(IntPtr hChannelHandle);

        #endregion

        #region ԭʼͼ��������

        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int SetImageStream(IntPtr hChannel, bool bStart, int fps, int width, int height,
            /*unsigned char **/byte[] imageBuffer);

        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int RegisterImageStreamCallback(IMAGE_STREAM_CALLBACK ImageStreamCallback, IntPtr context);

        #endregion

        #region ץͼ

        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int GetOriginalImage(IntPtr hChannelHandle, byte[] ImageBuf, ref ulong size);

        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int SaveYUVToBmpFile(string fileName, byte[] yuv, int Width, int Height);

        #endregion

        #region OSD

        //���� OSD ��ʾģʽ
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int SetOsdDisplayMode(IntPtr hChannelHandle, int Brightness, bool Translucent, int parameter, ushort[] Format1, ushort[] Format2);

        //���� OSD ��ʾģʽ����չ��
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int SetOsdDisplayModeEx(IntPtr hChannelHandle, int color, bool Translucent, int param, int nLineCount, ushort[][] Format);

        //���� OSD ��ʾ
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int SetOsd(IntPtr hChannelHandle, bool Enable);

        //ע�ửͼ�ص�����
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int RegisterDrawFun(int port, DRAWFUN DrawFun, int user);

        //ֹͣ��ͼ�ص�
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int StopRegisterDrawFun(int port);

        #endregion
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void IMAGE_STREAM_CALLBACK(int channelNumber, IntPtr context);
}
