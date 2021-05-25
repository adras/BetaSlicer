using BetaSlicerWpf;
using QuantumConcepts.Formats.StereoLithography;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace BetaSlicerCommon.WPF
{
    class MeshGeometryProvider
    {
        public static MeshGeometry3D CreateFromFacets(IEnumerable<Facet> facets)
        {
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            Vector3DCollection normals = new Vector3DCollection();
            Point3DCollection vertices = new Point3DCollection();
            Int32Collection vertexIndices = new Int32Collection();
            
            int vertexIndex = 0;
            foreach (Facet facet in facets)
            {
                normals.Add(VertexConverter.ConvertToVector3D(facet.Normal));
                foreach (Vertex vertex in facet.Vertices)
                {
                    vertices.Add(VertexConverter.ConvertToPoint3D(vertex));
                    vertexIndices.Add(vertexIndex);
                    vertexIndex++;
                }
            }
            //myMeshGeometry3D.Normals = normals;
            myMeshGeometry3D.Positions = vertices;
            myMeshGeometry3D.TriangleIndices = vertexIndices;
            


            // Create a collection of normal vectors for the MeshGeometry3D.
            //Vector3DCollection myNormalCollection = new Vector3DCollection();
            //myNormalCollection.Add(new Vector3D(0, 0, 1));
            //myNormalCollection.Add(new Vector3D(0, 0, 1));
            //myNormalCollection.Add(new Vector3D(0, 0, 1));
            //myNormalCollection.Add(new Vector3D(0, 0, 1));
            //myNormalCollection.Add(new Vector3D(0, 0, 1));
            //myNormalCollection.Add(new Vector3D(0, 0, 1));
            //myMeshGeometry3D.Normals = myNormalCollection;

            // Create a collection of vertex positions for the MeshGeometry3D.
            //Point3DCollection myPositionCollection = new Point3DCollection();
            //myPositionCollection.Add(new Point3D(-0.5, -0.5, 0.5));
            //myPositionCollection.Add(new Point3D(0.5, -0.5, 0.5));
            //myPositionCollection.Add(new Point3D(0.5, 0.5, 0.5));
            ////myPositionCollection.Add(new Point3D(0.5, 0.5, 0.5));
            //myPositionCollection.Add(new Point3D(-0.5, 0.5, 0.5));
            ////myPositionCollection.Add(new Point3D(-0.5, -0.5, 0.5));
            //myMeshGeometry3D.Positions = myPositionCollection;

            // Create a collection of texture coordinates for the MeshGeometry3D.
            //PointCollection myTextureCoordinatesCollection = new PointCollection();
            //myTextureCoordinatesCollection.Add(new Point(0, 0));
            //myTextureCoordinatesCollection.Add(new Point(1, 0));
            //myTextureCoordinatesCollection.Add(new Point(1, 1));
            ////myTextureCoordinatesCollection.Add(new Point(1, 1));
            //myTextureCoordinatesCollection.Add(new Point(0, 1));
            ////myTextureCoordinatesCollection.Add(new Point(0, 0));
            //myMeshGeometry3D.TextureCoordinates = myTextureCoordinatesCollection;

            // Create a collection of triangle indices for the MeshGeometry3D.
            //Int32Collection myTriangleIndicesCollection = new Int32Collection();
            //myTriangleIndicesCollection.Add(0);
            //myTriangleIndicesCollection.Add(1);
            //myTriangleIndicesCollection.Add(2);
            //myTriangleIndicesCollection.Add(0);
            //myTriangleIndicesCollection.Add(2);
            //myTriangleIndicesCollection.Add(3);
            //myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            return myMeshGeometry3D;
        }
    }
}
