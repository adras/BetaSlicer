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
            perspectiveCamera.FieldOfView = 60;

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
            if (wheelDelta != 0)
            {
                ZoomCamera();
            }
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
