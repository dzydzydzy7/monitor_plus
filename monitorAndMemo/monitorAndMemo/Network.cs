using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; // Diagnostic 诊断

namespace monitorAndMemo
{
    class Network
    {
        public string name { get; set; }
        public long downSpeed { get; private set; }
        public long upSpeed { get; private set; }
        public bool isConnect {get; private set;}
        private long currentDownload, oldDownload;
        private long currentUpload, oldUpload;

        private PerformanceCounter downCounter, upCounter;

        public Network(string name)
        {
            this.name = name;
            downCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", name);
            upCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", name);
            oldDownload = downCounter.NextSample().RawValue;
            oldUpload = upCounter.NextSample().RawValue;
            if (checkConnect())
            {
                // nextSample().RawValue 可获取流量总量
                
            }
            else
            {
                downSpeed = 0;
                upSpeed = 0;
            }
        }

        public bool checkConnect()
        {
            isConnect = false;
            if (downCounter == null || upCounter == null)
                return false;
            if (downCounter.NextSample() == null || upCounter.NextSample() == null)
                return false;
            if (oldDownload > 0 || oldUpload > 0)
            {
                isConnect = true;
                return true;
            }
            else return true;
        }

        public void refresh()
        {
            if (checkConnect())
            {
                currentDownload = downCounter.NextSample().RawValue;
                currentUpload = upCounter.NextSample().RawValue;
                downSpeed = currentDownload - oldDownload;
                upSpeed = currentUpload - oldUpload;
                oldDownload = currentDownload;
                oldUpload = currentUpload;
            }
            else
            {
                downSpeed = 0;
                upSpeed = 0;
            }
        }

        private string format(long speed)
        {
            double n = speed / 1024.0;
            if (n >= 1000.0) return (n / 1024).ToString("0.00") + "MB/s";
            else if (n >= 100.0) return n.ToString("0.0") + "KB/s";
            else return n.ToString("0.00") + "KB/s";
        }

        public string getDownload()
        {
            return format(downSpeed);
        }

        public string getUpload()
        {
            return format(upSpeed);
        }
    }
}
