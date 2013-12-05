using System;
using System.Collections.Generic;
using System.Text;

namespace PTZ
{
    #region ���ݽṹ����

    //������(bps)
    public enum TBaudRate
    {
        pbs_50 = 0,
        pbs_75 = 1,
        pbs_110 = 2,
        pbs_150 = 3,
        pbs_300 = 4,
        pbs_600 = 5,
        pbs_1200 = 6,
        pbs_2400 = 7,
        pbs_4800 = 8,
        pbs_9600 = 9,
        pbs_19200 = 10,
        pbs_38400 = 11,
        pbs_57600 = 12,
        pbs_76800 = 13,
        pbs_115_2k = 14
    };

    //����λ
    public enum TDataBit
    {
        dbit_5 = 0, //5λ
        dbit_6 = 1, //6λ
        dbit_7 = 2, //7λ
        dbit_8 = 3  //8λ
    };

    //ֹͣλ
    public enum TStopBit
    {
        sbit_1 = 0, //1λ
        sbit_2 = 1  //2λ
    };

    //У��
    public enum TParity
    {
        none = 0, //��У��
        odd = 1, //��У��
        even = 2  //żУ��
    };

    //����
    public enum TFlowControl
    {
        none = 0, //��
        soft = 1, //������
        hard = 2  //Ӳ����
    };

    //��̨����������
    public enum TDecoderType
    {
        YOULI = 0,
        LILIN_1016 = 1,
        LILIN_820 = 2,
        PELCO_P = 3,
        DM_QUICKBALL = 4,
        HD600 = 5,
        JC4116 = 6,
        PELCO_DWX = 7,
        PELCO_D = 8,
        VCOM_VC_2000 = 9,
        NETSTREAMER = 10,
        SAE = 11,
        SAMSUNG = 12,
        KALATEL_KTD_312 = 13,
        CELOTEX = 14,
        TLPELCO_P = 15,
        TL_HHX2000 = 16,
        BBV = 17,
        RM110 = 18,
        KC3360S = 19,
        ACES = 20,
        ALSON = 21,
        INV3609HD = 22,
        HOWELL = 23,
        TC_PELCO_P = 24,
        TC_PELCO_D = 25,
        AUTO_M = 26,
        AUTO_H = 27,
        ANTEN = 28,
        CHANGLIN = 29,
        DELTADOME = 30,
        XYM_12 = 31,
        ADR8060 = 32,
        EVI = 33,
        Demo_Speed = 34,
        DM_PELCO_D = 35,
        ST_832 = 36,
        LC_D2104 = 37,
        HUNTER = 38,
        A01 = 39,
        TECHWIN = 40,
        WEIHAN = 41,
        LG = 42,
        D_MAX = 43,
        PANASONIC = 44,
        KTD_348 = 45,
        INFINOVA = 46,
        LILIN = 47,
        IDOME_IVIEW_LCU = 48,
        DENNARD_DOME = 49,
        PHLIPS = 50,
        SAMPLE = 51,
        PLD = 52,
        PARCO = 53,
        HY = 54,
        NAIJIE = 55,
        CAT_KING = 56,
        YH_06 = 57,
        SP9096X = 58,
        M_PANEL = 59,
        M_MV2050 = 60,
        SAE_QUICK = 61,
        PEARMAIN = 62,
        NKO8G = 63,
        DAHUA = 64,
        TX_CONTROL_232 = 65,
        VCL_SPEED_DOME = 66,
        ST_2C160 = 67,
        TDWY = 68,
        TWHC = 69,
        USNT = 70,
        KALLATE_NVD2200PS = 71,
        VIDO_B01 = 72,
        LG_MULTIX = 73,
        ENKEL = 74,
        YT_PELCOD = 75,
        HIKVISION = 76,
        PE60 = 77,
        LiAo = 78,
        NK16 = 79,
        DaLi = 80,
        HN_4304 = 81,
        VIDEOTEC = 82,
        HNDCB = 83,
        Lion_2007 = 84,
        LG_LVC_C372 = 85,
        Gold_Video = 86,
        NVD1600PS = 87
    };

