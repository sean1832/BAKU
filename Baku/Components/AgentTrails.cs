using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using BakuCore.Types;

namespace Baku.Components
{
    public class AgentTrails : GH_Component
    {
        #region Metadata

        public AgentTrails()
            : base("Agent Trails", "Trails",
                "Draw agent trails. (This component might be slow for large data)",
                Config.Category, Config.SubCategory.Agent)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override IEnumerable<string> Keywords => new string[] { "trails" };
        protected override Bitmap Icon => Icons.AgentTrails;
        public override Guid ComponentGuid => new Guid("2502de2c-66eb-438a-a501-ca415fd52ce5");

        #endregion

        #region IO

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("AgentPositions", "Pos", "Current positions of agents", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Reset", "Reset", "Reset trails", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Trails", "T", "Trails of agents", GH_ParamAccess.list);
        }

        #endregion
        List<List<Point3d>> _trailPoints = new List<List<Point3d>>();
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> agentPositions = new List<Point3d>();
            bool reset = false;

            if (!DA.GetDataList(0, agentPositions)) return;
            if (!DA.GetData(1, ref reset)) return;

            if (reset)
            {
                _trailPoints.Clear();
            }

            // if the number of agents has changed, reset the trails
            if (_trailPoints.Count != agentPositions.Count)
            {
                Init(agentPositions);
            }

            AddTrail(agentPositions);

            List<Polyline> trails = DrawLine();

            DA.SetDataList(0, trails);
        }

        private void Init(List<Point3d> agentPositions)
        {
            _trailPoints.Clear();
            for (int i = 0; i < agentPositions.Count; i++)
            {
                _trailPoints.Add(new List<Point3d>());
            }
        }

        private void AddTrail(List<Point3d> agentPositions)
        {
            for (int i = 0; i < agentPositions.Count; i++)
            {
                _trailPoints[i].Add(agentPositions[i]);
            }
        }


        private List<Polyline> DrawLine()
        {
            var trails = new List<Polyline>();
            foreach (List<Point3d> originalPts in _trailPoints)
            {
                // Skip if there are not enough points to form a polyline
                if (originalPts.Count < 2) continue;

                // Create a new list for points, ensuring no consecutive duplicates
                var cleanedPts = new List<Point3d> { originalPts[0] }; // Always include the first point
                for (int i = 1; i < originalPts.Count; i++)
                {
                    if (!originalPts[i].Equals(originalPts[i - 1]))
                    {
                        cleanedPts.Add(originalPts[i]);
                    }
                }

                trails.Add(new Polyline(cleanedPts));
            }
            return trails;
        }

    }
}