using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// System.Runtime.InteropServices中的DllImportAttribute
// 可以用来定义用于访问非托管 API 的平台调用方法
using System.Runtime.InteropServices;

namespace monitorAndMemo
{
    class RAM
    {
        public struct MEMORYSTATUS
        {
            public uint dwLength; //当前结构体大小
            public uint dwMemoryLoad; //当前内存使用率
            public ulong ullTotalPhys; //总计物理内存大小
            public ulong ullAvailPhys; //可用物理内存大小
            public ulong ullTotalPageFile; //总计交换文件大小
            public ulong ullAvailPageFile; //总计交换文件大小
            public ulong ullTotalVirtual; //总计虚拟内存大小
            public ulong ullAvailVirtual; //可用虚拟内存大小
            public ulong ullAvailExtendedVirtual; //保留 这个值始终为0
        }

        // kernel32.dll是32位动态链接库文件，属于内核级文件。
        // 它控制着系统的内存管理、数据的输入输出操作和中断处理，
        // ref可以视作是C++的引用
        [DllImport("kernel32.dll")]
        public static extern void GlobalMemoryStatus(ref MEMORYSTATUS stat);

        private MEMORYSTATUS status;
        public int currentValue { get; private set; }

        public RAM()
        {
            GlobalMemoryStatus(ref status);
            currentValue = (int)status.dwMemoryLoad;
        }

        public void refresh()
        {
            GlobalMemoryStatus(ref status);
            currentValue = (int)status.dwMemoryLoad;
        }
    }
}
