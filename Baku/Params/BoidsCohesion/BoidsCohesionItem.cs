namespace Baku.Params.BoidsCohesion
{
    internal class BoidsCohesionItem(float weigh, float radius)
    {
        public float Weight { get; set; } = weigh;
        public float Radius { get; set; } = radius;
    }
}
