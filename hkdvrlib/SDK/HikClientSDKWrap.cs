using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using HKDevice;

namespace HKSDK
{
    public class HikClientSDKWrap
    {
        private const String SDKDll = "hikclient.dll";

        #region �������Ͷ���

        //�ͻ�״̬
        public enum ClientState
        {
            State_0 = -1, //��Ч 
            State_1 = 1,  //�������� 
            State_2 = 2,  //��ʼ����ͼ�� 
            State_3 = 3,  //�쳣�˳� 
            State_4 = 4,  //������ϣ��˳� 
            State_5 = 5,  //�޷���ϵ����� 
            State_6 = 6   //����˾ܾ����� 
        };

        ////ͼ���ʽ
        //public enum PicFormat
        //{
        //    T_UYVY	=	1,
        //    T_YV12	=	3,
        //    T_RGB32	=	7
        //};

        public enum ShowMode
        {
            NORMALMODE = 0,
            OVERLAYMODE
        };

        //ͼ���ʽ
        public enum ImageFormat : byte
        {
            MAIN_CHANNEL = 0,
            SUB = 1
        };

        //�������ӷ�ʽ
        public enum NetSendMode : byte
        {
            UDPMODE=0,
            TCPMODE,
            MULTIMODE,
            DIALING,
            AUDIODETACH
        };

        public enum PicQuality 
        { 
            LOWQUALITY = 0, 
            HIGHQUALITY 
        };


        public enum CleanBufferType 
        { 
            CLIENT = 0, 
            SERVER,
            BOTH
        };
        

        public enum VideoRGBYUVFormat : uint
        {
            vfGeneric                = 0xffffffff,
            vfNone                   = 0x80000000,
            vfRGB8A_233              = 0x00000001,
            vfRGB8R_332              = 0x00000002,
            vfRGB15Alpha             = 0x00000004,
            vfRGB16                  = 0x00000008,
            vfRGB24                  = 0x00000010,
            vfRGB24Alpha             = 0x00000020,
           
            vfYUV420Planar           = 0x00000040,
            vfYUV422Planar           = 0x00000080,
            vfYUV411Planar           = 0x00000100,
            vfYUV420Interspersed     = 0x00000200,
            vfYUV422Interspersed     = 0x00000400,
            vfYUV411Interspersed     = 0x00000800,
            vfYUV422Sequence         = 0x00001000,   /* U0, Y0, V0, Y1:  For VO overlay */
            vfYUV422SequenceAlpha    = 0x00002000,   
           /* U0, Y0, V0, Y1:  For VO overlay, with low bit for alpha blending */
            vfMono                   = 0x00004000,  /* 8 bit monochrome */

            vfYUV444Planar           = 0x00008000,

            vfDTVCMPlanar            = 0x00010000,
            vfDTVCMSequence          = 0x00020000,

            vfYInterleavedUV420      = 0x00040000,  /* YUV420 output by TM2 mpeg2 */

            vfYUV422PlanarAlpha4     = 0x00080000

        };

        [StructLayout(LayoutKind.Sequential)]
        public struct CLIENT_VIDEOINFO{
            public byte m_bRemoteChannel;
            public NetSendMode m_bSendMode;
            public ImageFormat m_nImgFormat;  // =0 main channel ; = 1 sub channel
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string m_sIPAddress;
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string m_sUserName;
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string m_sUserPassword;
            public bool m_bUserCheck;
            public IntPtr m_hShowVideo;
        };

        #endregion

        #region ��ʼ��                

        //���ö˿�
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool MP4_ClientSetNetPort(short dServerPort, short dClientPort);

        //���öಥ�� TTL ����
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool MP4_ClientSetTTL(short cTTLVal);

        //�Կͻ��˳�ʼ��
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool MP4_ClientStartup(int nMessage, IntPtr hWnd);

        //�������ÿͻ��˺���
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool MP4_ClientCleanup();

        //�����ͻ���
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int MP4_ClientStart(ref CLIENT_VIDEOINFO ClientInfo, READDATA_CALLBACK ReadDataCallBack);

        //ֹͣ�ͻ���
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool MP4_ClientStop(int StockHandle);

        //��ȡ�ͻ���״̬
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int MP4_ClientGetState(int StockHandle);

        //��ʼ�ͻ��˵����ݲ���
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool MP4_ClientStartCapture(int StockHandle);

        //ֹͣ�ͻ��˵����ݲ���
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool MP4_ClientStopCapture(int StockHandle);

        //��ȡ����˵�ͨ����
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static short MP4_ClientGetServerChanNum(string m_sIPAddress);

        //�������ӷ���˵ĵȴ�ʱ��ͳ��Դ���
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool MP4_ClientSetWait(int dEachWaitTime, int dTrynum);

        //����ͼ������
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool MP4_ClientSetQuality(int StockHandle, short wPicQuality);

        #endregion

        //����ץͼ�ص�����
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool MP4_ClientSetCapPicCallBack(int StockHandle, CAP_PIC_FUN CapPicFun);

        //���õ�����Ļ�Ļص�����
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool MP4_ClientRigisterDrawFun(int StockHandle, DRAWFUN DrawFun, int nUser);

        //���ն������ݺ�ſ�ʼ���� DelayLen��0-600
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool MP4_ClientSetPlayDelay(int StockHandle, short DelayLen);

        //����ͼ������
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool MP4_ClientSetQuality(int StockHandle, PicQuality wPicQuality);

        //���ý��ջ�������С wBufNum��0-50
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool MP4_ClientSetBufferNum(int StockHandle, short wBufNum);

        //������ݻ������������ͻ��˺ͷ���� 
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool MP4_ClientCleanBuffer(int StockHandle, CleanBufferType nCleanType);
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void READDATA_CALLBACK(int nChannel,byte[] pPacketBuffer,int nPacketSize);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    //public delegate void CAP_PIC_FUN(int StockHandle, byte[] pBuf, int nSize, int nWidth, int nHeight, int nStamp, int nType, int nReceaved);
    public delegate void CAP_PIC_FUN(int StockHandle, IntPtr pBuf, int nSize, int nWidth, int nHeight, int nStamp, int nType, int nReceaved);
}
