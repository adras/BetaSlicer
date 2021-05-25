using QuantumConcepts.Formats.StereoLithography;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace BetaSlicerWpf
{
    class VertexConverter
    {
        public static Vector3D ConvertToVector3D(Vertex vertex)
        {
            Vector3D result = new Vector3D(vertex.X, vertex.Y, vertex.Z);
            return result;
        }

        public static Point3D ConvertToPoint3D(Vertex vertex)
        {
            Point3D result = new Point3D(vertex.X, vertex.Y, vertex.Z);
            return result;
        }
    }
}
