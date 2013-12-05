using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using Config;

namespace Monitor
{
    public enum TVisionEventType
    {
        None = 0,			//�޼��
        Perimeter = 10,		//�ܽ���
        Leave = 20,		    //�������
        Remove = 30,		//�ƶ����
        FireDetect = 40,	//���ּ��
        FaceDetect = 80,	//�������
        CameraUnusual = 100	//��ͷ�쳣
    }

    public interface IVisionAlarm : IVisionMonitorAlarm
    {
        TVisionEventType EventType { get; }
        TGuardLevel GuardLevel { get; }
        int AreaIndex { get; }
        TAreaType AreaType { get; }                
        TAlertOpt AlertOpt { get; }
    }

    public class CVisionAlarm : CVisionMonitorAlarm, IVisionAlarm
    {
        private TVisionEventType mEventType = TVisionEventType.None;
        private TGuardLevel mGuardLevel = TGuardLevel.None;
        private int mAreaIndex = -1;
        private TAreaType mAreaType = TAreaType.All;
        private TAlertOpt mAlertOpt = TAlertOpt.Any;

        public CVisionAlarm(IVisionUser monitor)
            : base(monitor)
        {

        }

        public CVisionAlarm(IVisionUser monitor, string data)
            : base(monitor)
        {
            this.SetAlarmInfo(data);
        }

        public IVisionUser VisionUser
        {
            get { return Monitor as IVisionUser; }
        }

        public TVisionEventType EventType
        {
            get { return mEventType; }
            set { mEventType = value; }
        }

        public TGuardLevel GuardLevel 
        {
            get { return mGuardLevel; }
            set { mGuardLevel = value; }
        }

        public int AreaIndex
        {
            get { return mAreaIndex; }
            set { mAreaIndex = value; }
        }

        public TAreaType AreaType 
        {
            get { return mAreaType; }
            set { mAreaType = value; }
        }

        public TAlertOpt AlertOpt 
        {
            get { return mAlertOpt; }
            set { mAlertOpt = value; }
        }

        public string GetAlarmTypeDesc()
        {
            return string.Format("{0}({1})", GetAlarmTypeDesc(EventType, GuardLevel), GetAlertOptDesc());
        }

        public string GetAlertOptDesc()
        {
            return GetAlertOptDesc(AlertOpt);
        }

        public static string GetAlertOptDesc(TAlertOpt alertOpt)
        {
            StringBuilder sb = new StringBuilder("");

            if ((alertOpt & TAlertOpt.Stay) != TAlertOpt.Default)
                sb.Append("����");
            else if ((alertOpt & TAlertOpt.Wander) != TAlertOpt.Default)
                sb.Append("�ǻ�");
            else if ((alertOpt & TAlertOpt.Assemble) != TAlertOpt.Default)
                sb.Append("�ۼ�");
            else
            {
                if ((alertOpt & TAlertOpt.Left) != TAlertOpt.Default)
                    sb.Append("����");
                else if ((alertOpt & TAlertOpt.Right) != TAlertOpt.Default)
                    sb.Append("����");

                if ((alertOpt & TAlertOpt.Up) != TAlertOpt.Default)
                    sb.Append("����");
                else if ((alertOpt & TAlertOpt.Down) != TAlertOpt.Default)
                    sb.Append("����");


                if ((alertOpt & TAlertOpt.Enter) != TAlertOpt.Default)
                    sb.Append("����");
                else if ((alertOpt & TAlertOpt.Leave) != TAlertOpt.Default)
                    sb.Append("�뿪");
                else if ((alertOpt & TAlertOpt.Traverse) != TAlertOpt.Default)
                    sb.Append("��Խ");
            }

            return sb.ToString();

            //switch (alertOpt)
            //{
            //    case TAlertOpt.Enter:
            //        return "����";
            //    case TAlertOpt.Leave:
            //        return "�뿪";
            //    case TAlertOpt.Stay:
            //        return "����";
            //    case TAlertOpt.Wander:
            //        return "�ǻ�";
            //    case TAlertOpt.Traverse:
            //        return "��Խ";
            //    case TAlertOpt.Up:
            //        return "����";
            //    case TAlertOpt.Down:
            //        return "����";
            //    case TAlertOpt.Left:
            //        return "����";
            //    case TAlertOpt.Right:
            //        return "����";
            //    case TAlertOpt.Any:
            //        return "����";
            //    default:
            //        return "Ĭ��";
            //}
        }

        public static string GetAlarmTypeDesc(TVisionEventType type, TGuardLevel level)
        {
            switch (type)
            {
                case TVisionEventType.Perimeter:
                    return GetGuardLevelDesc(level);
                    //if (level == TGuardLevel.Prompt)
                    //    return "Խ����ʾ";
                    //else return "Խ�籨��";
                case TVisionEventType.Leave:
                    return "��������";
                case TVisionEventType.Remove:
                    return "�ƶ�����";
                case TVisionEventType.FireDetect:
                    return "���ֱ���";
                case TVisionEventType.FaceDetect:
                    return "�������";
                case TVisionEventType.CameraUnusual:
                    return "��ͷ�쳣";
                default:
                    return "" + type;
            }
        }

