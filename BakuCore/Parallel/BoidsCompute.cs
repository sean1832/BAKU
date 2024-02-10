using BakuCore.Kernel;
using BakuCore.Types;
using ILGPU;
using ILGPU.Runtime;

namespace BakuCore.Parallel
{
    public class BoidsCompute(Accelerator accelerator)
    {
        public Agent[] UpdateBoids(Agent[] agents, float timestep)
        {
            // create buffer
            using var deviceAgents = accelerator.Allocate1D(agents);

            // create kernel
            var kernel = accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<Agent>, float>(BoidsKernel.UpdateBoids);

            // execute kernel
            kernel((int)agents.Length, deviceAgents.View, timestep);

            // synchronize
            accelerator.Synchronize();

            // copy data from device to host
            agents = deviceAgents.GetAsArray1D();
            return agents;
        }
        
    }
}
