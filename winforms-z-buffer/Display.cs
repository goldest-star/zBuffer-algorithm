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
        List<Cube> selectedCubes = new List<Cube>();

        PictureBox pb;

        public Display()
        {
            initializeForm();

            initializePicturebox();

            initializeCamera();

            initializeCubes();

            OnPaint(null);
        }

        void initializeForm()
        {
            Width = Height = 800;
            StartPosition = FormStartPosition.CenterScreen;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true);
        }

        void initializePicturebox()
        {
            pb = new PictureBox
            {
                Name = "pb",
                Size = Size,
                Location = new Point(0, 0),
                Dock = DockStyle.Fill
            };
            Controls.Add(pb);
        }

        void initializeCamera()
        {
            new Camera(/* fov, near, far, aspect ratio */);
        }

        void initializeCubes()
        {
            cubes.Clear();

            //Cubes
            Cube c1 = Cube.UnitCube(42);
            c1.Rescale(5, 2, 2);
            c1.RotateAroundLocalAxis(0, -Math.PI / 3, -Math.PI / 9);
            c1.Translate(new Vector3D(10, 10, -3));
            cubes.Add(c1);

            Cube c2 = Cube.UnitCube(2137);
            c2.Rescale(5, 5, 5);
            c2.RotateAroundLocalAxis(0, Math.PI / 4, Math.PI / 12);
            c2.Translate(new Vector3D(-10, 5, -2));
            cubes.Add(c2);

            Cube c3 = Cube.UnitCube(1337);
            c3.Rescale(2, 5, 15);
            c3.Translate(new Vector3D(0, -15, 0));
            c3.RotateAroundOrigin(-Math.PI / 3, Math.PI / 8, -Math.PI / 2);
            cubes.Add(c3);

            Cube c4 = Cube.UnitCube(420);
            c4.Rescale(3, 3, 3);
            c4.Translate(new Vector3D(-15, -15, -15));
            c4.RotateAroundOrigin(Math.PI / 7, 0, Math.PI / 64);
            cubes.Add(c4);

            Cube c5 = Cube.UnitCube(0);
            c5.Rescale(0.5, 0.5, 20);
            c5.Translate(new Vector3D(0, 15, -15));
            c5.RotateAroundLocalAxis(Math.PI / 7, 0, Math.PI / 64);
            cubes.Add(c5);

            selectedCubes.AddRange(cubes);
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            Bitmap bmp = new Bitmap(Width, Height);
            var gr = Graphics.FromImage(bmp);
            gr.Clear(Color.Black);

            var drawing = new Drawing(bmp, Size, /*draw faces*/true);

            foreach (var c in cubes)
                drawing.Draw(c);

            pb.Image = drawing.GetResult();
        }

        #region shapes control

        void TranslateCubes(Vector3D displacement)
        {
            foreach (var c in selectedCubes)
                c.Translate(displacement);
        }
        void ScaleCubes(double factor)
        {
            foreach (var c in selectedCubes)
                c.Rescale(factor, factor, factor);
        }
        void RotateCubesAroundOrigin(double angleX, double angleY, double angleZ)
        {
            foreach (var c in selectedCubes)
                c.RotateAroundOrigin(angleX, angleY, angleZ);
        }
        void RotateCubesAroundLocal(double angleX, double angleY, double angleZ)
        {
            foreach (var c in selectedCubes)
                c.RotateAroundLocalAxis(angleX, angleY, angleZ);
        }

        void ChangeSelectedCubes(int index)
        {
            selectedCubes.Clear();

            if (index == -1)
                selectedCubes.AddRange(cubes);
            else if (index > -1 && index < cubes.Count)
                selectedCubes.Add(cubes[index]);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            double standardSpeed = 2;
            double fastSpeed = 10;
            double rotationStep = Math.PI / 64;

            switch (keyData)
            {
                case Keys.R:
                    OnPaint(null);
                    break;
                case (Keys.R | Keys.Control):
                    initializeCubes();
                    initializeCamera();
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
                case Keys.A:
                    RotateCubesAroundOrigin(0, standardSpeed * rotationStep, 0);
                    goto case Keys.R;
                case (Keys.A | Keys.Control):
                    RotateCubesAroundOrigin(0, fastSpeed * rotationStep, 0);
                    goto case Keys.R;
                case Keys.D:
                    RotateCubesAroundOrigin(0, -standardSpeed * rotationStep, 0);
                    goto case Keys.R;
                case (Keys.D | Keys.Control):
                    RotateCubesAroundOrigin(0, -fastSpeed * rotationStep, 0);
                    goto case Keys.R;

                case Keys.Q:
                    RotateCubesAroundOrigin(0, 0, standardSpeed * rotationStep);
                    goto case Keys.R;
                case (Keys.Q | Keys.Control):
                    RotateCubesAroundOrigin(0, 0, fastSpeed * rotationStep);
                    goto case Keys.R;
                case Keys.E:
                    RotateCubesAroundOrigin(0, 0, -standardSpeed * rotationStep);
                    goto case Keys.R;
                case (Keys.E | Keys.Control):
                    RotateCubesAroundOrigin(0, 0, -fastSpeed * rotationStep);
                    goto case Keys.R;

                case Keys.S:
                    RotateCubesAroundOrigin(standardSpeed * rotationStep, 0, 0);
                    goto case Keys.R;
                case (Keys.S | Keys.Control):
                    RotateCubesAroundOrigin(fastSpeed * rotationStep, 0, 0);
                    goto case Keys.R;
                case Keys.W:
                    RotateCubesAroundOrigin(-standardSpeed * rotationStep, 0, 0);
                    goto case Keys.R;
                case (Keys.W | Keys.Control):
                    RotateCubesAroundOrigin(-fastSpeed * rotationStep, 0, 0);
                    goto case Keys.R;

                case (Keys.A | Keys.Shift):
                    RotateCubesAroundLocal(0, standardSpeed * rotationStep, 0);
                    goto case Keys.R;
                case (Keys.A | Keys.Control | Keys.Shift):
                    RotateCubesAroundLocal(0, fastSpeed * rotationStep, 0);
                    goto case Keys.R;
                case (Keys.D | Keys.Shift):
                    RotateCubesAroundLocal(0, -standardSpeed * rotationStep, 0);
                    goto case Keys.R;
                case (Keys.D | Keys.Control | Keys.Shift):
                    RotateCubesAroundLocal(0, -fastSpeed * rotationStep, 0);
                    goto case Keys.R;

                case (Keys.Q | Keys.Shift):
                    RotateCubesAroundLocal(0, 0, standardSpeed * rotationStep);
                    goto case Keys.R;
                case (Keys.Q | Keys.Control | Keys.Shift):
                    RotateCubesAroundLocal(0, 0, fastSpeed * rotationStep);
                    goto case Keys.R;
                case (Keys.E | Keys.Shift):
                    RotateCubesAroundLocal(0, 0, -standardSpeed * rotationStep);
                    goto case Keys.R;
                case (Keys.E | Keys.Control | Keys.Shift):
                    RotateCubesAroundLocal(0, 0, -fastSpeed * rotationStep);
                    goto case Keys.R;

                case (Keys.S | Keys.Shift):
                    RotateCubesAroundLocal(standardSpeed * rotationStep, 0, 0);
                    goto case Keys.R;
                case (Keys.S | Keys.Control | Keys.Shift):
                    RotateCubesAroundLocal(fastSpeed * rotationStep, 0, 0);
                    goto case Keys.R;
                case (Keys.W | Keys.Shift):
                    RotateCubesAroundLocal(-standardSpeed * rotationStep, 0, 0);
                    goto case Keys.R;
                case (Keys.W | Keys.Control | Keys.Shift):
                    RotateCubesAroundLocal(-fastSpeed * rotationStep, 0, 0);
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
                case Keys.O:
                    Camera.Instance.ChangeFOV(Math.PI / 12);
                    goto case Keys.R;
                case Keys.P:
                    Camera.Instance.ChangeFOV(-Math.PI / 12);
                    goto case Keys.R;

                // Cube selection
                case Keys.D1:
                    ChangeSelectedCubes(0);
                    goto case Keys.R;
                case Keys.D2:
                    ChangeSelectedCubes(1);
                    goto case Keys.R;
                case Keys.D3:
                    ChangeSelectedCubes(2);
                    goto case Keys.R;
                case Keys.D4:
                    ChangeSelectedCubes(3);
                    goto case Keys.R;
                case Keys.D5:
                    ChangeSelectedCubes(4);
                    goto case Keys.R;
                case Keys.D6:
                    ChangeSelectedCubes(5);
                    goto case Keys.R;
                case Keys.D7:
                    ChangeSelectedCubes(6);
                    goto case Keys.R;
                case Keys.D8:
                    ChangeSelectedCubes(7);
                    goto case Keys.R;
                case Keys.D9:
                    ChangeSelectedCubes(8);
                    goto case Keys.R;
                case Keys.D0:
                    ChangeSelectedCubes(-1);
                    goto case Keys.R;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}