    //dwPTZCommand����̨��������
    public enum PTZCommand
    {
        LIGHT_PWRON = 2, 		/* ��ͨ�ƹ��Դ */
        WIPER_PWRON = 3,		/* ��ͨ��ˢ���� */
        FAN_PWRON = 4, 			/* ��ͨ���ȿ��� */
        HEATER_PWRON = 5, 		/* ��ͨ���������� */
        AUX_PWRON1 = 6, 		/* ��ͨ�����豸1���� */
        AUX_PWRON2 = 7, 		/* ��ͨ�����豸2���� */
        ZOOM_IN = 11,     		/* ������(���ʱ��) */
        ZOOM_OUT = 12,    		/* �����С(���ʱ�С) */
        FOCUS_NEAR = 13,     	/* ����ǰ�� */
        FOCUS_FAR = 14,   		/* ������ */
        IRIS_OPEN = 15,     	/* ��Ȧ���� */
        IRIS_CLOSE = 16,    	/* ��Ȧ��С */
        TILT_UP = 21, 			/* ��̨���� */
        TILT_DOWN = 22,			/* ��̨�¸� */
        PAN_LEFT = 23, 			/* ��̨��ת */
        PAN_RIGHT = 24, 		/* ��̨��ת */
        UP_LEFT = 25, 		    /* ��̨��������ת */
        UP_RIGHT = 26, 		    /* ��̨��������ת */
        DOWN_LEFT = 27, 		/* ��̨�¸�����ת */
        DOWN_RIGHT = 28, 		/* ��̨�¸�����ת */
        PAN_AUTO = 29		    /* ��̨������ٶ������Զ�ɨ�� */
    };

    //dwPTZPresetCmd����̨Ԥ��λ����:
    public enum PTZPresetCmd
    {
        SET_PRESET = 8,		    /* ����Ԥ�õ� */
        CLE_PRESET = 9,	 	    /* ���Ԥ�õ� */
        GOTO_PRESET = 39 	    /* ת��Ԥ�õ� */
    };

    //dwPTZCruiseCmd����̨Ѳ����������
    public enum PTZCruiseCmd
    {
        FILL_PRE_SEQ = 30,	    // ��Ԥ�õ����Ѳ������ 
        SET_SEQ_DWELL = 31,	    // ����Ѳ����ͣ��ʱ�� 
        SET_SEQ_SPEED = 32,	    // ����Ѳ���ٶ� 
        CLE_PRE_SEQ = 33,	    // ��Ԥ�õ��Ѳ��������ɾ�� 
        RUN_SEQ = 37,	        // ��ʼѲ�� 
        STOP_SEQ = 38	        // ֹͣѲ��
    };

    //dwPTZTrackCmd: ��̨�켣����:
    public enum PTZTrackCmd
    {
        STA_MEM_CRUISE = 34,	// ��ʼ��¼�켣 
        STO_MEM_CRUISE = 35,	// ֹͣ��¼�켣 
        RUN_CRUISE = 36         // ��ʼ�켣
    };

    public struct DecoderInfo
    {
        public TBaudRate BaudRate;	        /* ������(bps)*/
        public TDataBit DataBit;		    /* ����λ*/
        public TStopBit StopBit;		    /* ֹͣλ*/
        public TParity Parity;		        /* У��*/
        public TFlowControl Flowcontrol;	/* ���� */
        public TDecoderType DecoderType;	/* ���������� */
        public ushort DecoderAddress;       /* ��������ַ:0 - 255*/
    };

    #endregion

    //��̨���ƽӿ�
    public interface IPTZCtrl
    {
        //�Ƿ��ܿ�����̨
        bool CanCtrl { get; }
        //��С�ٶ�ֵ
        int MinSpeed { get; }
        //����ٶ�ֵ
        int MaxSpeed { get; }

        #region ��̨����

        //��̨����
        bool TiltUp();
        bool TiltUp(int speed);

        //��̨�¸�
        bool TiltDown();
        bool TiltDown(int speed);

        //��̨��ת
        bool PanLeft();
        bool PanLeft(int speed);

        //��̨��ת
        bool PanRight();
        bool PanRight(int speed);

        //��̨��������ת
        bool UpLeft();
        bool UpLeft(int speed);

        //��̨��������ת
        bool UpRight();
        bool UpRight(int speed);

        //��̨�¸�����ת
        bool DownLeft();
        bool DownLeft(int speed);

        //��̨�¸�����ת
        bool DownRight();
        bool DownRight(int speed);

        //������(���ʱ��)
        bool ZoomIn();
        bool ZoomIn(int speed);

        //�����С(���ʱ�С)
        bool ZoomOut();
        bool ZoomOut(int speed);

        //����ǰ��(����)
        bool FocusNear();
        bool FocusNear(int speed);

        //������(��Զ)
        bool FocusFar();
        bool FocusFar(int speed);

