using BakuCore.Types;
using Grasshopper.Kernel.Types;

namespace Baku.Params.BoidsAgent
{
    internal class BoidsAgentGoo: GH_Goo<Agent>
    {
        public BoidsAgentGoo() { }

        public BoidsAgentGoo(Agent value)
        {
            Value = value;
        }

        public override IGH_Goo Duplicate()
        {
            return new BoidsAgentGoo();
        }

        public override string ToString()
        {
            return $"BoidsAgent [{Value.Position}]";
        }

        public override bool IsValid => true;
        public override string TypeName => "BoidsAgent";
        public override string TypeDescription => "BoidsAgent";
    }
}
