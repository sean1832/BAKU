namespace Baku.Params.BoidsAvoidance
{
    internal class BoidsAvoidanceItem(float weight, float radius)
    {
        public float Weight { get; set; } = weight;
        public float Radius { get; set; } = radius;
    }
}
