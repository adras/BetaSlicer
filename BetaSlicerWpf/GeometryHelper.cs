using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace BetaSlicerWpf
{
    class GeometryHelper
    {
        static double height = 0.2;
        static double width = 0.4;

        public static IEnumerable<MeshGeometry3D> CreateRectangle(List<Point3D> points)
        {


            for (int i = 1; i < points.Count; i++)
            {
                MeshGeometry3D rect = CreateRectangle(points[i - 1], points[i]);

                yield return rect;
            }
        }

        private static MeshGeometry3D CreateRectangle(Point3D startPoint, Point3D endPoint)
        {

            Vector3D start = (Vector3D)startPoint;
            Vector3D end = (Vector3D)endPoint;
            Vector3D dir = end - start;
            
            Vector3D up = new Vector3D(0, 0, 1);
            Vector3D left = Vector3D.CrossProduct(dir, up);
            left.Normalize();

            left *= width;
            up *= height;

            // Upper side corners
            Vector3D startLeft = start + left + up;
            Vector3D endLeft = end + left + up;

            Vector3D startRight = start - left + up;
            Vector3D endRight = end - left + up;

            MeshGeometry3D geom = new MeshGeometry3D();
            geom.Positions = new Point3DCollection();
            geom.Positions.Add((Point3D)startLeft);
            geom.Positions.Add((Point3D)startRight);
            geom.Positions.Add((Point3D)endRight);
            geom.Positions.Add((Point3D)endLeft);

            geom.TriangleIndices = new Int32Collection();
            geom.TriangleIndices.Add(0);
            geom.TriangleIndices.Add(1);
            geom.TriangleIndices.Add(2);
            geom.TriangleIndices.Add(2);
            geom.TriangleIndices.Add(3);
            geom.TriangleIndices.Add(0);

            return geom;
        }
    }
}
