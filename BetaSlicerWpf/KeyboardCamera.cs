using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace BetaSlicerWpf
{
    class KeyboardCamera
    {
        public PerspectiveCamera TheCamera;

        // The camera's current location.
        public double CameraPhi = Math.PI / 6.0;       // 30 degrees
        public double CameraTheta = Math.PI / 6.0;     // 30 degrees
        public double CameraR = 4.0;

        // The change in CameraPhi when you press the up and down arrows.
        public const double CameraDPhi = 0.1;

        // The change in CameraTheta when you press the left and right arrows.
        public const double CameraDTheta = 0.1;

        // The change in CameraR when you press + or -.
        public const double CameraDR = 1.1;

        public KeyboardCamera()
        {
            TheCamera = new PerspectiveCamera();
            CameraPhi = 0.37f;
            CameraTheta = 0.52f;
            CameraR = 100;

            PositionCamera();
        }

        public void Update(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    CameraPhi += CameraDPhi;
                    if (CameraPhi > Math.PI / 2.0)
                        CameraPhi = Math.PI / 2.0;
                    break;
                case Key.Down:
                    CameraPhi -= CameraDPhi;
                    if (CameraPhi < -Math.PI / 2.0)
                        CameraPhi = -Math.PI / 2.0;
                    break;
                case Key.Left:
                    CameraTheta += CameraDTheta;
                    break;
                case Key.Right:
                    CameraTheta -= CameraDTheta;
                    break;
                case Key.Add:
                case Key.OemPlus:
                    CameraR -= CameraDR;
                    if (CameraR < CameraDR)
                        CameraR = CameraDR;
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    CameraR += CameraDR;
                    break;
            }

            // Update the camera's position.
            PositionCamera();
        }

        public void PositionCamera()
        {

            // Calculate the camera's position in Cartesian coordinates.
            double y = CameraR * Math.Sin(CameraPhi);
            double hyp = CameraR * Math.Cos(CameraPhi);
            double x = hyp * Math.Cos(CameraTheta);
            double z = hyp * Math.Sin(CameraTheta);
            TheCamera.Position = new Point3D(x, y, z);

            // Look toward the origin.
            TheCamera.LookDirection = new Vector3D(-x, -y, -z);

            // Set the Up direction.
            TheCamera.UpDirection = new Vector3D(0, 1, 0);

            // Console.WriteLine("Camera.Position: (" + x + ", " + y + ", " + z + ")");
        }

    }
}
