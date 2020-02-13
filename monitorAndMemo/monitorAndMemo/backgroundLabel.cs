using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 11行的基类Label需要using System.Windows.Forms
using System.Windows.Forms;
// brush、pen等需要using System.Drawing
using System.Drawing;

namespace monitorAndMemo
{
    // 继承Label重写OnPaintBackground方法
    // 实现用背景显示百分比
    public partial class BackgroundLabel : Label
    {
        bool BackColorVisible = true;
        bool BorderColorVisible = true;
        public int value = 0;
        int threshold = 70; // 10以上由绿变红

        public BackgroundLabel() { }
        
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            if (BackColorVisible)
            {
                if (value >= threshold)
                {
                    Brush brush = new SolidBrush(Color.Red);
                    int height = Height * value / 100;
                    e.Graphics.FillRectangle(brush, 0, Height - height, Width, Height);
                }
                else
                {
                    Brush brush = new SolidBrush(Color.Green);
                    int height = Height * value / 100;
                    e.Graphics.FillRectangle(brush, 0, Height - height, Width, Height);
                }
            }
            if (BorderColorVisible && value >= threshold)
            {
                Pen pen = new Pen(Color.Red, 1);
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            }
        }
    }
}