        //��Ȧ����
        bool IrisOpen();
        bool IrisOpen(int speed);

        //��Ȧ��С
        bool IrisClose();
        bool IrisClose(int speed);

        //��ʼ����
        bool StartCtrl(PTZCommand cmd);
        bool StartCtrl(PTZCommand cmd, int speed);

        //ֹͣ����
        bool StopCtrl();

        //ֱ�ӿ���
        bool DirectCtrl(string cmd);

        #endregion

        #region Ԥ��λ

        //Ԥ��λ����
        bool Preset(PTZPresetCmd cmd, int preset);

        //����Ԥ��λ
        bool SetPreset(int preset);

        //�Ƴ�Ԥ��λ
        bool RemovePreset(int preset);

        //��λ��Ԥ��λ
        bool GotoPreset(int preset);

        #endregion

        #region Ѳ��

        //Ѳ������
        bool Cruise(PTZCruiseCmd cmd, byte route, byte point, int input);

        #endregion

        #region �켣

        //�켣����
        bool Track(PTZTrackCmd cmd);

        #endregion

        #region �����

        //��ȡ�������Ϣ
        bool GetDecodeInfo(ref DecoderInfo decoderInfo);

        //���ñ������Ϣ
        bool SetDecodeInfo(ref DecoderInfo decoderInfo);

        #endregion

        //��λ
        void Reset();
    }

    //��̨���ƻ���
    public abstract class CPTZCtrl : IPTZCtrl
    {
        private object mCtrlLockObj = new object();
        private bool mIsCtrl = false;
        private int mCtrlCommand = 0;
        private int mCtrlSpeed = 0;
        private int mMinSpeed = 0;    
        private int mMaxSpeed = 100;

        public CPTZCtrl()
        {
            //
        }

        #region ��̨����

        public int CtrlCommand
        {
            get { return mCtrlCommand; }
            protected set
            {
                mCtrlCommand = value;
            }
        }

        public int CtrlSpeed
        {
            get { return mCtrlSpeed; }
            protected set
            {
                mCtrlSpeed = value;
            }
        }

        protected bool IsCtrl
        {
            get { return mIsCtrl; }
            set { mIsCtrl = value; }
        }

        public virtual bool DirectCtrl(string cmd)
        {
            return false;
        }

        protected virtual int TranslatePTZCommand(int cmd)
        {
            return cmd;
        }

        protected virtual bool DoStartCtrl(int cmd, int speed)
        {
            return false;
        }

        public virtual bool StartCtrl(PTZCommand cmd)
        {
            return StartCtrl(cmd, 0);
        }

        public virtual bool StartCtrl(PTZCommand cmd, int speed)
        {
            lock (mCtrlLockObj)
            {
                if (!IsCtrl)
                {
                    IsCtrl = true;
                    CtrlCommand = TranslatePTZCommand((int)cmd);

                    if (speed < MinSpeed)
                        CtrlSpeed = MinSpeed;
                    else if (speed > MaxSpeed)
                        CtrlSpeed = MaxSpeed;
                    else
                        CtrlSpeed = speed;

                    return DoStartCtrl(CtrlCommand, CtrlSpeed);
                }
                return false;
            }
        }

        protected virtual bool DoStopCtrl(int cmd, int speed)
        {
            return false;
        }

        public virtual bool StopCtrl()
        {
            lock (mCtrlLockObj)
            {
                if (IsCtrl)
                {
                    try
                    {
                        return DoStopCtrl(CtrlCommand, CtrlSpeed);
                    }
                    finally
                    {
                        IsCtrl = false;
                        CtrlCommand = 0;
                        CtrlSpeed = 0;
                    }
                }
                return false;
            }
        }        

        public virtual bool TiltUp()
        {
            return TiltUp(0);
        }

        public virtual bool TiltUp(int speed)
        {            
            return StartCtrl(PTZCommand.TILT_UP, speed);
        }

        public virtual bool TiltDown()
        {
            return TiltDown(0);
        }

        public virtual bool TiltDown(int speed)
        {
            return StartCtrl(PTZCommand.TILT_DOWN, speed);
        }

        public virtual bool PanLeft()
        {
            return PanLeft(0);
        }

        public virtual bool PanLeft(int speed)
        {
            return StartCtrl(PTZCommand.PAN_LEFT, speed);
        }

        public virtual bool PanRight()
        {
            return PanRight(0);
        }

        public virtual bool PanRight(int speed)
        {
            return StartCtrl(PTZCommand.PAN_RIGHT, speed);
        }

