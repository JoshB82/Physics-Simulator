using _3D_Engine;
using Physics_Simulator.Shapes;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Physics_Simulator
{
    public partial class Main_Form : Form
    {
        private Scene scene;
        private List<Item> scene_items = new List<Item>();

        private bool running = true;
        private long update_time;

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

            // Add some shapes
            Cube cube_mesh = new Cube(new Vector3D(10, 10, 10), Vector3D.Unit_Negative_Z, Vector3D.Unit_Y, 100);
            Shape cube = new Shape(cube_mesh);
            scene.Add(cube);
            Item cube_item = new Item(cube, Vector3D.Zero, new Vector3D(0, Constants.Grav_Acc, 0));
            scene_items.Add(cube_item);

            // Start loop
            Thread thread = new Thread(Loop) { IsBackground = true };
            thread.Start();
        }

        private void Loop()
        {
            const int max_frames_per_second = 60;
            const int max_updates_per_second = 60;
            const long frame_minimum_time = 1000 / max_frames_per_second;
            const long update_minimum_time = 1000 / max_updates_per_second;

            int no_frames = 0, no_updates = 0, timer = 1;

            long now_time, delta_time, frame_time = 0;
            
            Stopwatch sw = Stopwatch.StartNew();
            long start_time = sw.ElapsedMilliseconds;
            
            while (running)
            {
                now_time = sw.ElapsedMilliseconds;
                delta_time = now_time - start_time;
                start_time = now_time;

                frame_time += delta_time;
                update_time += delta_time;

                if (frame_time >= frame_minimum_time)
                {
                    scene.Render();
                    no_frames++;
                    frame_time -= frame_minimum_time;
                }

                if (update_time >= update_minimum_time)
                {
                    Update_Position();
                    no_updates++;
                    update_time -= update_minimum_time;
                }

                if (now_time >= 1000 * timer)
                {
                    Invoke((MethodInvoker)delegate { Text = $"Physics Simulator - FPS: {no_frames}, UPS: {no_updates}"; }); // ?
                    no_frames = 0; no_updates = 0;
                    timer += 1;
                }
            }
        }

        private void Canvas_Box_Paint(object sender, PaintEventArgs e) => e.Graphics.DrawImageUnscaled(scene.Canvas, Point.Empty);

        private void Update_Position()
        {
            foreach (Item item in scene_items)
            {
                item.Velocity += item.Acceleration;
                item.Position += item.Velocity * update_time;
            }
        }

        private void Main_Form_KeyDown(object sender, KeyEventArgs e)
        {
            const double camera_pan_dampener = 0.001;
            const double camera_tilt_dampener = 0.000002;

            switch (e.KeyCode)
            {
                case Keys.W:
                    // Pan forward
                    scene.Render_Camera.Translate(scene.Render_Camera.World_Direction * camera_pan_dampener * update_time);
                    break;
                case Keys.A:
                    // Pan left
                    scene.Render_Camera.Translate(scene.Render_Camera.World_Direction_Right * -camera_pan_dampener * update_time);
                    break;
                case Keys.D:
                    // Pan right
                    scene.Render_Camera.Translate(scene.Render_Camera.World_Direction_Right * camera_pan_dampener * update_time);
                    break;
                case Keys.S:
                    // Pan back
                    scene.Render_Camera.Translate(scene.Render_Camera.World_Direction * -camera_pan_dampener * update_time);
                    break;
                case Keys.Q:
                    // Pan up
                    scene.Render_Camera.Translate(scene.Render_Camera.World_Direction_Up * camera_pan_dampener * update_time);
                    break;
                case Keys.E:
                    // Pan down
                    scene.Render_Camera.Translate(scene.Render_Camera.World_Direction_Up * -camera_pan_dampener * update_time);
                    break;
                case Keys.I:
                    // Rotate up
                    Matrix4x4 transformation_up = Transform.Rotate(scene.Render_Camera.World_Direction_Right, camera_tilt_dampener * update_time);
                    scene.Render_Camera.Set_Camera_Direction_1(new Vector3D(transformation_up * new Vector4D(scene.Render_Camera.World_Direction)), new Vector3D(transformation_up * new Vector4D(scene.Render_Camera.World_Direction_Up)));
                    break;
                case Keys.J:
                    // Rotate left
                    Matrix4x4 transformation_left = Transform.Rotate(scene.Render_Camera.World_Direction_Up, camera_tilt_dampener * update_time);
                    scene.Render_Camera.Set_Camera_Direction_3(new Vector3D(transformation_left * new Vector4D(scene.Render_Camera.World_Direction_Right)), new Vector3D(transformation_left * new Vector4D(scene.Render_Camera.World_Direction)));
                    break;
                case Keys.L:
                    // Rotate right
                    Matrix4x4 transformation_right = Transform.Rotate(scene.Render_Camera.World_Direction_Up, -camera_tilt_dampener * update_time);
                    scene.Render_Camera.Set_Camera_Direction_3(new Vector3D(transformation_right * new Vector4D(scene.Render_Camera.World_Direction_Right)), new Vector3D(transformation_right * new Vector4D(scene.Render_Camera.World_Direction)));
                    break;
                case Keys.K:
                    // Rotate down
                    Matrix4x4 transformation_down = Transform.Rotate(scene.Render_Camera.World_Direction_Right, -camera_tilt_dampener * update_time);
                    scene.Render_Camera.Set_Camera_Direction_1(new Vector3D(transformation_down * new Vector4D(scene.Render_Camera.World_Direction)), new Vector3D(transformation_down * new Vector4D(scene.Render_Camera.World_Direction_Up)));
                    break;
                case Keys.U:
                    // Roll left
                    Matrix4x4 transformation_roll_left = Transform.Rotate(scene.Render_Camera.World_Direction, -camera_tilt_dampener * update_time);
                    scene.Render_Camera.Set_Camera_Direction_2(new Vector3D(transformation_roll_left * new Vector4D(scene.Render_Camera.World_Direction_Up)), new Vector3D(transformation_roll_left * new Vector4D(scene.Render_Camera.World_Direction_Right)));
                    break;
                case Keys.O:
                    // Roll right
                    Matrix4x4 transformation_roll_right = Transform.Rotate(scene.Render_Camera.World_Direction, camera_tilt_dampener * update_time);
                    scene.Render_Camera.Set_Camera_Direction_2(new Vector3D(transformation_roll_right * new Vector4D(scene.Render_Camera.World_Direction_Up)), new Vector3D(transformation_roll_right * new Vector4D(scene.Render_Camera.World_Direction_Right)));
                    break;
            }
        }
    }
}