﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

        public static Color GetPixelFast(this Bitmap bmp, int x, int y)
        {
            BitmapData data = bmp.LockBits(
                new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb
                );

            Color col = Color.FromArgb(0, 0, 0, 0); ;

            if (data.Stride * y + 4 * x < data.Stride * data.Height && data.Stride * y + 4 * x >= 0)
                unsafe
                {
                    byte* ptr = (byte*)data.Scan0;

                    col = Color.FromArgb(
                        ptr[data.Stride * y + 4 * x + 3],
                        ptr[data.Stride * y + 4 * x + 2],
                        ptr[data.Stride * y + 4 * x + 1],
                        ptr[data.Stride * y + 4 * x + 0]
                    );
                }

            bmp.UnlockBits(data);

            return col;
        }

        public static Dictionary<Point, Color> GetPixels(this Bitmap bmp, List<Point> list, bool wrapping)
        {
            var dict = new Dictionary<Point, Color>();

            BitmapData data = bmp.LockBits(
                new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb
                );

            Color col = Color.FromArgb(0, 0, 0, 0); ;

            foreach (var l in list)
            {
                int y = l.Y;
                int x = l.X;
                if (wrapping)
                {
                    y = y % bmp.Height;
                    x = x % bmp.Width;
                }

                if (data.Stride * y + 4 * x < data.Stride * data.Height && data.Stride * y + 4 * x >= 0)
                    unsafe
                    {
                        byte* ptr = (byte*)data.Scan0;

                        dict.Add(l, Color.FromArgb(
                            ptr[data.Stride * y + 4 * x + 3],
                            ptr[data.Stride * y + 4 * x + 2],
                            ptr[data.Stride * y + 4 * x + 1],
                            ptr[data.Stride * y + 4 * x + 0]
                        ));
                    }
            }

            bmp.UnlockBits(data);

            return dict;
        }

        public static Dictionary<Point, Color> GetColorsAsDict(this Bitmap bmp)
        {
            var dict = new Dictionary<Point, Color>();

            int width = bmp.Width;
            int height = bmp.Height;
            BitmapData srcData = bmp.LockBits(
                new System.Drawing.Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb
                );

            int stride = srcData.Stride;
            int bytes = srcData.Stride * srcData.Height;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);

            bmp.UnlockBits(srcData);

            int i = 0;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    i = stride * y + 4 * x;
                    dict.Add(new Point(x, y), Color.FromArgb(buffer[i + 3], buffer[i + 2], buffer[i + 1], buffer[i]));
                }

            return dict;
        }

        public static byte[] GetBitmapDataBytes(this Bitmap bmp, out int stride)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            BitmapData srcData = bmp.LockBits(
                new System.Drawing.Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb
                );

            stride = srcData.Stride;
            int bytes = srcData.Stride * srcData.Height;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);

            bmp.UnlockBits(srcData);

            return buffer;
        }

        public static byte[] GetBitmapDataBytes(this Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            BitmapData srcData = bmp.LockBits(
                new System.Drawing.Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb
                );

            int bytes = srcData.Stride * srcData.Height;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);

            bmp.UnlockBits(srcData);

            return buffer;
        }

        public static void SetBitmapDataBytes(this Bitmap bmp, byte[] bytes)
        {
            BitmapData resData = bmp.LockBits(
                new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb
                );

            Marshal.Copy(bytes, 0, resData.Scan0, bytes.Length);

            bmp.UnlockBits(resData);
        }
    }
}
