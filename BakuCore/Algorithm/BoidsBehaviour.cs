using BakuCore.Types;
using ILGPU;

namespace BakuCore.Algorithm
{
    internal struct BoidsBehaviour
    {
        /// <summary>
        /// GPU accelerated separation behaviour
        /// </summary>
        /// <param name="index"></param>
        /// <param name="agents"></param>
        /// <returns></returns>
        public static Vector3 ComputeSeparation(Index1D index, ArrayView<Agent> agents)
        {
            Vector3 force = new Vector3();
            int count = 0;

            for (int i = 0; i < agents.Length; i++)
            {
                if (i == index) continue;

                float distance = Vector3.Distance(agents[index].Position, agents[i].Position);
                if (!(distance < agents[index].SeparationRadius) || !(distance > 0)) continue;
                if (!IsWithinFov(agents[index], agents[i])) continue;

                // neighbour is within the radius
                Vector3 direction = Vector3.Normalize(agents[index].Position - agents[i].Position);
                // separation logic
                force += direction / distance;
                count++;
            }

            if (count > 0)
            {
                force /= count; // average the force
            }

            return force * agents[index].SeparationWeight;
        }

        public static Vector3 ComputeCohesion(Index1D index, ArrayView<Agent> agents)
        {
            Vector3 sumPosition = new Vector3();
            int count = 0;

            for (int i = 0; i < agents.Length; i++)
            {
                if (i == index) continue;

                float distance = Vector3.Distance(agents[index].Position, agents[i].Position);
                if (!(distance < agents[index].CohesionRadius) || !(distance > 0)) continue;
                if (!IsWithinFov(agents[index], agents[i])) continue;

                // neighbour is within the radius
                sumPosition += agents[i].Position; // sum all position within the radius
                count++;
            }

            if (count > 0)
            {
                sumPosition /= count; // average the position
                Vector3 direction = Vector3.Normalize(sumPosition - agents[index].Position);
                return direction * agents[index].CohesionWeight;
            }

            return new Vector3();
        }

        public static Vector3 ComputeAlignment(Index1D index, ArrayView<Agent> agents)
        {
            Vector3 sumVelocity = new Vector3();
            int count = 0;

            for (int i = 0; i < agents.Length; i++)
            {
                if (i == index) continue;

                float distance = Vector3.Distance(agents[index].Position, agents[i].Position);
                if (!(distance < agents[index].AlignmentRadius) || !(distance > 0)) continue;
                if (!IsWithinFov(agents[index], agents[i])) continue;

                // neighbour is within the radius
                sumVelocity += agents[i].Velocity; // sum all velocity within the radius
                count++;
            }

            if (count > 0)
            {
                sumVelocity /= count; // average the velocity
                return (sumVelocity - agents[index].Velocity) * agents[index].AlignmentWeight;
            }

            return new Vector3();
        }

        private static bool IsWithinFov(Agent agent, Agent target)
        {
            Vector3 toTarget = Vector3.Normalize(target.Position - agent.Position);
            float angleToTarget = Vector3.Dot(agent.Velocity, toTarget);
            return angleToTarget <= agent.Fov / 2; // fov is centered around agent's velocity direction
        }

        public static Vector3 ComputeBoundary(Index1D index, ArrayView<Agent> agents, float tolerance)
        {
            Agent agent = agents[index];
            Vector3 force = new Vector3
            {
                X = CalculateDimensionForce(agent.Position.X, agent.BoundingBox.Min.X, agent.BoundingBox.Max.X, tolerance),
                Y = CalculateDimensionForce(agent.Position.Y, agent.BoundingBox.Min.Y, agent.BoundingBox.Max.Y, tolerance),
                Z = CalculateDimensionForce(agent.Position.Z, agent.BoundingBox.Min.Z, agent.BoundingBox.Max.Z, tolerance)
            };

            return force * agent.BoundaryWeight;
        }

        private static float CalculateDimensionForce(float position, float min, float max, float tolerance)
        {
            if (position < min + tolerance)
            {
                return (min + tolerance - position) / tolerance;
            }
            else if (position > max - tolerance)
            {
                return -(position - (max - tolerance)) / tolerance;
            }
            return 0;
        }
    }
}
