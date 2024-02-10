using Grasshopper.Kernel.Types;

namespace Baku.Params.BoidsAvoidance
{
    internal class BoidsAvoidanceGoo: GH_Goo<BoidsAvoidanceItem>
    {
        public BoidsAvoidanceGoo() { }

        public BoidsAvoidanceGoo(BoidsAvoidanceItem value)
        {
            Value = value;
        }

        public override IGH_Goo Duplicate()
        {
            return new BoidsAvoidanceGoo();
        }

        public override string ToString()
        {
            return $"BoidsAvoidance [{Value.Weight}:{Value.Radius}]";
        }

        public override bool IsValid
        {
            get
            {
                if (Value == null) return false;
                if (Value.Weight < 0 || Value.Radius < 0) return false;
                return true;
            }
        }

        public override string TypeName => "BoidsAvoidance";
        public override string TypeDescription => "BoidsAvoidance";
    }
}
