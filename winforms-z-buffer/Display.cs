using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace winforms_z_buffer
{
    public partial class Display : Form
    {
        List<Cube> cubes = new List<Cube>();
        Cube CSO;

        PictureBox pb;

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

            pb = new PictureBox
            {
                Name = "pb",
                Size = Size,
                Location = new Point(0, 0),
                Dock = DockStyle.Fill
            };
            Controls.Add(pb);

            // Camera setup
            new Camera(/*fov, near, far : set to default*/);

            initializeCubes();

            OnPaint(null);
        }

        void initializeCubes()
        {
            // Coordinate System Origin
            CSO = Cube.UnitCube(Color.White, Color.Transparent);
            CSO.Rescale(0, 0, 0);
            cubes.Add(CSO);

            // Cubes
            Cube c1 = Cube.UnitCube(Color.DarkRed, Color.Red);
            c1.Rescale(10, 5, 2);
            c1.RotateAroundLocalAxis(0, - Math.PI / 3, - Math.PI / 9);
            c1.Translate(new Vector3D(10, 10, -3));
            cubes.Add(c1);

            Cube c2 = Cube.UnitCube(Color.DarkBlue, Color.Blue);
            c2.Rescale(5, 5, 5);
            c2.RotateAroundLocalAxis(0, Math.PI / 4, Math.PI / 12);
            c2.Translate(new Vector3D(-10, 5, -2));
            cubes.Add(c2);

            Cube c3 = Cube.UnitCube(Color.DarkGreen, Color.Green);
            c3.Rescale(15, 1, 1);
            c3.Translate(new Vector3D(0, -15, 0));
            c3.RotateAroundOrigin(Math.PI / 3, Math.PI / 8, 0);
            cubes.Add(c3);

            Cube c4 = Cube.UnitCube(Color.Purple, Color.Magenta);
            c4.Rescale(15, 15, 15);
            c4.Translate(new Vector3D(-15, -15, -15));
            c4.RotateAroundOrigin(Math.PI / 7, 0, Math.PI / 64);
            cubes.Add(c4);
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            Bitmap bmp = new Bitmap(Width, Height);
            var gr = Graphics.FromImage(bmp);
            gr.Clear(Color.Black);

            var drawing = new Drawing(bmp, Size, /*draw edges*/false, /*draw faces*/true);

            foreach (var c in cubes)
                drawing.Draw(c);

            pb.Image = drawing.GetResult();
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
                    OnPaint(null);
                    break;
                case (Keys.R | Keys.Control):
                    initializeCubes();
                    new Camera();
                    goto case Keys.R;

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

                // Camera
                case Keys.D1:
                    Camera.Instance.ChangeFOV(Math.PI / 12);
                    goto case Keys.R;
                case Keys.D2:
                    Camera.Instance.ChangeFOV(-Math.PI / 12);
                    goto case Keys.R;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }



    }
}
