namespace BakuCore.Types
{
    public struct Agent
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public MBoundingBox BoundingBox { get; set; }
        public float BoundaryWeight { get; set; }

        // field of view
        public float Fov { get; set; }

        // speed
        public float MaxSpeed { get; set; }

        // separation
        public float SeparationRadius { get; set; }
        public float SeparationWeight { get; set; }

        // cohesion
        public float CohesionRadius { get; set; }
        public float CohesionWeight { get; set; }

        // alignment
        public float AlignmentWeight { get; set; }
        public float AlignmentRadius { get; set; }

        // avoidance
        public float AvoidanceRadius { get; set; }
        public float AvoidanceWeight { get; set; }

        // obstacle
        public float ObstacleWeight { get; set; }
        public float ObstacleRadius { get; set; }

        // target
        public float TargetWeight { get; set; }
        public float TargetRadius { get; set; }


        public Agent()
        {
            Position = new Vector3();
            BoundingBox = new MBoundingBox();
            Velocity = new Vector3(1, 1, 1);
            DefaultParams();
        }

        public Agent(Vector3 position)
        {
            Position = position;
            BoundingBox = new MBoundingBox();
            Velocity = new Vector3(1, 1, 1);

            DefaultParams();
        }

        public Agent(Vector3 position, MBoundingBox bbox)
        {
            Position = position;
            BoundingBox = bbox;
            Velocity = new Vector3(1,1,1);
            DefaultParams();
        }

        private void DefaultParams()
        {
            // boundary
            BoundaryWeight = 1;

            // separation
            SeparationRadius = 1;
            SeparationWeight = 1;

            // cohesion
            CohesionRadius = 1;
            CohesionWeight = 1;

            // alignment
            AlignmentRadius = 1;
            AlignmentWeight = 1;

            // avoidance
            AvoidanceWeight = 1;
            AvoidanceRadius = 1;

            // obstacle
            ObstacleWeight = 1;
            ObstacleRadius = 1;

            // target
            TargetWeight = 1;
            TargetRadius = 1;
        }
        
    }
}
