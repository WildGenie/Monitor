using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface IHKAlarmOutActionConfig : IActionConfig
    {
        string IP { get; }
        int Port { get; }
        string UserName { get; }
        string Password { get; }

        int OutputPort { get; }
    }

    public class CHKAlarmOutActionConfig : CActionConfig, IHKAlarmOutActionConfig
    {
        public CHKAlarmOutActionConfig()
            : base()
        {

        }

        public CHKAlarmOutActionConfig(string name)
            : base(name)
        {

        }

        //�������������DVR����Ϣ������Ϊ�ա���ָ��ʱ��ʾ�������������DVR�뱨����ƵԴ������DVR��ͬһ����
        #region �������������DVR

        public string IP
        {
            get { return StrValue("IP"); }
        }

        public int Port
        {
            get { return IntValue("Port"); }
        }

        public string UserName
        {
            get { return StrValue("UserName"); }
        }

        public string Password
        {
            get { return StrValue("Password"); }
        }

        #endregion

        //��0��ʼ��0xff��ʾȫ��
        public int OutputPort
        {
            get { return IntValue("OutputPort"); }
        }
    }
}
