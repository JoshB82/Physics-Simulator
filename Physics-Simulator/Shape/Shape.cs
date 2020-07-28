using _3D_Engine;

namespace Physics_Simulator
{
    public sealed class Shape
    {
        #region Fields and Properties

        // ID
        /// <summary>
        /// Identification number.
        /// </summary>
        public int ID { get; private set; }
        private static int next_id = -1;

        // Meshes
        public Mesh Collision_Mesh { get; set; }
        public Mesh Render_Mesh { get; set; }

        public Vector3D Position
        {
            get => Collision_Mesh.World_Origin;
            set
            {
                Collision_Mesh.World_Origin = value;
                Render_Mesh.World_Origin = value;
            }
        }
        public Vector3D Velocity { get; set; }
        public Vector3D Acceleration { get; set; }

        // Appearance
        public bool Selected { get; set; } = false;
        
        #endregion

        #region Constructors

        public Shape(Mesh collision_mesh, Mesh render_mesh, Vector3D velocity, Vector3D acceleration)
        {
            ID = ++next_id;

            Collision_Mesh = collision_mesh;
            Render_Mesh = render_mesh;
            
            Velocity = velocity;
            Acceleration = acceleration;
        }

        public Shape(Mesh mesh, Vector3D velocity, Vector3D acceleration) : this(mesh, mesh, velocity, acceleration) { }

        #endregion
    }
}