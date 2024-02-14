using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace Baku.Params.BoidsBoundary
{
    internal class BoidsBoundaryItem(BoundingBox bbox, float weight, float range, float exponent)
    {
        public BoundingBox Bounds { get; set; } = bbox;
        public float Weight { get; set; } = weight;
        public float Range { get; set; } = range;
        public float Exponent { get; set; } = exponent;
    }
}
