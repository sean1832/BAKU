using Grasshopper.Kernel.Types;

namespace Baku.Params.BoidsCohesion
{
    internal class BoidsCohesionGoo: GH_Goo<BoidsCohesionItem>
    {
        public BoidsCohesionGoo() { }

        public BoidsCohesionGoo(BoidsCohesionItem value)
        {
            Value = value;
        }

        public override IGH_Goo Duplicate()
        {
            return new BoidsCohesionGoo();
        }

        public override string ToString()
        {
            return $"BoidsCohesion [{Value.Weight}:{Value.Radius}]";
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
        public override string TypeName => "BoidsCohesion";
        public override string TypeDescription => "BoidsCohesion";
    }
}
