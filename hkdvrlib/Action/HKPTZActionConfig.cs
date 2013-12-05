using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface IHKPTZActionConfig : IActionConfig
    {
        string VSName { get; }
        int Interval { get; }
    }

    public class CHKPTZActionConfig : CActionConfig, IHKPTZActionConfig
    {
        public CHKPTZActionConfig()
            : base()
        {

        }

        public CHKPTZActionConfig(string name)
            : base(name)
        {

        }

        //PTZ��ƵԴ���ƣ�����Ϊ�ա���ָ��ʱ���ݱ�����ƵԴ��PTZVSName���Ի�ȡ
        public string VSName
        {
            get { return StrValue("VSName"); }
        }

        //PTZ���Ƽ��ʱ�䵥λ����(MS)
        public int Interval
        {
            get { return IntValue("Interval"); }
        }
    }
}
