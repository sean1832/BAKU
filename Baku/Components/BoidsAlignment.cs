using System;
using System.Collections.Generic;
using System.Drawing;
using Baku.Params.BoidsAlignment;
using Grasshopper.Kernel;

namespace Baku.Components
{
    public class BoidsAlignment : GH_Component
    {
        #region Metadata

        public BoidsAlignment()
            : base("BoidsAlignment", "Alignment",
                "Controls Boids alignment for cohesive flock movement, influencing directionality based on nearby agents. ",
                Config.Category, Config.SubCategory.Behaviour)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override IEnumerable<string> Keywords => new string[] { "alignment", "boids" };
        protected override Bitmap Icon => Icons.BoidsAlignment;
        public override Guid ComponentGuid => new Guid("9e6e0bb6-6425-4615-84f5-fb004140914b");

        #endregion

        #region IO

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("AlignmentRange", "Range", "Distance for agent alignment consideration.", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("AlignmentWeight", "Weight", "Influence scalar of alignment on movement.", GH_ParamAccess.item, 1);
        
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BoidsAlignmentParam(), "Alignment", "Ali", "Alignment force for agent.", GH_ParamAccess.item);
        }

        #endregion

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double alignmentRange = 1;
            double alignmentWeight = 1;

            if (!DA.GetData(0, ref alignmentRange)) return;
            if (!DA.GetData(1, ref alignmentWeight)) return;

            var alignment = new BoidsAlignmentGoo(new BoidsAlignmentItem((float)alignmentWeight, (float)alignmentRange));

            DA.SetData(0, alignment);
        }
    }
}