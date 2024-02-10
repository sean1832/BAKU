using Grasshopper.Kernel.Types;

namespace Baku.Params.BoidsSeparation
{
    internal class BoidsSeparationGoo: GH_Goo<BoidsSeparationItem>
    {

        public BoidsSeparationGoo() { }

        public BoidsSeparationGoo(BoidsSeparationItem value)
        {
            Value = value;
        }

        public override IGH_Goo Duplicate()
        {
            return new BoidsSeparationGoo();
        }

        public override string ToString()
        {
            return $"BoidsSeparation [{Value.Weight}:{Value.Radius}]";
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
        public override string TypeName => "BoidsSeparation";
        public override string TypeDescription => "BoidsSeparation";
    }
}
