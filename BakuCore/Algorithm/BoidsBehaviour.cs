using BakuCore.Types;
using ILGPU;
using System;

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

        public static Vector3 ComputeBoundary(Index1D index, ArrayView<Agent> agents)
        {
            Agent agent = agents[index];
            Vector3 force = new Vector3
            {
                X = CalculateAxisForce(agent.Position.X, agent.BoundingBox.Min.X, agent.BoundingBox.Max.X, agent.BoundaryRadius, agent.BoundaryExponent),
                Y = CalculateAxisForce(agent.Position.Y, agent.BoundingBox.Min.Y, agent.BoundingBox.Max.Y, agent.BoundaryRadius, agent.BoundaryExponent),
                Z = CalculateAxisForce(agent.Position.Z, agent.BoundingBox.Min.Z, agent.BoundingBox.Max.Z, agent.BoundaryRadius, agent.BoundaryExponent)
            };

            return force * agent.BoundaryWeight;
        }

        private static float CalculateAxisForce(float position, float min, float max, float boundaryRange, float exponentialFactor = 1.0f)
        {
            // Calculate the distance from each boundary
            float distanceToMin = position - min;
            float distanceToMax = max - position;

            // Normalize the distance to the boundaryRange
            float normalizedDistanceToMin = distanceToMin / boundaryRange;
            float normalizedDistanceToMax = distanceToMax / boundaryRange;

            // Apply an exponential function to increase the force sharply as the agent approaches the boundary
            if (distanceToMin < boundaryRange)
            {
                // The force increases exponentially as the agent gets closer to the min boundary
                return (float)Pow(1 - normalizedDistanceToMin, exponentialFactor, boundaryRange);
                //return (float)(1 - normalizedDistanceToMin);
            }
            else if (distanceToMax < boundaryRange)
            {
                // The force increases exponentially as the agent gets closer to the max boundary
                return -(float)Pow(1 - normalizedDistanceToMax, exponentialFactor, boundaryRange);
                //return (float)-(1 - normalizedDistanceToMax);
            }

            // No force applied if not within boundaryRange of either boundary
            return 0;
        }

        private static float Pow(float baseValue, float exponent, float tolerance = 0.001f)
        {
            if (Math.Abs(exponent - 1.0f) < tolerance)
            {
                // If the exponent is 1, the result is the base itself.
                return baseValue;
            }

            float result = 1.0f;
            for (int i = 0; i < (int)exponent; i++)
            {
                result *= baseValue;
            }

            // Simplified adjustment for non-integer exponents.
            if (exponent > (int)exponent)
            {
                float fractionalPart = exponent - (int)exponent;
                float partialResult = 1.0f + (baseValue - 1.0f) * fractionalPart; // This is a very basic approximation.
                result *= partialResult;
            }

            return result;
        }
    }
}
