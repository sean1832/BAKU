using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;

namespace Baku.Params.Accelerator
{
    internal class AcceleratorParam : GH_PersistentParam<AcceleratorGoo>
    {
        public override Guid ComponentGuid => new Guid("DBAF34C8-DE45-49C9-A6CE-34BA63B31DA8");

        public AcceleratorParam() :
            base("Accelerator", "Accelerator",
                "Represents accelerator device",
                Config.Category, Config.SubCategory.Params)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override Bitmap Icon => null;

        protected override GH_GetterResult Prompt_Plural(ref List<AcceleratorGoo> values)
        {
            return GH_GetterResult.cancel;
        }

        protected override GH_GetterResult Prompt_Singular(ref AcceleratorGoo value)
        {
            return GH_GetterResult.cancel;
        }
    }
}