        public static string GetGuardLevelDesc(TGuardLevel level)
        {
            switch (level)
            {
                case TGuardLevel.Red:
                    return "Խ�籨��";
                case TGuardLevel.Yellow:
                    return "Խ����";
                case TGuardLevel.Green:
                    return "�Ǽ��";
                case TGuardLevel.Mask:
                    return "��˽";
                case TGuardLevel.Prompt:
                    return "Խ����ʾ";
                default:
                    return "�޾���";
            }
        }

        public override string ToXml()
        {
            StringBuilder sb = new StringBuilder("<AlarmInfo>");
            try
            {
                sb.Append("<SystemContext>" + SystemContext.MonitorSystem.Name + "</SystemContext>");
                sb.Append("<ID>" + ID + "</ID>");
                sb.Append("<Sender>" + Sender + "</Sender>");

                IVisionMonitorConfig config = this.Monitor.Config as IVisionMonitorConfig;
                if (config != null)
                {
                    sb.Append("<VideoSource>" + config.VisionParamConfig.VSName + "</VideoSource>");
                }
                sb.Append("<AlarmType>" + (int)EventType + "</AlarmType>");
                sb.Append("<GuardLevel>" + (int)GuardLevel + "</GuardLevel>");
                sb.Append("<AreaIndex>" + AreaIndex + "</AreaIndex>");
                sb.Append("<AreaType>" + (int)AreaType + "</AreaType>");
                sb.Append("<AlertOpt>" + (int)AlertOpt + "</AlertOpt>");
                sb.Append("<AlarmTime>" + AlarmTime.ToString("yyyy-MM-dd HH:mm:ss") + "</AlarmTime>");
                sb.Append("<TransactTime>" + TransactTime.ToString("yyyy-MM-dd HH:mm:ss") + "</TransactTime>");
                sb.Append("<Transactor>" + Transactor + "</Transactor>");
            }
            finally
            {
                sb.Append("</AlarmInfo>");
            }
            return sb.ToString();
        }

        public override string GetAlarmInfo()
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                AlarmImage.Save(ms, ImageFormat.Jpeg);

                StringBuilder sb = new StringBuilder(SystemContext.Name + "<SystemContext>");
                sb.Append(Monitor.Name + "<Monitor><MonitorAlarm>");
                sb.Append(ID + "<ID>");
                sb.Append(Sender + "<Sender>");
                sb.Append(Desc + "<Desc>");
                sb.Append((int)EventType + "<EventType>");
                sb.Append((int)GuardLevel + "<GuardLevel>");
                sb.Append(AreaIndex + "<AreaIndex>");
                sb.Append((int)AreaType + "<AreaType>");
                sb.Append((ushort)AlertOpt + "<AlertOpt>");
                sb.Append(AlarmTime.ToLongDateString() + " " + AlarmTime.ToLongTimeString() + "<AlarmTime>");
                sb.Append(Convert.ToBase64String(ms.GetBuffer()) + "<AlarmImage></MonitorAlarm>");

                return sb.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public override bool SetAlarmInfo(string data)
        {
            int index = data.IndexOf("<MonitorAlarm>");
            if (index>=0)
            {                
                int n = data.IndexOf("<ID>");
                ID = data.Substring(index + 14, n - index - 14);
                int m = data.IndexOf("<Sender>");
                Sender = data.Substring(n + 4, m - n - 4);
                n = data.IndexOf("<Desc>");
                Desc = data.Substring(m + 8, n - m - 8);
                m = data.IndexOf("<EventType>");
                EventType = (TVisionEventType)(Convert.ToInt32(data.Substring(n + 6, m - n - 6)));
                n = data.IndexOf("<GuardLevel>");
                GuardLevel = (TGuardLevel)(Convert.ToInt32(data.Substring(m + 11, n - m - 11)));
                m = data.IndexOf("<AreaIndex>");
                AreaIndex = Convert.ToInt32(data.Substring(n + 12, m - n - 12));
                n = data.IndexOf("<AreaType>");
                AreaType = (TAreaType)(Convert.ToInt32(data.Substring(m + 11, n - m - 11)));
                m = data.IndexOf("<AlertOpt>");
                AlertOpt = (TAlertOpt)(Convert.ToUInt16(data.Substring(n + 10, m - n - 10)));
                n = data.IndexOf("<AlarmTime>");
                string time = data.Substring(m + 10, n - m - 10);
                m = data.IndexOf("<AlarmImage>");

                string[] aa = { "-", " ", ":" };
                string[] tt = time.Split(aa, 6, StringSplitOptions.RemoveEmptyEntries);
                if (tt != null && tt.Length == 6)
                {
                    AlarmTime = new DateTime(Convert.ToInt32(tt[0]), Convert.ToInt32(tt[1]), Convert.ToInt32(tt[2]), Convert.ToInt32(tt[3]), Convert.ToInt32(tt[4]), Convert.ToInt32(tt[5]));
                }
                else AlarmTime = DateTime.Now;

                MemoryStream ms = new MemoryStream(Convert.FromBase64String(data.Substring(n + 11, m - n - 11)));
                Image image = Bitmap.FromStream(ms);

                if (image != null)
                {
                    try
                    {
                        AlarmImage = new Bitmap(image);
                    }
                    finally
                    {
                        image.Dispose();
                        ms.Close();
                        ms.Dispose();
                    }
                }

                return true;
            }
            return false;
        }
    }
}
