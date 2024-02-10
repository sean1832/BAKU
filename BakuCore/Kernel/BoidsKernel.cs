using BakuCore.Algorithm;
using BakuCore.Types;
using ILGPU;

namespace BakuCore.Kernel
{
    internal class BoidsKernel
    {
        public static void UpdateBoids(Index1D index, ArrayView<Agent> agents, float timestep)
        {
            if (index >= agents.Length)
            {
                return;
            }

            Vector3 separationForce = BoidsBehaviour.ComputeSeparation(index, agents);
            Vector3 alignmentForce = BoidsBehaviour.ComputeAlignment(index, agents);
            Vector3 cohesionForce = BoidsBehaviour.ComputeCohesion(index, agents);
            Vector3 boundaryForce = BoidsBehaviour.ComputeBoundary(index, agents, 0.001f);

            Vector3 acceleration = separationForce + alignmentForce + cohesionForce + boundaryForce;
            agents[index].Velocity += acceleration * timestep;

            // clamp velocity
            if (agents[index].Velocity.Length > agents[index].MaxSpeed)
            {
                agents[index].Velocity = Vector3.Normalize(agents[index].Velocity) * agents[index].MaxSpeed;
            }


            // update position
            agents[index].Position += agents[index].Velocity;
        }
    }
}
