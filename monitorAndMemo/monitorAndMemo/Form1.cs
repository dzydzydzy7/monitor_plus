using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics; //
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// System.Runtime.InteropServices中的DllImportAttribute
// 可以用来定义用于访问非托管 API 的平台调用方法
using System.Runtime.InteropServices;

namespace monitorAndMemo
{
    public partial class Form1 : Form
    {

        private CPU cpu;
        private RAM ram;
        private Disk disk;
        private Network network = null;
        Form2 form2;

        public Form1()
        {
            cpu = new CPU();
            ram = new RAM();
            disk = new Disk();
            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");
            foreach (string name in category.GetInstanceNames())
            {
                if (name == "MS TCP Loopback interface")
                    continue;
                // 这可不是个好方法
                if (name == "Intel[R] Dual Band Wireless-AC 3168")
                {
                    network = new Network(name);
                }
                Network n = new Network(name);
                if (n.isConnect)
                {
                    network = n;
                    break;
                }
                
            }
            InitializeComponent();

        }

        //[DllImport("user32.dll")]
        //public static extern bool ReleaseCapture();
        //[DllImport("user32.dll")]
        //public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        
        //初始化鼠标位置
        bool beginMove = false;
        int currentXPosition;
        int currentYPosition;

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.value = ram.currentValue;
            label1.Text = "CPU:" + cpu.currentValue.ToString() + "%";
            label2.Text = "RAM\n" + ram.currentValue.ToString() + "%";
            label3.Text = "DISK:" + disk.currentValue.ToString() + "%";
            label4.Text = "  " + network.getDownload();
            label5.Text = "  " + network.getUpload();
            label6.Text = "^^";
            form2 = new Form2();
            form2.Left = this.Left;
            form2.Top = this.Top - form2.Height;
            form2.Visible = false;
            //form2.Show(); // 去掉
            //form2.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cpu.refresh();
            label1.Text = "CPU:" + cpu.currentValue.ToString() + "%";
            ram.refresh();
            label2.value = ram.currentValue;
            label2.Text = "RAM\n" + ram.currentValue.ToString() + "%";
            disk.refresh();
            label3.Text = "DISK:" + disk.currentValue.ToString() + "%";
            network.refresh();
            label4.Text = "  " + network.getDownload();
            label5.Text = "  " + network.getUpload();
            
            if(!form2.Visible)this.TopMost = true;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_TOOLWINDOW = 0x80;
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_TOOLWINDOW;      // 不显示在Alt+Tab
                return cp;
            }
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                beginMove = true;
                currentXPosition = MousePosition.X;//鼠标的x坐标为当前窗体左上角x坐标
                currentYPosition = MousePosition.Y;//鼠标的y坐标为当前窗体左上角y坐标
            }
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            if (beginMove)
            {
                //根据鼠标x坐标确定窗体的左边坐标x
                this.Left += MousePosition.X - currentXPosition;
                //根据鼠标的y坐标窗体的顶部，即Y坐标
                this.Top += MousePosition.Y - currentYPosition;
                currentXPosition = MousePosition.X;
                currentYPosition = MousePosition.Y;

                form2.Left = this.Left;
                form2.Top = this.Top - form2.Height;
            }
        }

        private void label2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // 设置为初始状态
                currentXPosition = 0;
                currentYPosition = 0;
                beginMove = false;
            }
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Visible = !this.Visible;
            form2.Visible = false;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            if(form2.Visible == false)
            {
                form2.Left = this.Left;
                form2.Top = this.Top - form2.Height;
                label6.Text = "↓";
                form2.Visible = true;
            }
            else
            {
                form2.Left = this.Left;
                form2.Top = this.Top - form2.Height;
                label6.Text = "^^";
                form2.Visible = false;
            }
        }

        private void 启动MySQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            run_Cmd("net start MySQL");
        }

        private String run_Cmd(String cmd)
        {
            cmd = cmd.Trim().TrimEnd('&') + "&exit";//说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            //p.StartInfo.WorkingDirectory = workingDir;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(cmd);
            p.StandardInput.AutoFlush = true;
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }

        private void 关闭MySQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            run_Cmd("net stop MySQL");
        }
    }
}
