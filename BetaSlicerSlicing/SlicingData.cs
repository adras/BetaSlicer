using QuantumConcepts.Formats.StereoLithography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BetaSlicerSlicing
{
    public class SlicingConfiguration
    {
        public double layerHeight;
        public double nozzleWidth;

        public SlicingConfiguration(double layerHeight, double nozzleWidth)
        {
            this.layerHeight = layerHeight;
            this.nozzleWidth = nozzleWidth;
        }

        public static SlicingConfiguration CreateDefaultTesting()
        {
            return new SlicingConfiguration(0.2, 0.4);
        }
    }

    public class SlicingLayer
    {
        public IEnumerable<Vector3> pathPoints;
    }

    

    public class SlicingData
    {
        public IEnumerable<Facet> facets;
        public Vector3 objectSize;
        
        // This is duplicate, layers.Count is the same and always accurate
        public int layerCount;
        public IList<SlicingLayer> layers;
        public SlicingConfiguration configuration;

        public SlicingData(IEnumerable<Facet> facets, SlicingConfiguration configuration)
        {
            this.facets = facets;
            this.configuration = configuration;
        }
    }
}
