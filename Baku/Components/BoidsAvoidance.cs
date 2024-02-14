using System;
using System.Collections.Generic;
using System.Drawing;
using Baku.Params.BoidsAvoidance;
using Grasshopper.Kernel;

namespace Baku.Components
{
    public class BoidsAvoidance : GH_Component
    {
        #region Metadata

        public BoidsAvoidance()
            : base("Boids Avoidance", "Avoidance",
                "Description",
                Config.Category, Config.SubCategory.Behaviour)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override IEnumerable<string> Keywords => new string[] { "avoidance", "boids" };
        protected override Bitmap Icon => Icons.BoidsAvoidance;
        public override Guid ComponentGuid => new Guid("b89781f5-6c14-49cb-b18d-b014d1a602da");

        #endregion

        #region IO

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Range", "R", "Range of self avoidance",
                GH_ParamAccess.item, 0.01);
            pManager.AddNumberParameter("Weight", "W", "Weight of self avoidance", GH_ParamAccess.item, 1);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BoidsAvoidanceParam(), "Avoidance", "Avo", "Avoidance parameter",
                GH_ParamAccess.item);
        }

        #endregion

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double avoidanceRange = 0.01;
            double avoidanceWeight = 1;

            if (!DA.GetData(0, ref avoidanceRange)) return;
            if (!DA.GetData(1, ref avoidanceWeight)) return;

            var avoidance = new BoidsAvoidanceGoo(new BoidsAvoidanceItem((float)avoidanceWeight, (float)avoidanceRange));

            DA.SetData(0, avoidance);
        }
    }
}