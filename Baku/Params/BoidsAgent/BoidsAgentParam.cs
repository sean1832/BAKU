using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Baku.Params.BoidsAgent
{
    internal class BoidsAgentParam: GH_PersistentParam<BoidsAgentGoo>
    {
        public BoidsAgentParam() : 
            base("BoidsAgent", "Agent", 
                "Represents boids agents", 
                Config.Category, Config.SubCategory.Params)
        {
        }

        public override Guid ComponentGuid => new Guid("3568A082-7605-47B5-8458-16BC704C2125");
        protected override GH_GetterResult Prompt_Singular(ref BoidsAgentGoo value)
        {
            return GH_GetterResult.cancel;
        }

        protected override GH_GetterResult Prompt_Plural(ref List<BoidsAgentGoo> values)
        {
            return GH_GetterResult.cancel;
        }
    }
}
