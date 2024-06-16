using BetaSlicerWpf;
using QuantumConcepts.Formats.StereoLithography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace BetaSlicerCommon.WPF
{
    class MeshGeometryHelper
    {
        public static Vector3D GetCenter(Geometry3D geometry)
        {
            double x = geometry.Bounds.X + geometry.Bounds.Size.X / 2.0;
            double y = geometry.Bounds.Y + geometry.Bounds.Size.Y / 2.0;
            double z = geometry.Bounds.Z + geometry.Bounds.Size.Z / 2.0;

            Vector3D result = new Vector3D(x, y, z);

            return result;
        }

        public static MeshGeometry3D CreatePrinterBed(double xWidth, double yWidth)
        {
            // Use a small zHeight to lower the bed a little bit so that there's no zfighting between the bed and the model
            double zHeight = -0.01;
            Vector3DCollection normals = new Vector3DCollection();
            Point3DCollection vertices = new Point3DCollection();
            PointCollection textureCoordinates = new PointCollection();
            Int32Collection vertexIndices = new Int32Collection();

            // top left, top right, bottom right, bottom left
            vertices.Add(new Point3D(-1 * xWidth, 1 * yWidth, zHeight));
            vertices.Add(new Point3D(1 * xWidth, 1 * yWidth, zHeight));
            vertices.Add(new Point3D(1 * xWidth, -1 * yWidth, zHeight));
            vertices.Add(new Point3D(-1 * xWidth, -1 * yWidth, zHeight));

            // Counter clockwise
            // top left, bottom right, top right
            vertexIndices.Add(0);
            vertexIndices.Add(2);
            vertexIndices.Add(1);

            // top left, bottom left, bottom right
            vertexIndices.Add(0);
            vertexIndices.Add(3);
            vertexIndices.Add(2);

            textureCoordinates.Add(new Point(0, 0));
            textureCoordinates.Add(new Point(0, 1));
            textureCoordinates.Add(new Point(1, 1));
            textureCoordinates.Add(new Point(0, 1));

            MeshGeometry3D result = new MeshGeometry3D();
            result.Positions = vertices;
            result.TriangleIndices = vertexIndices;
            result.TextureCoordinates = textureCoordinates;
            return result;
        }

        static bool Equals(Vertex a, Vertex other)
        {
            if (!a.X.Equals(other.X))
                return false;

            if (!a.Y.Equals(other.Y))
                return false;
            if (!a.Z.Equals(other.Z))
                return false;

            return true;
        }

        static Dictionary<Point3D, int> cachedVertices;
        public static MeshGeometry3D CreateFromFacetsCached(IEnumerable<Facet> facets)
        {
            // Broken
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();
            cachedVertices = new Dictionary<Point3D, int>();

            Vector3DCollection normals = new Vector3DCollection();
            Point3DCollection vertices = new Point3DCollection();
            Int32Collection vertexIndices = new Int32Collection();

            Point3D a = new Point3D(3, 4, 5);
            Point3D b = new Point3D(5, 4, 3);

            int vertexIndex = 0;
            foreach (Facet facet in facets)
            {
                normals.Add(VertexConverter.ConvertToVector3D(facet.Normal));
                foreach (Vertex vertex in facet.Vertices)
                {
                    Point3D vertexPoint = VertexConverter.ConvertToPoint3D(vertex);
                    int newIndex = vertexIndex;
                    if (!cachedVertices.ContainsKey(vertexPoint))
                    {
                        cachedVertices.Add(vertexPoint, newIndex);
                        vertices.Add(vertexPoint);
                        vertexIndex++;
                    }
                    else
                    {
                        newIndex = cachedVertices[vertexPoint];
                    }
                    vertexIndices.Add(newIndex);
                }

            }
            myMeshGeometry3D.Positions = vertices;
            //myMeshGeometry3D.Normals = normals;
            myMeshGeometry3D.TriangleIndices = vertexIndices;

            cachedVertices.Clear();
            return myMeshGeometry3D;
        }

        public static MeshGeometry3D CreateFromFacetsNormals(IEnumerable<Facet> facets)
        {
            // also broken
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            Vector3DCollection normals = new Vector3DCollection();
            Point3DCollection vertices = new Point3DCollection();
            Int32Collection vertexIndices = new Int32Collection();

            int vertexIndex = 0;
            foreach (Facet facet in facets)
            {
                //normals.Add(-VertexConverter.ConvertToVector3D(facet.Normal));
                //for (int i = facet.Vertices.Count - 1; i >= 0; i--)
                //{
                //    Vertex vertex = facet.Vertices[i];

                //    vertices.Add(VertexConverter.ConvertToPoint3D(vertex));
                //    vertexIndices.Add(vertexIndex);
                //    vertexIndex++;
                //}

                vertices.Add(VertexConverter.ConvertToPoint3D(facet.Vertices[2]));
                vertices.Add(VertexConverter.ConvertToPoint3D(facet.Vertices[0]));
                vertices.Add(VertexConverter.ConvertToPoint3D(facet.Vertices[1]));

                Vector3D a = VertexConverter.ConvertToVector3D(facet.Vertices[0]);
                Vector3D b = VertexConverter.ConvertToVector3D(facet.Vertices[1]);
                Vector3D c = VertexConverter.ConvertToVector3D(facet.Vertices[2]);
                Vector3D normal = Vector3D.CrossProduct(c - a, c - b);

                //Vector3D normal = new Vector3D(1, 1, 1);
                normal.Normalize();

                normals.Add(normal);
                //normals.Add(new Vector3D(-10, 0, 0));

            }
            // Normal generation seems to be broken right now. WPF automatically generates normals when indices are set
            myMeshGeometry3D.Normals = normals;

            myMeshGeometry3D.Positions = vertices;
            //myMeshGeometry3D.TriangleIndices = vertexIndices;

            return myMeshGeometry3D;
        }

        public static MeshGeometry3D CreateFromFacets(IEnumerable<Facet> facets)
        {
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            Vector3DCollection normals = new Vector3DCollection();
            Point3DCollection vertices = new Point3DCollection();
            Int32Collection vertexIndices = new Int32Collection();

            int vertexIndex = 0;
            foreach (Facet facet in facets)
            {
                normals.Add(-VertexConverter.ConvertToVector3D(facet.Normal));
                foreach (Vertex vertex in facet.Vertices)
                {
                    vertices.Add(VertexConverter.ConvertToPoint3D(vertex));
                    vertexIndices.Add(vertexIndex);
                    vertexIndex++;
                }
            }
            // Normal generation seems to be broken right now. WPF automatically generates normals when indices are set
            //myMeshGeometry3D.Normals = normals;

            myMeshGeometry3D.Positions = vertices;
            myMeshGeometry3D.TriangleIndices = vertexIndices;

            // get some information for debugging purposes
            double testMinX = vertices.Min(v => v.X);
            double testMinY = vertices.Min(v => v.Y);
            double testMinZ = vertices.Min(v => v.Z);

            double testMaxX = vertices.Max(v => v.X);
            double testMaxY = vertices.Max(v => v.Y);
            double testMaxZ = vertices.Max(v => v.Z);
            Rect3D bounds = myMeshGeometry3D.Bounds;


            return myMeshGeometry3D;
        }
    }
}
