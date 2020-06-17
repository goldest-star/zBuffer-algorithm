using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace winforms_z_buffer
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((item, index) => (item, index));
    }

    public static class BitmapExtension
    {
        public static void SetPixelFast(this Bitmap bmp, int x, int y, Color color)
        {
            var newValues = new byte[] { color.B, color.G, color.R, 255 };

            BitmapData data = bmp.LockBits(
                new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb
                );

            if (
                data.Stride * y + 4 * x < data.Stride * data.Height
                && data.Stride * y + 4 * x >= 0
                && x * 4 < data.Stride
                && y < data.Height
                && x > 0
                )
                unsafe
                {
                    byte* ptr = (byte*)data.Scan0;

                    for (int i = 0; i < 4; i++)
                        ptr[data.Stride * y + 4 * x + i] = newValues[i];
                }

            bmp.UnlockBits(data);
        }
    }
}
