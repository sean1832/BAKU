namespace Baku.Params.Accelerator
{
    internal class AcceleratorIndex
    {
        private readonly int _index;
        private readonly string _name;

        public AcceleratorIndex() { }
        public AcceleratorIndex(int index, string name)
        {
            _index = index;
            _name = name;
        }

        public (int, string) Get()
        {
            return (_index, _name);
        }
        public int Index => _index;
        public string Name => _name;
    }
}