        public virtual bool UpLeft()
        {
            return UpLeft(0);
        }

        public virtual bool UpLeft(int speed)
        {
            return StartCtrl(PTZCommand.UP_LEFT, speed);
        }

        public virtual bool UpRight()
        {
            return UpRight(0);
        }

        public virtual bool UpRight(int speed)
        {
            return StartCtrl(PTZCommand.UP_RIGHT, speed);
        }

        public virtual bool DownLeft()
        {
            return DownLeft(0);
        }

        public virtual bool DownLeft(int speed)
        {
            return StartCtrl(PTZCommand.DOWN_LEFT, speed);
        }

        public virtual bool DownRight()
        {
            return DownRight(0);
        }

        public virtual bool DownRight(int speed)
        {
            return StartCtrl(PTZCommand.DOWN_RIGHT, speed);
        }

        public virtual bool ZoomIn()
        {
            return ZoomIn(0);
        }

        public virtual bool ZoomIn(int speed)
        {
            return StartCtrl(PTZCommand.ZOOM_IN, speed);
        }

        public virtual bool ZoomOut()
        {
            return ZoomOut(0);
        }

        public virtual bool ZoomOut(int speed)
        {
            return StartCtrl(PTZCommand.ZOOM_OUT, speed);
        }

        public virtual bool FocusNear()
        {
            return FocusNear(0);
        }

        public virtual bool FocusNear(int speed)
        {
            return StartCtrl(PTZCommand.FOCUS_NEAR, speed);
        }

        public virtual bool FocusFar()
        {
            return FocusFar(0);
        }

        public virtual bool FocusFar(int speed)
        {
            return StartCtrl(PTZCommand.FOCUS_FAR, speed);
        }

        public virtual bool IrisOpen()
        {
            return IrisOpen(0);
        }

        public virtual bool IrisOpen(int speed)
        {
            return StartCtrl(PTZCommand.IRIS_OPEN, speed);
        }

        public virtual bool IrisClose()
        {
            return IrisClose(0);
        }

        public virtual bool IrisClose(int speed)
        {
            return StartCtrl(PTZCommand.IRIS_CLOSE, speed);
        }

        #endregion

        #region Ԥ��λ

        protected virtual int TranslatePresetCommand(int cmd)
        {
            return cmd;
        }

        protected virtual bool DoPreset(int cmd, int preset)
        {
            return false;
        }

        public virtual bool Preset(PTZPresetCmd cmd, int preset)
        {
            return DoPreset(TranslatePresetCommand((int)cmd), preset);
        }

        public virtual bool SetPreset(int preset)
        {
            return Preset(PTZPresetCmd.SET_PRESET, preset);
        }

        public virtual bool RemovePreset(int preset)
        {
            return Preset(PTZPresetCmd.CLE_PRESET, preset);
        }

        public virtual bool GotoPreset(int preset)
        {
            return Preset(PTZPresetCmd.GOTO_PRESET, preset);
        }

        #endregion

        #region Ѳ��

        protected virtual int TranslateCruiseCommand(int cmd)
        {
            return cmd;
        }

        protected virtual bool DoCruise(int cmd, byte route, byte point, int input)
        {
            return false;
        }

        public virtual bool Cruise(PTZCruiseCmd cmd, byte route, byte point, int input)
        {
            return DoCruise(TranslateCruiseCommand((int)cmd), route, point, input);
        }

        #endregion

        #region �켣

        protected virtual int TranslateTrackCommand(int cmd)
        {
            return cmd;
        }

        protected virtual bool DoTrack(int cmd)
        {
            return false;
        }

        public virtual bool Track(PTZTrackCmd cmd)
        {
            return DoTrack(TranslateTrackCommand((int)cmd));
        }

        #endregion

        #region �����

        public virtual bool GetDecodeInfo(ref DecoderInfo decoderInfo)
        {
            return false;
        }

        public virtual bool SetDecodeInfo(ref DecoderInfo decoderInfo)
        {
            return false;
        }

        #endregion

        //�Ƿ��ܿ�����̨
        public virtual bool CanCtrl 
        {
            get { return false; }
        }

        public int MinSpeed
        {
            get { return mMinSpeed; }
            protected set
            {
                mMinSpeed = value;
            }
        }

        public int MaxSpeed
        {
            get { return mMaxSpeed; }
            protected set
            {
                mMaxSpeed = value;
            }
        }

        //��λ
        public virtual void Reset()
        {
            //
        }
    }
}
