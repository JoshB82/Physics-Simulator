using _3D_Engine;
using System.Drawing;
using System.Windows.Forms;

namespace Physics_Simulator
{
    public partial class Main_Form : Form
    {
        private Scene scene;

        public Main_Form()
        {
            // Create form
            InitializeComponent();

            // Create scene
            scene = new Scene(Canvas_Box, Canvas_Box.Width, Canvas_Box.Height);

            // Create origin and axes
            scene.Create_Origin();
            scene.Create_Axes();

            // Create camera
            double camera_width = Canvas_Box.Width / 10;
            double camera_height = Canvas_Box.Height / 10;

            Perspective_Camera camera = new Perspective_Camera(new Vector3D(0, 0, 100), scene.Shape_List[0].Render_Mesh, Vector3D.Unit_Y, camera_width, camera_height, 10, 750);
            scene.Render_Camera = camera;
            
            // Render the scene
            scene.Render();
        }

        private void Canvas_Box_Paint(object sender, PaintEventArgs e) => e.Graphics.DrawImageUnscaled(scene.Canvas, Point.Empty);
    }
}
