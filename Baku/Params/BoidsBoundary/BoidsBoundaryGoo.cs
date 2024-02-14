using Baku.Params.BoidsAvoidance;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baku.Params.BoidsBoundary
{
    internal class BoidsBoundaryGoo : GH_Goo<BoidsBoundaryItem>
    {
        public BoidsBoundaryGoo() { }

        public BoidsBoundaryGoo(BoidsBoundaryItem value)
        {
            Value = value;
        }

        public override IGH_Goo Duplicate()
        {
            return new BoidsBoundaryGoo();
        }

        public override string ToString()
        {
            return $"BoidsBoundary [{Value.Weight}:{Value.Bounds.Min}:{Value.Bounds.Max}]";
        }

        public override bool IsValid {
            get
            {
                if (Value == null) return false;
                if (Value.Weight < 0 || Value.Range < 0) return false;
                if (Value.Exponent <= 0) return false;
                if (!Value.Bounds.IsValid) return false;
                return true;
            }
        }
        public override string TypeName => "BoidsBoundary";
        public override string TypeDescription => "BoidsBoundary";
    }
}
