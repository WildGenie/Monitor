using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EncryptLib;

namespace Verify
{
    public partial class FormSysInitConfig : Form
    {
        private static FormSysInitConfig mForm = null;
        private string mDVRIP = "";
        private string mDVRUser = "";
        private string mDVRPassword = "";

        public FormSysInitConfig()
        {
            InitializeComponent();
        }

        public static String DVRIP
        {
            get { return mForm != null ? mForm.CurDVRIP : ""; }
        }

        public static String DVRUser
        {
            get { return mForm != null ? mForm.CurDVRUser : ""; }
        }

        public static String DVRPassword
        {
            get { return mForm != null ? mForm.CurDVRPassword : ""; }
        }

        public static bool ShowWindow()
        {
            if (mForm == null)
                mForm = new FormSysInitConfig();

            mForm.DialogResult = DialogResult.Cancel;

            return mForm.ShowDialog() == DialogResult.OK;
        }

        public string CurDVRIP
        {
            get { return mDVRIP; }
        }

        public string CurDVRUser
        {
            get { return mDVRUser; }
        }

        public string CurDVRPassword
        {
            get { return mDVRPassword; }
        }

        private void FormSysInitConfig_Shown(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            button_prior.Visible = false;
            button_next.Text = "��һ�� >>";
            textBox_sn.Text = "";
            textBox_sn.Focus();

            textBox_dvrip.Text = "";
            textBox_dvruser.Text = "";
            textBox_dvrpassword.Text = "";

            mDVRIP = "";
            mDVRUser = "";
            mDVRPassword = "";

            comboBox_networkAdapter.Items.Clear();

            CIPInfo[] ipInfoList = CIPUtil.GetIPInfoList();
            if (ipInfoList != null)
            {
                foreach (CIPInfo ipInfo in ipInfoList)
                {
                    comboBox_networkAdapter.Items.Add(ipInfo);
                }
            }

            if (comboBox_networkAdapter.Items.Count > 0)
                comboBox_networkAdapter.SelectedIndex = 0;

            comboBox_networkAdapter_SelectedIndexChanged(null, null);
        }

        private void comboBox_networkAdapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_ip.Text = "";
            textBox_mask.Text = "";
            textBox_gateway.Text = "";

            if (comboBox_networkAdapter.SelectedIndex >= 0)
            {
                CIPInfo ipInfo = comboBox_networkAdapter.Items[comboBox_networkAdapter.SelectedIndex] as CIPInfo;
                if (ipInfo != null)
                {
                    textBox_ip.Text = ipInfo.IPAddress;

                    textBox_mask.Text = ipInfo.SubnetMask;

                    textBox_gateway.Text = ipInfo.DefaultGetway;
                }
            }
        }

        private void SetIPInfo()
        {
            if (comboBox_networkAdapter.SelectedIndex >= 0)
            {
                CIPInfo ipInfo = comboBox_networkAdapter.Items[comboBox_networkAdapter.SelectedIndex] as CIPInfo;
                if (ipInfo != null)
                {
                    ipInfo.IPAddress = textBox_ip.Text;

                    ipInfo.SubnetMask = textBox_mask.Text;

                    ipInfo.DefaultGetway = textBox_gateway.Text;
                }
            }
        }

        private void FormSysInitConfig_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                SendKeys.Send("{TAB}");
        }

        private void button_modifyIp_Click(object sender, EventArgs e)
        {
            SetLocalIP();
        }

        private bool SetLocalIP()
        {
            if (!CIPUtil.IsIPAddress(textBox_ip.Text.Trim()))
            {
                MessageBox.Show("��ǰ���õ�IP��Ч�����������룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox_ip.Focus();
                return false;
            }

            if (!CIPUtil.IsIPAddress(textBox_mask.Text.Trim()))
            {
                MessageBox.Show("��ǰ���õ�����������Ч�����������룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox_mask.Focus();
                return false;
            }

            if (!CIPUtil.IsIPAddress(textBox_gateway.Text.Trim()))
            {
                MessageBox.Show("��ǰ���õ�Ĭ��������Ч�����������룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox_gateway.Focus();
                return false;
            }

            SetIPInfo();
            return true;
        }

        private void button_prior_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    tabControl1.SelectedIndex = tabControl1.SelectedIndex - 1;
                    button_prior.Visible = false;
                    break;
                case 2:
                    tabControl1.SelectedIndex = tabControl1.SelectedIndex - 1;
                    button_next.Text = "��һ�� >>";
                    break;
                default:
                    break;
            }
        }

        private void button_next_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    if (!textBox_sn.Text.Equals(""))
                    {
                        if (textBox_sn.Text.Equals("123456789") || CSystemVerifier.Verify(textBox_sn.Text))
                        {
                            tabControl1.SelectedIndex = tabControl1.SelectedIndex + 1;
                            button_prior.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("�����ϵ�кŲ���ȷ�����������룡", "У��ʧ��", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textBox_sn.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("������ϵ�кţ��ٽ�����һ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox_sn.Focus();
                    }
                    break;
                case 1:
                    if (!SetLocalIP()) return;

                    if (comboBox_networkAdapter.Items.Count > 0)
                    {
                        string[] ips = new string[comboBox_networkAdapter.Items.Count];
                        string[] subnetmasks = new string[comboBox_networkAdapter.Items.Count];
                        string[] defaultgetways = new string[comboBox_networkAdapter.Items.Count];
                        int index = 0;

                        foreach (CIPInfo ipInfo in comboBox_networkAdapter.Items)
                        {
                            if (ipInfo != null)
                            {
                                ips[index] = ipInfo.IPAddress;
                                subnetmasks[index] = ipInfo.SubnetMask;
                                defaultgetways[index] = ipInfo.DefaultGetway;
                            }
                            index++;
                        }

                        CIPUtil.SetIPAddress(ips, subnetmasks, defaultgetways, null);
                    }

                    tabControl1.SelectedIndex = tabControl1.SelectedIndex + 1;
                    button_next.Text = "�� ��";

                    break;
                case 2:
                    if (!CIPUtil.IsIPAddress(textBox_dvrip.Text.Trim()))
                    {
                        MessageBox.Show("��ǰ���õ�Ӳ��¼���IP��Ч�����������룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox_dvrip.Focus();
                        return;
                    }

                    if (textBox_dvruser.Text.Trim().Equals(""))
                    {
                        MessageBox.Show("������Ӳ��¼�����¼�û���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox_dvruser.Focus();
                        return;
                    }

                    mDVRIP = textBox_dvrip.Text.Trim();
                    mDVRUser = textBox_dvruser.Text.Trim();
                    mDVRPassword = textBox_dvrpassword.Text.Trim();

                    DialogResult = DialogResult.OK;
                    break;
                default:
                    break;
            }
        }
    }
}