using System;
using System.Collections.Generic;
using System.Drawing;
using Baku.Params.BoidsCohesion;
using Grasshopper.Kernel;

namespace Baku.Components
{
    public class BoidsCohesion : GH_Component
    {
        #region Metadata

        public BoidsCohesion()
            : base("Boids Cohesion", "Cohesion",
                "Manages cohesive behavior, drawing Boids towards the average position of neighbors for group integrity.",
                Config.Category, Config.SubCategory.Behaviour)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override IEnumerable<string> Keywords => new string[] { "cohesion", "boids" };
        protected override Bitmap Icon => Icons.BoidsCohesion;
        public override Guid ComponentGuid => new Guid("99a24176-4359-4dac-8f12-3301f732f82f");

        #endregion

        #region IO

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Range", "R", "Defines how far an agent checks for neighbors to calculate the average position for cohesion.", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Weight", "W", "Defines the strength of the pull towards the group's average position.", GH_ParamAccess.item, 1);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BoidsCohesionParam(), "Cohesion", "Coh", "Alignment force for agent.",
                GH_ParamAccess.item);
        }

        #endregion

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double cohesionRange = 1;
            double cohesionWeight = 1;

            if (!DA.GetData(0, ref cohesionRange)) return;
            if (!DA.GetData(1, ref cohesionWeight)) return;

            var cohesion = new BoidsCohesionGoo(new BoidsCohesionItem((float)cohesionWeight, (float)cohesionRange));

            DA.SetData(0, cohesion);
        }
    }
}