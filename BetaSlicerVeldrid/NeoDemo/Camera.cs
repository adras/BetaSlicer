﻿using BetaSlicer.NeoDemo;
using ImGuiNET;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using Veldrid.Sdl2;

namespace Veldrid.NeoDemo
{
    public class Camera : IUpdateable
    {
        private float _fov = 1f;

        // Using a very low near plane to allow close zooming because the models can be quite small
        private float _near = 0.001f;
        private float _far = 1000f;

        private Matrix4x4 _viewMatrix;
        private Matrix4x4 _projectionMatrix;

        private Vector3 _position = new Vector3();
        private Vector3 _lookDirection = new Vector3();
        private float _moveSpeed = 10.0f;

        private float _yaw;
        private float _pitch;

        private Vector2 _mousePressedPos;
        private bool _mousePressed = false;
        private GraphicsDevice _gd;
        private bool _useReverseDepth;
        private float _windowWidth;
        private float _windowHeight;
        private Sdl2Window _window;

        public Camera(GraphicsDevice gd, Sdl2Window window)
        {
            _gd = gd;
            _useReverseDepth = gd.IsDepthRangeZeroToOne;
            _window = window;
            _windowWidth = window.Width;
            _windowHeight = window.Height;
            UpdatePerspectiveMatrix();
            UpdateViewMatrix();
        }

        public void UpdateBackend(GraphicsDevice gd, Sdl2Window window)
        {
            _gd = gd;
            _useReverseDepth = gd.IsDepthRangeZeroToOne;
            _window = window;
            _windowWidth = window.Width;
            _windowHeight = window.Height;
            UpdatePerspectiveMatrix();
        }

        public Matrix4x4 ViewMatrix => _viewMatrix;
        public Matrix4x4 ProjectionMatrix => _projectionMatrix;

        public Vector3 Position { get => _position; set { _position = value; UpdateViewMatrix(); } }
        public Vector3 LookDirection => _lookDirection;

        public float FarDistance => _far;

        public float FieldOfView => _fov;
        public float NearDistance => _near;

        public float AspectRatio => _windowWidth / _windowHeight;

        public float Yaw { get => _yaw; set { _yaw = value; UpdateViewMatrix(); } }
        public float Pitch { get => _pitch; set { _pitch = value; UpdateViewMatrix(); } }

        public void Update(float deltaSeconds)
        {
            float sprintFactor = InputTracker.GetKey(Key.ControlLeft)
                ? 0.01f
                : InputTracker.GetKey(Key.ShiftLeft)
                    ? 2.5f
                    : 1f;
            Vector3 motionDir = Vector3.Zero;
            if (InputTracker.GetKey(Key.A))
            {
                motionDir += -Vector3.UnitX;
            }
            if (InputTracker.GetKey(Key.D))
            {
                motionDir += Vector3.UnitX;
            }
            if (InputTracker.GetKey(Key.W))
            {
                motionDir += -Vector3.UnitZ;
            }
            if (InputTracker.GetKey(Key.S))
            {
                motionDir += Vector3.UnitZ;
            }
            if (InputTracker.GetKey(Key.Q))
            {
                motionDir += -Vector3.UnitY;
            }
            if (InputTracker.GetKey(Key.E))
            {
                motionDir += Vector3.UnitY;
            }

            if (motionDir != Vector3.Zero)
            {
                Quaternion lookRotation = Quaternion.CreateFromYawPitchRoll(Yaw, Pitch, 0f);
                motionDir = Vector3.Transform(motionDir, lookRotation);
                _position += motionDir * _moveSpeed * sprintFactor * deltaSeconds;
                Debug.WriteLine(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Position: {0}", _position));
                Debug.WriteLine(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Yaw: {0} - Pitch: {1}", Yaw, Pitch));

                UpdateViewMatrix();
            }

            if (!ImGui.GetIO().WantCaptureMouse
                && (InputTracker.GetMouseButton(MouseButton.Left) || InputTracker.GetMouseButton(MouseButton.Right)))
            {
                if (!_mousePressed)
                {
                    _mousePressed = true;
                    _mousePressedPos = InputTracker.MousePosition;
                    Sdl2Native.SDL_ShowCursor(0);
                    Sdl2Native.SDL_SetWindowGrab(_window.SdlWindowHandle, true);
                }
                Vector2 mouseDelta = _mousePressedPos - InputTracker.MousePosition;
                Sdl2Native.SDL_WarpMouseInWindow(_window.SdlWindowHandle, (int)_mousePressedPos.X, (int)_mousePressedPos.Y);
                Yaw += mouseDelta.X * 0.002f;
                Pitch += mouseDelta.Y * 0.002f;
            }
            else if (_mousePressed)
            {
                Sdl2Native.SDL_WarpMouseInWindow(_window.SdlWindowHandle, (int)_mousePressedPos.X, (int)_mousePressedPos.Y);
                Sdl2Native.SDL_SetWindowGrab(_window.SdlWindowHandle, false);
                Sdl2Native.SDL_ShowCursor(1);
                _mousePressed = false;
            }

            Pitch = Math.Clamp(Pitch, -1.55f, 1.55f);
            UpdateViewMatrix();
        }

        public void WindowResized(float width, float height)
        {
            _windowWidth = width;
            _windowHeight = height;
            UpdatePerspectiveMatrix();
        }

        private void UpdatePerspectiveMatrix()
        {
            float aspectRatio = _windowWidth / _windowHeight;
            _projectionMatrix = Util.CreatePerspective(_gd, _useReverseDepth, _fov, aspectRatio, _near, _far);
        }

        private void UpdateViewMatrix()
        {
            Quaternion lookRotation = Quaternion.CreateFromYawPitchRoll(Yaw, Pitch, 0f);
            Vector3 lookDir = Vector3.Transform(-Vector3.UnitZ, lookRotation);
            _lookDirection = lookDir;
            _viewMatrix = Matrix4x4.CreateLookAt(_position, _position + _lookDirection, Vector3.UnitY);
        }

        public CameraInfo GetCameraInfo() => new CameraInfo
        {
            CameraPosition_WorldSpace = _position,
            CameraLookDirection = _lookDirection
        };
    }
}
