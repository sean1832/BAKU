using Baku.Params.BoidsCohesion;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baku.Params.BoidsBoundary
{
    internal class BoidsBoundaryParam : GH_PersistentParam<BoidsBoundaryGoo>
    {
        public BoidsBoundaryParam() :
            base("BoidsBoundaryParam", "BoidsBoundaryParam",
                "Boids boundary parameter",
                Config.Category, Config.SubCategory.Params)
        {
        }

        public override Guid ComponentGuid => new Guid("df8547d9-2fe6-4a68-a045-7e2a4b79ca31");
        protected override GH_GetterResult Prompt_Singular(ref BoidsBoundaryGoo value)
        {
            return GH_GetterResult.cancel;
        }

        protected override GH_GetterResult Prompt_Plural(ref List<BoidsBoundaryGoo> values)
        {
            return GH_GetterResult.cancel;
        }
    }
}