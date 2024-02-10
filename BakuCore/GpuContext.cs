using System;
using ILGPU;
using ILGPU.Runtime;

namespace BakuCore
{
    public class GpuContext: IDisposable
    {
        private Accelerator _accelerator;
        private Context _context;

        public GpuContext(int index)
        {
            CreateContext();
            Init(index);
        }
        public Accelerator GetAccelerator()
        {
            return _accelerator;
        }

        public string[] GetDeviceNames()
        {
            var devices = _context.Devices;
            var names = new string[devices.Length];
            for (int i = 0; i < devices.Length; i++)
            {
                names[i] = devices[i].Name;
            }
            return names;
        }

        private void Init(int index)
        {
            
            _accelerator = _context.Devices[index].CreateAccelerator(_context);
        }

        private void CreateContext()
        {
            _context = Context.Create(builder => builder.AllAccelerators());
        }

        public void Dispose()
        {
            _accelerator?.Dispose();
            _context?.Dispose();
        }
    }
}
