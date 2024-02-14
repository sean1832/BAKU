using System;
using System.Collections.Generic;
using System.Drawing;
using Baku.Params.Accelerator;
using BakuCore;
using Grasshopper.Kernel;

namespace Baku.Components
{
    public class AcceleratorContext : GH_Component
    {
        #region Metadata

        public AcceleratorContext()
            : base("Accelerator Context", "Accelerator",
                "Defines accelerator context",
                Config.Category, Config.SubCategory.Utils)
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override IEnumerable<string> Keywords => new string[] { "Accelerator" };
        protected override Bitmap Icon => Icons.DeviceContext;
        public override Guid ComponentGuid => new Guid("1b368c66-01e7-44fe-9dab-c8e43da0cdc0");

        #endregion

        #region IO

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("DeviceIndex", "Idx", "Device Index to select", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("DevicesInfo", "DI", "All Accelerator devices available on this computer",
                GH_ParamAccess.list);
            pManager.AddParameter(new AcceleratorParam(), "Accelerator", "Acc", "Current Accelerator device", GH_ParamAccess.item);
        }

        #endregion

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int deviceIndex = 0;

            if (!DA.GetData(0, ref deviceIndex)) return;

            if (deviceIndex < 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Device index must be greater than 0");
                DA.AbortComponentSolution();
                return;
            }

            try
            {
                using var deviceContext = new GpuContext(deviceIndex);
                var devices = deviceContext.GetDeviceNames();
                string device = null;
                string[] devicesName = new string[devices.Length];

                for (int i = 0; i < devices.Length; i++)
                {
                    devicesName[i] = devices[i];
                    if (i == deviceIndex)
                    {
                        device = devices[i];
                    }
                }

                Message = devicesName[deviceIndex];

                DA.SetDataList(0, devicesName);
                DA.SetData(1, new AcceleratorGoo(new AcceleratorIndex(deviceIndex, device)));
            }
            catch (IndexOutOfRangeException)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Error: Invalid device index. Please ensure you select a valid index based on the available devices.");
                DA.AbortComponentSolution();
            }
        }
    }
}