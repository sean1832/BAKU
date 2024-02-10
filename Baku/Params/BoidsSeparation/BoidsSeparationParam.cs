using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Baku.Params.BoidsSeparation
{
    internal class BoidsSeparationParam: GH_PersistentParam<BoidsSeparationGoo>
    {
        public BoidsSeparationParam() : 
            base("BoidsSeparationParam", "SeparationParam", 
                "Represents boids separation parameters", 
                Config.Category, Config.SubCategory.Params)
        {
        }

        public override Guid ComponentGuid => new Guid("48F95322-0160-443A-A532-34BA84B9DEC0");
        protected override GH_GetterResult Prompt_Singular(ref BoidsSeparationGoo value)
        {
            return GH_GetterResult.cancel;
        }

        protected override GH_GetterResult Prompt_Plural(ref List<BoidsSeparationGoo> values)
        {
            return GH_GetterResult.cancel;
        }
    }
}
