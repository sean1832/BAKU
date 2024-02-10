using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Baku.Params.BoidsCohesion
{
    internal class BoidsCohesionParam: GH_PersistentParam<BoidsCohesionGoo>
    {
        public BoidsCohesionParam() : 
            base("BoidsCohesionParam", "Cohesion", 
                "Represents cohesion param for boids algorithm", 
                Config.Category, Config.SubCategory.Params)
        {
        }

        public override Guid ComponentGuid => new Guid("509FD5E7-8B7A-48A2-B27F-2406364B8FCF");
        protected override GH_GetterResult Prompt_Singular(ref BoidsCohesionGoo value)
        {
            return GH_GetterResult.cancel;
        }

        protected override GH_GetterResult Prompt_Plural(ref List<BoidsCohesionGoo> values)
        {
            return GH_GetterResult.cancel;
        }
    }
}
