using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Baku.Params.BoidsAvoidance
{
    internal class BoidsAvoidanceParam: GH_PersistentParam<BoidsAvoidanceGoo>
    {
        public BoidsAvoidanceParam() : 
            base("BoidsAvoidanceParam", "AvoidanceParam", "Avoidance Parameter",
                Config.Category, Config.SubCategory.Params)
        {
        }

        public override Guid ComponentGuid => new Guid("E790BABC-CC63-48FF-84DC-E19EE60D82B0");
        protected override GH_GetterResult Prompt_Singular(ref BoidsAvoidanceGoo value)
        {
            return GH_GetterResult.cancel;
        }

        protected override GH_GetterResult Prompt_Plural(ref List<BoidsAvoidanceGoo> values)
        {
            return GH_GetterResult.cancel;
        }
    }
}
