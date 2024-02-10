using Grasshopper.Kernel.Types;

namespace Baku.Params.BoidsAlignment
{
    internal class BoidsAlignmentGoo: GH_Goo<BoidsAlignmentItem>
    {

        public BoidsAlignmentGoo() { }

        public BoidsAlignmentGoo(BoidsAlignmentItem value)
        {
            Value = value;
        }

        public override IGH_Goo Duplicate()
        {
            return new BoidsAlignmentGoo();
        }

        public override string ToString()
        {
            return $"BoidsAlignment [{Value.Weight}:{Value.Radius}]";
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
        public override string TypeName => "BoidsAlignment";
        public override string TypeDescription => "BoidsAlignment";
    }
}
