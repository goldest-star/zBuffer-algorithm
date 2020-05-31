using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace winforms_z_buffer
{
    public partial class Display : Form
    {
        Cube[] cubes = null;
        Cube CSO;

        public Display()
        {
            // Setup Winform
            Width = Height = 800;
            StartPosition = FormStartPosition.CenterScreen;
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true);

            CSO = Cube.UnitCube(Color.PaleGoldenrod);
            CSO.Rescale(0.1, 0.1, 0.1);

            // Create Cubes
            Cube c1 = Cube.UnitCube(Color.Red);
            c1.Rescale(10, 5, 2);
            // c1.Translate(new Vector3D(10, 10, -3));

            //Cube c2 = Cube.UnitCube(Color.Blue);
            //c2.Rescale(5, 5, 5);
            //c2.Translate(new Vector3D(-10, 5, -2));



            cubes = new Cube[] { CSO, c1, };
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            var g = args.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.Black);

            // Move coordinate origin to center
            g.TranslateTransform(Width / 2, Height / 2);

            // Mark coordinate origin
            g.FillEllipse(Brushes.Purple, -4, -4, 8, 8);

            var drawing = new Drawing(g, true, true);

            if (cubes != null)
                foreach (var c in cubes)
                    drawing.Draw(c);
        }

        void TranslateCubes(Vector3D displacement)
        {
            foreach (var c in cubes)
                c.Translate(displacement);
        }

        void ScaleCubes(double factor)
        {
            foreach (var c in cubes)
                c.Rescale(factor, factor, factor);
        }
        void RotateCubes(double angleX, double angleY, double angleZ)
        {
            foreach (var c in cubes)
                c.RotateAroundOrigin(angleX, angleY, angleZ);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            double standardSpeed = 1;
            double fastSpeed = 10;
            double rotationStep = Math.PI / 64;

            switch (keyData)
            {
                case Keys.R:
                    Refresh();
                    break;

                // Translation

                case Keys.Right:
                    TranslateCubes(new Vector3D(-standardSpeed, 0, 0));
                    goto case Keys.R;
                case (Keys.Right | Keys.Control):
                    TranslateCubes(new Vector3D(-fastSpeed, 0, 0));
                    goto case Keys.R;
                case Keys.Left:
                    TranslateCubes(new Vector3D(standardSpeed, 0, 0));
                    goto case Keys.R;
                case (Keys.Left | Keys.Control):
                    TranslateCubes(new Vector3D(fastSpeed, 0, 0));
                    goto case Keys.R;

                case Keys.Up:
                    TranslateCubes(new Vector3D(0, standardSpeed, 0));
                    goto case Keys.R;
                case (Keys.Up | Keys.Control):
                    TranslateCubes(new Vector3D(0, fastSpeed, 0));
                    goto case Keys.R;
                case Keys.Down:
                    TranslateCubes(new Vector3D(0, -standardSpeed, 0));
                    goto case Keys.R;
                case (Keys.Down | Keys.Control):
                    TranslateCubes(new Vector3D(0, -fastSpeed, 0));
                    goto case Keys.R;

                case Keys.Z:
                    TranslateCubes(new Vector3D(0, 0, standardSpeed));
                    goto case Keys.R;
                case (Keys.Z | Keys.Control):
                    TranslateCubes(new Vector3D(0, 0, fastSpeed));
                    goto case Keys.R;
                case Keys.X:
                    TranslateCubes(new Vector3D(0, 0, -standardSpeed));
                    goto case Keys.R;
                case (Keys.X | Keys.Control):
                    TranslateCubes(new Vector3D(0, 0, -fastSpeed));
                    goto case Keys.R;

                // Rotation

                case Keys.W:
                    RotateCubes(0, standardSpeed * rotationStep, 0);
                    goto case Keys.R;
                case (Keys.W | Keys.Control):
                    RotateCubes(0, fastSpeed * rotationStep, 0);
                    goto case Keys.R;
                case Keys.S:
                    RotateCubes(0, -standardSpeed * rotationStep, 0);
                    goto case Keys.R;
                case (Keys.S | Keys.Control):
                    RotateCubes(0, -fastSpeed * rotationStep, 0);
                    goto case Keys.R;

                case Keys.A:
                    RotateCubes(0, 0, standardSpeed * rotationStep);
                    goto case Keys.R;
                case (Keys.A | Keys.Control):
                    RotateCubes(0, 0, fastSpeed * rotationStep);
                    goto case Keys.R;
                case Keys.D:
                    RotateCubes(0, 0, -standardSpeed * rotationStep);
                    goto case Keys.R;
                case (Keys.D | Keys.Control):
                    RotateCubes(0, 0, -fastSpeed * rotationStep);
                    goto case Keys.R;

                case Keys.E:
                    RotateCubes(standardSpeed * rotationStep, 0, 0);
                    goto case Keys.R;
                case (Keys.E | Keys.Control):
                    RotateCubes(fastSpeed * rotationStep, 0, 0);
                    goto case Keys.R;
                case Keys.Q:
                    RotateCubes(-standardSpeed * rotationStep, 0, 0);
                    goto case Keys.R;
                case (Keys.Q | Keys.Control):
                    RotateCubes(-fastSpeed * rotationStep, 0, 0);
                    goto case Keys.R;

                // Scale
                case Keys.OemPeriod:
                    ScaleCubes(1.5);
                    goto case Keys.R;
                case (Keys.OemPeriod | Keys.Control):
                    ScaleCubes(2);
                    goto case Keys.R;
                case Keys.Oemcomma:
                    ScaleCubes(0.75);
                    goto case Keys.R;
                case (Keys.Oemcomma | Keys.Control):
                    ScaleCubes(0.5);
                    goto case Keys.R;
            }

            // Refresh();

            return base.ProcessCmdKey(ref msg, keyData);
        }



    }
}
