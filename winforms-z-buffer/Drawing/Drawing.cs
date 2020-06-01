using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using winforms_z_buffer.Utils;

namespace winforms_z_buffer
{
    public class Drawing
    {
        ZBuffer zBuffer;
        Graphics graphics;
        Bitmap bmp;

        Size w;

        bool drawBorders;
        bool drawFill;

        public Drawing(Graphics g, Size windowSize, bool drawBorders, bool drawFill)
        {
            graphics = g;
            zBuffer = new ZBuffer(windowSize);
            this.drawBorders = drawBorders;
            this.drawFill = drawFill;
            w = windowSize;
        }
        public Drawing(Bitmap bmp, Size windowSize, bool drawBorders, bool drawFill)
        {
            this.bmp = bmp;
            zBuffer = new ZBuffer(windowSize);
            this.drawBorders = drawBorders;
            this.drawFill = drawFill;
            w = windowSize;
        }

        public Bitmap GetResult() { return bmp; }

        public void Draw(Cube c)
        {
            foreach (var side in c.Sides)
                foreach (var triangle in side)
                    DrawTriangle(triangle);
        }

        public void DrawTriangle(Triangle t)
        {
            if (!drawFill && !drawBorders)
                return;

            var points = t.TriangleVertexScreenPointsAsPoint3D();

            if (drawFill)
                drawTriangleFill(points, t.FaceColor);

            if (drawBorders)
                drawTriangleEdges(points, t.EdgeColor);
        }

        void drawTriangleEdges(Point3D[] vertices, Color color)
        {
            var points = new List<Point3D> { vertices[0], vertices[1], vertices[2] };


            foreach (var p in Line.OutlineTriangle(points))
                drawPoint(p, color);
        }

        void drawTriangleFill(Point3D[] vertices, Color color)
        {
            var points = new List<Point3D> { vertices[0], vertices[1], vertices[2] };

            foreach (var p in Fill.FillTriangle(points))
                drawPoint(p, color);
        }

        void drawPoint(Point3D point, Color color)
        {
            var p2D = (Point)point;

            if (zBuffer[point.X, point.Y] <= point.Z)
                return;

            zBuffer[point.X, point.Y] = point.Z;

            bmp.SetPixelFast(p2D.X + w.Width / 2, p2D.Y + w.Height / 2, color);
        }
    }
}
