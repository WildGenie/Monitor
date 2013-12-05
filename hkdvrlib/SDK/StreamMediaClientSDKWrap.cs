using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HKSDK
{
    public interface IHikClientAdviseSink
    {
	    //��Setupʱ������,��ȡ�ܵĲ��ų���.nLengthΪ�ܵĲ��ų���,��1/64��Ϊ��λ
	    int OnPosLength(ulong nLength);

        //��Setup�󱻵���,��ʾURL�Ѿ����ɹ���,sucessΪ1��ʾ�ɹ�,0��ʾʧ��
	    int OnPresentationOpened(int success);

        //��Player��ֹͣ���ٺ����
	    int OnPresentationClosed();

        //δʹ��
	    int OnPreSeek(ulong uOldTime, ulong uNewTime);

        //δʹ��
	    int OnPostSeek(ulong uOldTime, ulong uNewTime);

        //δʹ��
	    int OnStop();

        //��Pauseʱ�����ã�uTimeĿǰ����0
	    int OnPause(ulong uTime);

        //�ڿ�ʼ����ʱ���ã�uTimeĿǰ����0
	    int OnBegin(ulong uTime);

        //���������ʱ���ã�uTimeĿǰ����0
	    int OnRandomBegin(ulong uTime);

        //��Setupǰ���ã�pszHost��ʾ�������ӵķ�����
	    int OnContacting(string pszHost);

	    //�ڷ������˷��س�����Ϣ�ǵ��ã� pError��Ϊ������Ϣ����
	    int OnPutErrorMsg(string pError);
	
        //δʹ��
	    int OnBuffering(uint uFlag, ushort uPercentComplete);

	    int OnChangeRate(int flag);

	    int OnDisconnect();
    }

    public class StreamMediaClientSDKWrap
    {
        private const String SDKDll = "client.dll";

        //��ʼ�����ú�����Ҫ�ڴ��ڳ����ʼ��ʱ���ã��ɹ����� 0��ʧ�ܷ���-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int InitStreamClientLib();

        //����ʼ�����ú�����Ҫ�ڴ��ڳ���ر�ʱʱ���ã��ɹ����� 0��ʧ�ܷ���-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int FiniStreamClientLib();

        //����ʼ�����ú�����Ҫ�ڴ��ڳ���ر�ʱʱ���ã��ɹ����� 0��ʧ�ܷ���-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int HIKS_CreatePlayer(IntPtr pSink, IntPtr pWndSiteHandle, IntPtr pRecFunc, IntPtr pMsgFunc, int TransMethod);
        //public extern static int HIKS_CreatePlayer(IHikClientAdviseSink pSink, IntPtr pWndSiteHandle, IntPtr pRecFunc, IntPtr pMsgFunc, int TransMethod);

        //����URL�����ӷ��������ɹ����� 1��ʧ�ܷ���-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int HIKS_OpenURL(int hSession,string pszURL, int iusrdata);

        //���ţ��ɹ����� 1��ʧ�ܷ���-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int HIKS_Play(int hSession);

        //��ͣ���ţ��ɹ����� 1��ʧ�ܷ���-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int HIKS_Pause(int hSession);

        //�ָ����ţ��ɹ����� 1��ʧ�ܷ���-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int HIKS_Resume(int hSession);

        //ֹͣ����,����Player�������˸ú�����Ͳ���Ҫ�ٵ���HIKS_Destroy �����ˣ��ɹ����� 0��ʧ�ܷ���-1
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int HIKS_Stop(int hSession);

        //����Player��ֻ��HIKS_OpenURL ����ʧ�ܵ�����µ����ɹ����� 0��ʧ�ܷ���-1��
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static int HIKS_Destroy(int hSession);
    }
}
