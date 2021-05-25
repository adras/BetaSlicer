using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace BetaSlicerWpf
{
    class MouseOrbitCamera
    {
        private IInputElement uiElement;

        public PerspectiveCamera perspectiveCamera;

        private Point lastMousePos;
        private Point currentMousePos;

        private Point positionDelta;
        private int wheelDelta;

        const double ZOOM_FACTOR = 0.1;

        public MouseOrbitCamera(IInputElement uiElement)
        {
            this.uiElement = uiElement;

            perspectiveCamera = new PerspectiveCamera();
            perspectiveCamera.FieldOfView = 30;

            lastMousePos = Mouse.GetPosition(uiElement);
            currentMousePos = Mouse.GetPosition(uiElement);
            uiElement.MouseWheel += UiElement_MouseWheel;
        }

        private void UiElement_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            wheelDelta = e.Delta;
        }

        public void Update(DeltaTime delta)
        {
            lastMousePos = currentMousePos;

            currentMousePos = Mouse.GetPosition(uiElement);

            positionDelta = (Point)(currentMousePos - lastMousePos);

            UpdateCamera();

            // perspectiveCamera.LookDirection
            // perspectiveCamera.UpDirection;
            // perspectiveCamera.Position;
            wheelDelta = 0;
        }

        private void UpdateCamera()
        {
            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                PanCamera();
            }
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                OrbitCamera();
            }
            if (wheelDelta != 0)
            {
                ZoomCamera();
            }
        }

        private void OrbitCamera()
        {
            //RotateTransform3D rotateTransform = (RotateTransform3D)perspectiveCamera.Transform;

            //AxisAngleRotation3D rotation = new AxisAngleRotation3D(perspectiveCamera.UpDirection, -0.1);
            //RotateTransform3D rotateTransform =new RotateTransform3D(rotation);
            //perspectiveCamera.Transform = rotation;

            // https://stackoverflow.com/questions/43375372/moving-perspectivecamera-in-the-direction-it-is-facing-in-c-sharp
            RotateVertical(positionDelta.Y);
            Rotate(positionDelta.X);
            Debug.WriteLine("Up: " + perspectiveCamera.UpDirection);
        }

        public void Rotate(double d)
        {
            double u = 0.05;
            double angleD = u * d;
            Vector3D lookDirection = perspectiveCamera.LookDirection;

            var m = new Matrix3D();
            m.Translate((Vector3D)perspectiveCamera.Position);
            m.Rotate(new Quaternion(perspectiveCamera.UpDirection, -angleD)); // Rotate about the camera's up direction to look left/right
            perspectiveCamera.LookDirection = m.Transform(perspectiveCamera.LookDirection);
        }

        public void RotateVertical(double d)
        {
            double u = 0.05;
            double angleD = u * d;
            Vector3D lookDirection = perspectiveCamera.LookDirection;

            // Cross Product gets a vector that is perpendicular to the passed in vectors (order does matter, reverse the order and the vector will point in the reverse direction)
            var cp = Vector3D.CrossProduct(perspectiveCamera.UpDirection, lookDirection);
            cp.Normalize();

            var m = new Matrix3D();
            m.Rotate(new Quaternion(cp, -angleD)); // Rotate about the vector from the cross product
            perspectiveCamera.LookDirection = m.Transform(perspectiveCamera.LookDirection);

            perspectiveCamera.UpDirection = Vector3D.CrossProduct(perspectiveCamera.LookDirection, cp);
        }

        private void ZoomCamera()
        {
            perspectiveCamera.Position += perspectiveCamera.LookDirection * (wheelDelta * ZOOM_FACTOR);
        }

        private void PanCamera()
        {
            Vector3D leftAxis = Vector3D.CrossProduct(perspectiveCamera.LookDirection, perspectiveCamera.UpDirection);
            Vector3D upAxis = perspectiveCamera.UpDirection;

            Vector3D translation = -leftAxis * positionDelta.X + upAxis * positionDelta.Y;
            Debug.WriteLine("Translation: " + translation);
            perspectiveCamera.Position += translation;
        }
    }
}
