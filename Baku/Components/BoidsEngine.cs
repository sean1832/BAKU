using System;
using System.Collections.Generic;
using System.Drawing;
using Baku.Params.Accelerator;
using Baku.Params.BoidsAgent;
using BakuCore;
using BakuCore.Parallel;
using BakuCore.Types;
using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;

namespace Baku.Components
{
    public class BoidsEngine : GH_Component
    {
        #region Metadata

        public BoidsEngine()
            : base("Boids Engine", "Boids Engine",
                "Execute boids algorithm simulation",
                Config.Category, Config.SubCategory.Engine)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override IEnumerable<string> Keywords => new string[] { "boids", "engine" };
        protected override Bitmap Icon => Icons.BoidsEngine;
        public override Guid ComponentGuid => new Guid("67098345-189f-4279-9227-78c9c531c904");

        #endregion

        #region IO

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new AcceleratorParam(), "Accelerator", "Acc", "Accelerator device to run",
                GH_ParamAccess.item);
            pManager.AddParameter(new BoidsAgentParam(), "Agent", "A", "Represents agents for boids algorithm",
                GH_ParamAccess.list);

            pManager.AddNumberParameter("Speed", "S", "timestep for agent", GH_ParamAccess.item, 1.0);

            pManager.AddIntegerParameter("Interval", "I",
                "Simulation Interval (Higher the number, slower the simulation)", GH_ParamAccess.item, 50);

            pManager.AddBooleanParameter("Run", "Run", "Run simulation", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Reset", "Reset", "reset agents to its starting position",
                GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Agent Points", "pt", "Agent locations", GH_ParamAccess.list);
            pManager.AddVectorParameter("Velocity", "V", "Velocity of each agents", GH_ParamAccess.list);
        }

        #endregion

        private GpuContext _lastContext;
        private int _lastIndexValue = -1;
        private Agent[] _lastAgents;
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            AcceleratorGoo indexGoo = null;
            var agents = new List<BoidsAgentGoo>();
            double timestep = 1.0;
            var interval = 50;
            var run = false;
            var reset = false;

            if (!DA.GetData(0, ref indexGoo)) return;
            if (!DA.GetDataList(1, agents)) return;
            if (!DA.GetData(2, ref timestep)) return;
            if (!DA.GetData(3, ref interval)) return;
            if (!DA.GetData(4, ref run)) return;
            if (!DA.GetData(5, ref reset)) return;


            if (!indexGoo.IsValid)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid accelerator index");
                Dispose();
                DA.AbortComponentSolution();
                return;
            }

            if (agents.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No agent");
                DA.AbortComponentSolution();
                Dispose();
                return;
            }

            // initialize context
            if (_lastIndexValue != indexGoo.Value.Index)
            {
                _lastContext = Initialise(indexGoo.Value.Index);
                _lastIndexValue = indexGoo.Value.Index;
            }

            Message = GetDeviceName(indexGoo);

            

            List<Point3d> ptResults = [];
            List<Vector3d> velocityResults = [];

            if (reset)
            {
                // flush previous agents
                FlushAgents();
            }

            if (run)
            {
                // store agents
                if (_lastAgents == null)
                {
                    _lastAgents = new Agent[agents.Count];
                    for (int i = 0; i < agents.Count; i++)
                    {
                        _lastAgents[i] = agents[i].Value;
                    }
                }
                

                BoidsCompute boidsCompute = new BoidsCompute(_lastContext.GetAccelerator());

                // run simulation
                OnPingDocument().ScheduleSolution(interval, d =>
                {
                    ExpireSolution(false);
                    _lastAgents = boidsCompute.UpdateBoids(_lastAgents, (float)timestep);
                });
                
            }

            if (_lastAgents != null)
            {
                // unpack agents
                ptResults = new();
                foreach (Agent lastAgent in _lastAgents)
                {
                    ptResults.Add(new Point3d(lastAgent.Position.X, lastAgent.Position.Y, lastAgent.Position.Z));
                    velocityResults.Add(new Vector3d(lastAgent.Velocity.X, lastAgent.Velocity.Y, lastAgent.Velocity.Z));
                }
            }

            DA.SetDataList(0, ptResults);
            DA.SetDataList(1, velocityResults);

        }

        private GpuContext Initialise(int index)
        {
            try
            {
                var deviceContext = new GpuContext(index);
                return deviceContext;
            }
            catch
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No accelerator found");
                Dispose();
                throw;
            }
        }

        // Component Lifecycle
        public void Dispose()
        {
            _lastContext?.Dispose();
            _lastContext = null;
            _lastIndexValue = -1; // invalid index
            FlushAgents();
        }

        private void FlushAgents()
        {
            _lastAgents = null;
        }

        // when component is removed from the document
        public override void RemovedFromDocument(GH_Document document)
        {
            Dispose();
        }

        // before solving the instance
        protected override void BeforeSolveInstance()
        {
            RhinoDoc.CloseDocument += OnRhinoCloseDocument;
            RhinoApp.Closing += OnRhinoCloseApp;
        }

        private void OnRhinoCloseDocument(object sender, DocumentEventArgs e)
        {
            Dispose();
        }

        private void OnRhinoCloseApp(object sender, EventArgs e)
        {
            Dispose();
        }

        private string GetDeviceName(AcceleratorGoo goo)
        {
            return goo.Value.Name;
        }
    }
}