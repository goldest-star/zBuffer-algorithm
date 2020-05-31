using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using winforms_z_buffer.Utils;

namespace winforms_z_buffer
{
    public class Drawing
    {
        ZBuffer zBuffer = new ZBuffer(new Size(800, 800));
        Graphics graphics;

        bool drawBorders;
        bool drawFill;

        public Drawing(Graphics g, bool drawBorders, bool drawFill)
        {
            graphics = g;
            this.drawBorders = drawBorders;
            this.drawFill = drawFill;
        }

        public void Draw(Cube c)
        {
            foreach (var side in c.Sides)
                foreach (var triangle in side)
                    DrawTriangle(triangle);
        }

        public void DrawTriangle(Triangle t)
        {
            if (drawBorders)
                foreach (var edge in t.TriangleEdgeScreenPointsAsPoint3D())
                    drawTriangleEdges(edge);

            if (drawFill)
                drawTriangleFill(t.TriangleVertexScreenPointsAsPoint3D());
        }

        void drawTriangleEdges(Point3D[] edge)
        {
            foreach (var p in Line.GetPoints(edge[0], edge[1]))
            {
                if (!(bool)zBuffer[p.X, p.Y, p.Z])
                    continue;

                zBuffer[p.X, p.Y, double.NaN] = p.Z;

                graphics.FillRectangle(Brushes.White, Ut.F(p.X), Ut.F(p.Y), 1, 1);
            }
        }

        void drawTriangleFill(Point3D[] vertices)
        {
            var points = new List<Point3D> { vertices[0], vertices[1], vertices[2] };

            foreach (var p in Fill.FillTriange(points))
            {

                if (!(bool)zBuffer[p.X, p.Y, 0])
                    continue;

                zBuffer[p.X, p.Y, double.NaN] = 0.0d;

                graphics.FillRectangle(Brushes.White, Ut.F(p.X), Ut.F(p.Y), 1, 1);
            }

            // graphics.FillPolygon(Brushes.Blue, points);


        }
    }
}
