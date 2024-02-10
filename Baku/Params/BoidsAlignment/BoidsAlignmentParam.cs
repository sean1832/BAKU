using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Baku.Params.BoidsAlignment
{
    internal class BoidsAlignmentParam: GH_PersistentParam<BoidsAlignmentGoo>
    {
        public BoidsAlignmentParam() : 
            base("BoidsAlignmentParam", "Alignment", 
                "Represents boids alignment parameters", 
                Config.Category, Config.SubCategory.Params)
        {
        }
        public override Guid ComponentGuid => new Guid("566A3BDC-AEE1-4CFD-B879-D06392C597D6");
        protected override GH_GetterResult Prompt_Singular(ref BoidsAlignmentGoo value)
        {
            return GH_GetterResult.cancel;
        }

        protected override GH_GetterResult Prompt_Plural(ref List<BoidsAlignmentGoo> values)
        {
            return GH_GetterResult.cancel;
        }
    }
}
