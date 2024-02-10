namespace BakuCore.Types
{
    public struct MBoundingBox
    {
        public Vector3 Min;
        public Vector3 Max;

        public MBoundingBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public Vector3 Center => (Min + Max) * 0.5f;
        public Vector3 Extents => (Max - Min) * 0.5f;

        public MBoundingBox Expand(float amount)
        {
            return new MBoundingBox(Min - new Vector3(amount, amount, amount), Max + new Vector3(amount, amount, amount));
        }
    }
}
