using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    class CActionMetaManager : CMetaManager
    {
        public CActionMetaManager()
        {
            CActionType type = new CActionType();
            type.Name = "_MSActionType_";
            type.Desc = "������������";
            type.ConfigClass = "Config.CMSActionConfig";
            type.ConfigFormClass = "Config.FormMSActionConfig";
            type.ConfigControlClass = "Config.MSActionConfigControl";
            type.ActionClass = "Action.CMSAction";
            type.FileName = "Bin\\ExtentTypes\\actionlib.dll";
            type.Enabled = true;

            this.AppendType(type);

            type = new CActionType();
            type.Name = "_LEDActionType_";
            type.Desc = "LED��������";
            type.ConfigClass = "Config.CLEDActionConfig";
            type.ConfigFormClass = "Config.FormLEDActionConfig";
            type.ActionClass = "Action.CLEDAction";
            type.FileName = "Bin\\ExtentTypes\\actionlib.dll";
            type.Enabled = true;

            this.AppendType(type);

            type = new CActionType();
            type.Name = "_SoundActionType_";
            type.Desc = "������������";
            type.ConfigClass = "Config.CSoundActionConfig";
            //type.ConfigFormClass = "Config.FormSoundActionConfig";
            type.ActionClass = "Action.CSoundAction";
            type.FileName = "Bin\\ExtentTypes\\actionlib.dll";
            type.Enabled = true;

            this.AppendType(type);

            type = new CActionType();
            type.Name = "_LampActionType_";
            type.Desc = "������������";
            type.ConfigClass = "Config.CLampActionConfig";
            //type.ConfigFormClass = "Config.FormLampActionConfig";
            type.ActionClass = "Action.CLampAction";
            type.FileName = "Bin\\ExtentTypes\\actionlib.dll";
            type.Enabled = true;

            this.AppendType(type);

            type = new CActionType();
            type.Name = "_TrumpetActionType_";
            type.Desc = "������������";
            type.ConfigClass = "Config.CTrumpetActionConfig";
            //type.ConfigFormClass = "Config.FormLampActionConfig";
            type.ActionClass = "Action.CTrumpetAction";
            type.FileName = "Bin\\ExtentTypes\\actionlib.dll";
            type.Enabled = true;

            this.AppendType(type);

            //type = new CActionType();
            //type.Name = "_HKPTZActionType_";
            //type.Desc = "����PTZ��������";
            //type.ConfigClass = "Config.CPTZActionConfig";
            ////type.ConfigFormClass = "Config.FormLampActionConfig";
            //type.ActionClass = "Action.CPTZAction";
            //type.FileName = "Bin\\ExtentTypes\\actionlib.dll";
            //type.Enabled = true;

            this.AppendType(type);

            type = new CActionType();
            type.Name = "_ForegroundActionType_";
            type.Desc = "ǰ����������";
            type.ConfigClass = "Config.CForegroundActionConfig";
            type.ActionClass = "Action.CForegroundAction";
            type.FileName = "Bin\\ExtentTypes\\actionlib.dll";
            type.Enabled = true;

            this.AppendType(type);
        }
    }

    public class CMetaManageEnter : CMetaManageEnterBase
    {
        public CMetaManageEnter()
            : base()
        {
            Desc = "ϵͳͨ����������";
        }

        protected override IMetaManager CreateMetaManager()
        {
            return new CActionMetaManager();
        }
    }
}
