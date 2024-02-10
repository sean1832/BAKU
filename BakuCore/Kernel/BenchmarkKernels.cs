using ILGPU;

namespace BakuCore.Kernel
{
    internal class BenchmarkKernels
    {
        public static void CountingNumber(Index1D index, ArrayView<int> data, ArrayView<int> output)
        {
            output[index] = data[index];
        }
    }
}
