using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace winforms_z_buffer
{
    public class Camera
    {
        public static Camera Instance;

        double fov = Math.PI / 2;
        double near = 0.001;
        double far = 10000;
        double aspect = 1;

        public Camera(double fov, double near, double far) : this()
        {
            this.fov = fov;
            this.near = near;
            this.far = far;
        }

        public Camera() { Instance = this; }

        public Matrix3D GetProjectionMatrix()
        {
            double fn = far - near;
            double fovRad = 1 / Math.Tan(fov / 2);

            Matrix3D matrix = new Matrix3D(
                    aspect * fovRad, 0, 0, 0,
                    0, fovRad, 0, 0,
                    0, 0, far / fn, -far * near / fn,
                    0, 0, 1, 0
                );

            return matrix;
        }

        public void ChangeFOV(double displacement)
        {
            fov += displacement;
        }
    }
}
