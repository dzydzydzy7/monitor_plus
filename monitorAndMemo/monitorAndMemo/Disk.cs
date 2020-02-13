using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; // Diagnostic 诊断

namespace monitorAndMemo
{
    class Disk
    {
        // windows 性能计数器组件
        private PerformanceCounter performanceCounter;

        public int currentValue { get; private set; }

        public Disk()
        {
            performanceCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
            currentValue = (int)performanceCounter.NextValue();
        }

        public void refresh()
        {
            int current = (int)performanceCounter.NextValue();
            if (current > 100) currentValue = 100;
            else currentValue = current;
        }
    }
}
