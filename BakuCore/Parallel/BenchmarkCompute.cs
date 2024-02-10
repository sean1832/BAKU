using BakuCore.Kernel;
using ILGPU;
using ILGPU.Runtime;

namespace BakuCore.Parallel
{
    public class BenchmarkCompute
    {
        private readonly Accelerator _accelerator;

        public BenchmarkCompute(Accelerator accelerator)
        {
            _accelerator = accelerator;
        }

        public int[] CountingNumber(int number)
        {
            // set data
            int[] sourceData = SetData(number);

            // create buffer
            using var deviceData = _accelerator.Allocate1D(sourceData);
            using var deviceOutput = _accelerator.Allocate1D<int>(sourceData.Length);

            // create kernel
            var kernel = _accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<int>, ArrayView<int>>(BenchmarkKernels.CountingNumber);

            // execute kernel
            kernel((int)sourceData.Length, deviceData.View, deviceOutput.View);

            // synchronize
            _accelerator.Synchronize();

            // copy data from device to host
            int[] result = deviceOutput.GetAsArray1D();
            return result;
        }

        private int[] SetData(int number)
        {
            int[] data = new int[number];

            for (int i = 0; i < number; i++)
            {
                data[i] = i;
            }

            return data;
        }
    }
}
