using System;
using System.Management; 
using System.Collections.Generic;
using System.Text;

namespace EncryptLib
{
    /// Computer Information 
    public class CComputer
    {
        private string mCpuID = ""; //1.cpu���к�
        private string mMacAddress = ""; //2.mac���к�
        private string mDiskID = ""; //3.Ӳ��id
        private string mIpAddress = ""; //4.ip��ַ
        private string mLoginUserName = ""; //5.��¼�û���
        private string mComputerName = ""; //6.�������
        private string mSystemType = ""; //7.ϵͳ����
        private string mTotalPhysicalMemory = ""; //8. �ڴ��� ��λ��M 

        public CComputer()
        {
            mCpuID = GetCpuID();
            mMacAddress = GetMacAddress();
            mDiskID = GetDiskID();
            mIpAddress = GetIPAddress();
            mLoginUserName = GetUserName();
            mSystemType = GetSystemType();
            mTotalPhysicalMemory = GetTotalPhysicalMemory();
            mComputerName = GetComputerName();
        }

        public string CpuID
        {
            get { return mCpuID; }
        }

        public string MacAddress
        {
            get { return mMacAddress; }
        }

        public string DiskID
        {
            get { return mDiskID; }
        }

        public string IpAddress
        {
            get { return mIpAddress; }
        }

        public string LoginUserName
        {
            get { return mLoginUserName; }
        }

        public string ComputerName
        {
            get { return mComputerName; }
        }

        public string SystemType
        {
            get { return mSystemType; }
        }

        public string TotalPhysicalMemory
        {
            get { return mTotalPhysicalMemory; }
        }

        //1.��ȡCPU���кŴ��� 
        private string GetCpuID()
        {
            try
            {
                string cpuInfo = "";//cpu���к�
                ManagementClass mc = new ManagementClass("Win32_Processor");                
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo += mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //2.��ȡ����Ӳ����ַ 
        private string GetMacAddress()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //3.��ȡӲ��ID 
        private string GetDiskID()
        {
            try
            {
                String HDid = "";
                //ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementClass mc = new ManagementClass("Win32_PhysicalMedia");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    //HDid = (string)mo.Properties["Model"].Value;
                    HDid += (string)mo.Properties["SerialNumber"].Value;                    
                }
                moc = null;
                mc = null;
                return HDid;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //4.��ȡIP��ַ 
        private string GetIPAddress()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        //st=mo["IpAddress"].ToString();
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        if (ar != null)
                        {
                            st = ar.GetValue(0).ToString();
                            if (!st.Equals(""))
                                break;
                        }
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //5.����ϵͳ�ĵ�¼�û��� 
        private string GetUserName()
        {
            try
            {
                return Environment.UserName;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //6.��ȡ�������
        private string GetComputerName()
        {
            try
            {
                return System.Environment.MachineName;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }

        //7 PC���� 
        private string GetSystemType()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["SystemType"].ToString();
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }

        //8. �����ڴ� 
        private string GetTotalPhysicalMemory()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["TotalPhysicalMemory"].ToString();
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }
    }
}