using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winforms_z_buffer
{
    public class ZBuffer
    {
        double[,] zBufferMap;
        Size w;

        public ZBuffer(Size windowSize)
        {
            w = windowSize;
            zBufferMap = new double[windowSize.Width, windowSize.Height];
            for (int i = 0; i < windowSize.Width * windowSize.Height; i++)
                zBufferMap[i % windowSize.Width, i / windowSize.Width] = double.MinValue;
        }

        // get: true if z is higher or equal to point
        // set: z at point
        public object this[double x, double y, double z]
        {
            get
            {
                x = Ut.F(x) + (w.Width / 2);
                y = Ut.F(y) + (w.Height / 2);
                if (x > w.Width - 1 || x < 0 || y < 0 || y > w.Height - 1) return false;
                return (zBufferMap[Ut.F(x), Ut.F(y)] <= z);
            }
            set { zBufferMap[Ut.F(x) + 400, Ut.F(y) + 400] = (double)value; }
        }
    }
}
