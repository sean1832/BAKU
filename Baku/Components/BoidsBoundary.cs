using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using Baku.Params.BoidsBoundary;

namespace Baku.Components
{
    public class BoidsBoundary : GH_Component
    {
        #region Metadata

        public BoidsBoundary()
            : base("Boids Boundary", "Boundary",
                "Controls boids boundary repulsion force.",
                Config.Category, Config.SubCategory.Behaviour)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override IEnumerable<string> Keywords => new string[] { "boundary", "boids" };
        protected override Bitmap Icon => Icons.BoidsBoundary;
        public override Guid ComponentGuid => new Guid("cd5e2237-dd21-426c-ac57-68eb87fdb3a9");

        #endregion

        #region IO

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBoxParameter("BoundingBox", "BBox", "Defines the maximum volume within which agents can operate, constraining their movement to this 3D space.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Range", "R", "Defines the range of the boundary, influencing how far agents are affected by the boundary.", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Weight", "W", "Defines the weight of the boundary, influencing how much agents are affected by the boundary.", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Exponent", "^x", "Defines the gradient force of the boundary, 1 being linear and 2 being quadratic.", GH_ParamAccess.item, 2);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BoidsBoundaryParam(), "Boundary", "B", "Outputs the constructed boundary.", GH_ParamAccess.item);
        }

        #endregion

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var box = new Box();
            double range = 1.0;
            double weight = 1.0;
            double factor = 2.0;

            if (!DA.GetData(0, ref box)) return;
            if (!DA.GetData(1, ref range)) return;
            if (!DA.GetData(2, ref weight)) return;
            if (!DA.GetData(3, ref factor)) return;

            var boundary = new BoidsBoundaryGoo(new BoidsBoundaryItem(box.BoundingBox, (float)weight, (float)range, (float)factor));

            DA.SetData(0, boundary);
        }
    }
}