using System;
using System.Collections.Generic;
using System.Drawing;
using Baku.Params.BoidsSeparation;
using Grasshopper.Kernel;

namespace Baku.Components
{
    public class BoidsSeparation : GH_Component
    {
        #region Metadata

        public BoidsSeparation()
            : base("BoidsSeparation", "Separation",
                "Description",
                Config.Category, Config.SubCategory.Behaviour)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override IEnumerable<string> Keywords => new string[] { };
        protected override Bitmap Icon => Icons.BoidsSeparation;
        public override Guid ComponentGuid => new Guid("cae77c87-6114-457e-ac50-c447007cdb94");

        #endregion

        #region IO

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("SeparationRange", "Range", "Range of separation", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("SeparationWeight", "Weight", "Weight of separation", GH_ParamAccess.item, 1);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BoidsSeparationParam(),"Separation", "Sep", "Agent's Separation force", GH_ParamAccess.item);
        }

        #endregion

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double separationRange = 1;
            double separationWeight = 1;

            if (!DA.GetData(0, ref separationRange)) return;
            if (!DA.GetData(1, ref separationWeight)) return;

            var separation = new BoidsSeparationGoo(new BoidsSeparationItem((float)separationWeight, (float)separationRange));

            DA.SetData(0, separation);
        }
    }
}