using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Baku.Params.Accelerator;
using BakuCore;
using BakuCore.Parallel;
using Grasshopper.Kernel;
using Rhino;

namespace Baku.Components
{
    public class Benchmark : GH_Component, IDisposable
    {
        #region Metadata

        public Benchmark()
            : base("Benchmark", "Benchmark",
                "Benchmark device process speed for debugging GPU",
                Config.Category, Config.SubCategory.Utils)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override IEnumerable<string> Keywords => new string[] { };
        protected override Bitmap Icon => Icons.Benchmark;
        public override Guid ComponentGuid => new Guid("8e48026e-b4ce-40ad-a379-6a6c371f37b7");

        #endregion

        #region IO

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new AcceleratorParam(), "Accelerator", "Acc", "Current Accelerator device", GH_ParamAccess.item);
            pManager.AddNumberParameter("Iterations", "Iter", "Number to test", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Output", "O", "Output", GH_ParamAccess.item);
            pManager.AddTextParameter("Info", "I", "Info", GH_ParamAccess.list);
        }

        #endregion

        private GpuContext _lastContext;
        private int _lastIndexValue = -1;
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            AcceleratorGoo indexGoo = null;
            double iterations = 0;

            if (!DA.GetData(0, ref indexGoo)) return;
            if (!DA.GetData(1, ref iterations)) return;
            Stopwatch sw1 = new Stopwatch();
            Stopwatch sw2 = new Stopwatch();
            Stopwatch sw3 = new Stopwatch();
            List<string> info = new();

            if (iterations <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Iterations must be greater than 0");
                Dispose();
                DA.AbortComponentSolution();
                return;
            }

            if (!indexGoo.IsValid)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid accelerator index");
                Dispose();
                DA.AbortComponentSolution();
                return;
            }

            // initialize
            sw1.Start();
            if (_lastIndexValue != indexGoo.Value.Index)
            {
                _lastContext = Initialise(indexGoo.Value.Index);
                _lastIndexValue = indexGoo.Value.Index;
            }
            sw1.Stop();
            info.Add($"Initialization: {sw1.ElapsedMilliseconds} ms");


            // implement kernel here
            sw3.Start();
            int[] result;
            try
            {
                Message = GetDeviceName(indexGoo);
                BenchmarkCompute compute = new(_lastContext.GetAccelerator());
                result = compute.CountingNumber((int)iterations);
            }
            catch (Exception e)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, e.Message);
                Dispose();
                DA.AbortComponentSolution();
                return;
            }
            sw3.Stop();
            info.Add($"Execute Kernel: {sw3.ElapsedMilliseconds} ms");
            
            // output
            DA.SetData(0, result[result.Length - 1]);
            DA.SetDataList(1, info);
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