using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace winforms_z_buffer
{
    public class ZBuffer
    {
        // minVal is at the top of the stack
        double[,] zBufferMap;
        Size w;

        public ZBuffer(Size windowSize)
        {
            w = windowSize;
            zBufferMap = new double[windowSize.Width, windowSize.Height];
            for (int i = 0; i < windowSize.Width * windowSize.Height; i++)
                zBufferMap[i % windowSize.Width, Ut.F(i / windowSize.Width)] = double.MaxValue;
        }

        // get: boolean if z is higher or equal to point (=> true = can draw)
        // set: z at point
        public double this[double x, double y]
        {
            get
            {
                x = Ut.F(x) + (w.Width / 2);
                y = Ut.F(y) + (w.Height / 2);
                if (x > w.Width - 1 || x < 0 || y < 0 || y > w.Height - 1) return double.MinValue ;
                return zBufferMap[Ut.F(x), Ut.F(y)];
            }
            set
            {
                zBufferMap[Ut.F(x) + (w.Width / 2), Ut.F(y) + (w.Height / 2)] = (double)value;
            }
        }

        public override string ToString()
        {         
            var sb = new StringBuilder();
                                
            for (int i = 0; i < w.Width; i++)
            {
                for (int j = 0; j < w.Height; j++)
                {
                    if (zBufferMap[i, j] == double.MinValue)
                        sb.Append(",");
                    else
                        sb.Append(zBufferMap[i, j]);
                    sb.Append(" ");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
