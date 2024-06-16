using BetaSlicerCommon;
using QuantumConcepts.Formats.StereoLithography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;


namespace BetaSlicerSlicing
{
    public class GeometryContext
    {
        public IEnumerable<Facet> facets;
        public MeshGeometry3D meshGeometry;

        private GeometryContext()
        {
        }

        public static GeometryContext CreateFrom(IEnumerable<Facet> facets, MeshGeometry3D meshGeometry)
        {
            GeometryContext result = new GeometryContext();
            result.facets = facets;
            result.meshGeometry = meshGeometry;
            return result;
        }
    }
}
