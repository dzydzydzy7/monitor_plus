using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; // Diagnostic 诊断

namespace monitorAndMemo
{
    class CPU
    {
        // windows 性能计数器组件
        private PerformanceCounter performanceCounter;

        public int currentValue { get; private set; }

        public CPU()
        {
            performanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            currentValue = (int)performanceCounter.NextValue();
        }

        public void refresh()
        {
            currentValue = (int)performanceCounter.NextValue();
        }
    }
}
