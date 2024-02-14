using System;
using System.Collections.Generic;
using System.Drawing;
using Baku.Params.BoidsAgent;
using Baku.Params.BoidsAlignment;
using Baku.Params.BoidsAvoidance;
using Baku.Params.BoidsBoundary;
using Baku.Params.BoidsCohesion;
using Baku.Params.BoidsSeparation;
using BakuCore.Types;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Baku.Components
{
    public class BoidsAgent : GH_Component
    {
        #region Metadata

        public BoidsAgent()
            : base("ConstructAgent", "Agent",
                "Constructs a Boids agent to simulate flocking behavior in a 3D environment. " +
                "Agents follow simple rules to exhibit complex swarm behavior, such as separation, " +
                "alignment, and cohesion within a defined boundary.",
                Config.Category, Config.SubCategory.Agent)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override IEnumerable<string> Keywords => new string[] { "agent", "boids" };
        protected override Bitmap Icon => Icons.BoidsAgent;
        public override Guid ComponentGuid => new Guid("5810f42d-e2f6-43c6-b95c-278b4ec53233");

        #endregion

        #region IO

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "Pt", "Starting points for agents.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Max speed", "MxS", "Specifies the maximum speed at which an agent can move, influencing how fast agents can respond to their environment.", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Field of view", "Fov", "Determines the field of view for each agent in degrees, affecting how agents perceive their surroundings.", GH_ParamAccess.item, 360);
            pManager.AddParameter(new BoidsSeparationParam(), "Separation", "Sep", "Defines the minimum distance agents try to maintain from each other to avoid crowding.", GH_ParamAccess.item);
            pManager.AddParameter(new BoidsCohesionParam(), "Cohesion", "Coh", "Defines the range within which agents attempt to move closer to form a group.", GH_ParamAccess.item);
            pManager.AddParameter(new BoidsAlignmentParam(), "Alignment", "Ali", "Defines the force to which an agent aligns its direction with that of nearby agents.", GH_ParamAccess.item);
            pManager.AddParameter(new BoidsBoundaryParam(), "Boundary", "B", "Defines the maximum volume within which agents can operate, constraining their movement to this 3D space.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BoidsAgentParam(), "Agent", "A", "Outputs the constructed agents.", GH_ParamAccess.list);
        }

        #endregion

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var tempBox = new Box().BoundingBox;

            var points = new List<Point3d>();
            double maxSpeed = 1.0;
            double fov = 360.0;
            var separation = new BoidsSeparationGoo(new BoidsSeparationItem(1, 1));
            var cohesion = new BoidsCohesionGoo(new BoidsCohesionItem(1, 1));
            var alignment = new BoidsAlignmentGoo(new BoidsAlignmentItem(1, 1));
            var bounds = new BoidsBoundaryGoo(new BoidsBoundaryItem(tempBox, 1, 2, 0.1f));

            if (!DA.GetDataList(0, points)) return;
            if (!DA.GetData(1, ref maxSpeed)) return;
            if (!DA.GetData(2, ref fov)) return;

            if (!DA.GetData(3, ref separation)) return;
            if (!DA.GetData(4, ref cohesion)) return;
            if (!DA.GetData(5, ref alignment)) return;
            if (!DA.GetData(6, ref bounds)) return;

            BoundingBox bbox = bounds.Value.Bounds;
            Vector3 min = new Vector3((float)bbox.Min.X, (float)bbox.Min.Y, (float)bbox.Min.Z);
            Vector3 max = new Vector3((float)bbox.Max.X, (float)bbox.Max.Y, (float)bbox.Max.Z);

            List<BoidsAgentGoo> agents = new List<BoidsAgentGoo>();
            foreach (var point in points)
            {
                Agent agent = new Agent(new Vector3((float)point.X, (float)point.Y, (float)point.Z))
                    {
                        BoundingBox = new MBoundingBox(min, max),
                        BoundaryWeight = bounds.Value.Weight,
                        BoundaryExponent = bounds.Value.Exponent,
                        BoundaryRadius = bounds.Value.Range,
                        MaxSpeed = (float)maxSpeed,
                        Fov = (float)fov,
                        SeparationWeight = separation.Value.Weight,
                        SeparationRadius = separation.Value.Radius,
                        CohesionWeight = cohesion.Value.Weight,
                        CohesionRadius = cohesion.Value.Radius,
                        AlignmentWeight = alignment.Value.Weight,
                        AlignmentRadius = alignment.Value.Radius,
                };
                agents.Add(new BoidsAgentGoo(agent));
            }

            DA.SetDataList(0, agents);
        }
    }
}