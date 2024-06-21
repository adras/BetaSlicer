using Clipper2Lib;
using QuantumConcepts.Formats.StereoLithography;
using System.Numerics;

namespace BetaSlicerSlicing
{
    public class MySlicer
    {
        SlicingConfiguration configuration;

        public MySlicer()
        {
            configuration = SlicingConfiguration.CreateDefaultTesting();
        }

        public async Task<SlicingData> Slice(IEnumerable<Facet> facets)
        {
            SlicingData result = new SlicingData(facets, configuration);
            await UpdateObjectSize(result);
            UpdateRemainingObjectProperties(result);

            UpdateSliceGeometry(result);

            return result;
        }

        private void UpdateSliceGeometry(SlicingData result)
        {
            //Clipper2Lib.Clipper.
            // Actual slizing is supposed to happen here now
        }

        private void UpdateRemainingObjectProperties(SlicingData slicingData)
        {
            slicingData.layerCount= (int) (slicingData.objectSize.Z / configuration.layerHeight);
        }

        private async Task UpdateObjectSize(SlicingData slicingData)
        {
            await Task.Run(() =>
            {
                var allPoints = slicingData.facets.SelectMany(f => f.Vertices);
                float minX = allPoints.Min(p => p.X);
                float minY = allPoints.Min(p => p.Y);
                float minZ = allPoints.Min(p => p.Z);

                float maxX = allPoints.Max(p => p.X);
                float maxY = allPoints.Max(p => p.Y);
                float maxZ = allPoints.Max(p => p.Z);

                // for now we keep the general size by calculating the min/max delta
                slicingData.objectSize = new Vector3(maxX - minX, maxY - minY, maxZ - minZ);

            });

        }
    }
}
