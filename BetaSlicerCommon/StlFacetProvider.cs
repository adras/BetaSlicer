using QuantumConcepts.Formats.StereoLithography;
using System;
using System.Collections.Generic;

namespace BetaSlicerCommon
{
    public class StlFacetProvider
    {
        public static IEnumerable<Facet> ReadFacets(string fileName)
        {
            STLDocument doc = STLDocument.Open(fileName);
            
            foreach(Facet facet in doc.Facets)
            {
                yield return facet;
            }
        }
    }
}
