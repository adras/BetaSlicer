using BetaSlicerCommon;
using OpenTK.Graphics.OpenGL4;
using QuantumConcepts.Formats.StereoLithography;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetaSlicerOpenTK.GUI
{
    class TriangleRendering
    {
        // A simple vertex shader possible. Just passes through the position vector.
        const string VertexShaderSource = @"
            #version 330
            layout(location = 0) in vec4 position;
            void main(void)
            {
                gl_Position = position;
            }
        ";

        // A simple fragment shader. Just a constant red color.
        const string FragmentShaderSource = @"
            #version 330
            out vec4 outputColor;
            void main(void)
            {
                outputColor = vec4(1.0, 0.0, 0.0, 1.0);
            }
        ";

        // Points of a triangle in normalized device coordinates.
        float[] Points = new float[] {
            // X, Y, Z, W
            -0.5f, 0.0f, 0.0f, 1.0f,
            0.5f, 0.0f, 0.0f, 1.0f,
            0.0f, 0.5f, 0.0f, 1.0f };

        int VertexShader;
        int FragmentShader;
        int ShaderProgram;
        int VertexBufferObject;
        int VertexArrayObject;
        
        public void OnRenderFrame()
        {
            //GL.Clear(ClearBufferMask.ColorBufferBit);

            // Bind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            // Bind the VAO
            GL.BindVertexArray(VertexArrayObject);
            // Use/Bind the program
            GL.UseProgram(ShaderProgram);
            // This draws the triangle.
            GL.DrawArrays(PrimitiveType.Triangles, 0, Points.Length / 4);
        }

        public void OnLoad()
        {
            //Points = LoadPoints("bearing5.stl");


            // Load the source of the vertex shader and compile it.
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);
            GL.CompileShader(VertexShader);

            // Load the source of the fragment shader and compile it.
            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);
            GL.CompileShader(FragmentShader);

            // Create the shader program, attach the vertex and fragment shaders and link the program.
            ShaderProgram = GL.CreateProgram();
            GL.AttachShader(ShaderProgram, VertexShader);
            GL.AttachShader(ShaderProgram, FragmentShader);
            GL.LinkProgram(ShaderProgram);

            // Create the vertex buffer object (VBO) for the vertex data.
            VertexBufferObject = GL.GenBuffer();
            // Bind the VBO and copy the vertex data into it.
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, Points.Length * sizeof(float), Points, BufferUsageHint.StaticDraw);

            // Retrive the position location from the program.
            var positionLocation = GL.GetAttribLocation(ShaderProgram, "position");

            // Create the vertex array object (VAO) for the program.
            VertexArrayObject = GL.GenVertexArray();
            // Bind the VAO and setup the position attribute.
            GL.BindVertexArray(VertexArrayObject);
            GL.VertexAttribPointer(positionLocation, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(positionLocation);

            // Set the clear color to blue
            GL.ClearColor(0.0f, 0.0f, 1.0f, 0.0f);

        }

        private float[] LoadPoints(string fileName)
        {
            string stlPath = @"..\..\..\..\TestStl\";
            string filePath = System.IO.Path.Combine(stlPath, fileName);
            List<float> pointList = new List<float>();
            foreach(Facet x in StlFacetProvider.ReadFacets(filePath))
            {
                for (int i = 0; i < 3; i++)
                {
                    //float[] point = new float[] { x.Vertices[i].X, x.Vertices[i].Y, x.Vertices[i].Z, 1 };
                    //points
                    pointList.Add(x.Vertices[i].X);
                    pointList.Add(x.Vertices[i].Y);
                    pointList.Add(x.Vertices[i].Z);
                    pointList.Add(1);
                }
            }
            return pointList.ToArray();
        }

        public void OnResize(int width, int height)
        {
            // Resize the viewport to match the window size.
            GL.Viewport(0, 0, width, height);
        }

        public void OnUnload()
        {
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // Delete all the resources.
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);
            GL.DeleteProgram(ShaderProgram);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);

        }
    }
}
