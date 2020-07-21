using _3D_Engine;

namespace Physics_Simulator
{
    public sealed class Item
    {
        public Shape Shape { get; set; }
        public Vector3D Position
        {
            get => Shape.Render_Mesh.World_Origin;
            set => Shape.Render_Mesh.World_Origin = value;
        }
        public Vector3D Velocity { get; set; }
        public Vector3D Acceleration { get; set; }

        public Item(Shape shape, Vector3D velocity, Vector3D acceleration)
        {
            Shape = shape;
            Velocity = velocity;
            Acceleration = acceleration;
        }
    }
}