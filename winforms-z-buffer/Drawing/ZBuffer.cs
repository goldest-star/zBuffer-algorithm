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
        public Dictionary<Point, double> zBufferDict = new Dictionary<Point, double>();
        double[,] zBufferMap;
        Size w;

        public ZBuffer(Size windowSize)
        {
            w = windowSize;
            zBufferMap = new double[windowSize.Width, windowSize.Height];
            for (int i = 0; i < windowSize.Width * windowSize.Height; i++)
                zBufferMap[i % windowSize.Width, Ut.F(i / windowSize.Width)] = double.MinValue;
        }

        // get: boolean if z is higher or equal to point (=> true = can draw)
        // set: z at point
        public object this[double x, double y, double z]
        {
            get
            {
                x = Ut.F(x) + (w.Width / 2);
                y = Ut.F(y) + (w.Height / 2);
                if (x > w.Width - 1 || x < 0 || y < 0 || y > w.Height - 1) return false;
                return (zBufferMap[Ut.F(x), Ut.F(y)] > z);
            }
            set
            {
                zBufferMap[Ut.F(x) + (w.Width / 2), Ut.F(y) + (w.Height / 2)] = (double)value;
            }
        }

        public double this[Point point]
        {
            get
            {
                if (!zBufferDict.ContainsKey(point))
                    return double.MaxValue;
                return zBufferDict[point];
            }
            set
            {
                if (!zBufferDict.ContainsKey(point))
                    zBufferDict.Add(point, (double)(value));
                else
                    zBufferDict[point] = (double)value;
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
