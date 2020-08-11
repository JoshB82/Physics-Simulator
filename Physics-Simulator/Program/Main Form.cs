using _3D_Engine;
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
        private List<Shape> scene_shapes = new List<Shape>();

        private Vector2D mouse_centre;

        private bool use_keyboard_only = true;
        private bool use_WASDQE_and_mouse = false;

        private bool running = true;
        private long update_time;

        public Main_Form()
        {
            // Create form
            InitializeComponent();

            // Create scene
            scene = new Scene(Canvas_Box, Canvas_Box.Width, Canvas_Box.Height);
            Settings.Mesh_Debug_Output_Verbosity = Verbosity.All;

            // Create origin and axes
            scene.Create_Origin();
            scene.Create_Axes();

            // Create camera
            double camera_width = Canvas_Box.Width / 10;
            double camera_height = Canvas_Box.Height / 10;

            Perspective_Camera camera = new Perspective_Camera(new Vector3D(0, 0, -100), scene.Meshes[0], Vector3D.Unit_Y, camera_width, camera_height, 10, 750);
            scene.Render_Camera = camera;

            // Add some shapes
            Vector3D[] texture_vertices = Texture.Generate_Vertices("Plane");
            Texture smiley = new Texture(Properties.Resources.smiley, texture_vertices);

            Cube cube_mesh = new Cube(new Vector3D(10, 10, 10), Vector3D.Unit_Z, Vector3D.Unit_Y, 250); //{ Display_Direction_Arrows = true };
            scene.Add(cube_mesh);
            Shape cube = new Shape(cube_mesh, Vector3D.Zero, new Vector3D(0, Constants.Grav_Acc, 0));
            scene_shapes.Add(cube);

            //Custom teapot_mesh = new Custom(new Vector3D(150, 150, 150), Vector3D.Unit_Z, Vector3D.Unit_Y, "C:\\Users\\jbrya\\source\repos\\3D-Engine\\3D-Engine\\External\\Models\\teapot.obj");
            //scene.Add(teapot_mesh);

            //cube_mesh.Display_Direction_Arrows = true;

            /*
            Ring ring_mesh = new Ring(Vector3D.One * 100, Vector3D.Unit_Z, Vector3D.Unit_Y, 100, 200, 50);
            scene.Add(ring_mesh);
            */

            //Arrow arrow_mesh = new Arrow(Vector3D.One * 100, Vector3D.One * 150, 50, 30, 75, 25, false);
            //scene.Add(arrow_mesh);

            /*
            
            Point_Light point_light = new Point_Light(new Vector3D(0, 200, 0), Vector3D.Unit_Z, Vector3D.Unit_Y, 100) { Colour = Color.Green };
            scene.Add(point_light);

            Square square_mesh = new Square(new Vector3D(100, 200, 100), Vector3D.Unit_X, Vector3D.Unit_Z, 100) { Face_Colour = Color.Red };
            scene.Add(square_mesh);
            Shape square = new Shape(square_mesh, Vector3D.Zero, new Vector3D(0, Constants.Grav_Acc, 0));
            scene_shapes.Add(square);

            Circle test_circle_mesh = new Circle(new Vector3D(200, 100, 100), Vector3D.Unit_X, Vector3D.Unit_Y, 50, 30) { Face_Colour = Color.Purple, Display_Direction_Arrows = true };
            scene.Add(test_circle_mesh);
            Shape test_circle = new Shape(test_circle_mesh, Vector3D.Zero, new Vector3D(0, Constants.Grav_Acc, 0));
            scene_shapes.Add(test_circle);

            */

            //Pyramid test_pyramid_mesh = new Pyramid(new Vector3D(100, 100, 100), Vector3D.Unit_X, Vector3D.Unit_Y, 50, 20, 30);
            //Shape test_pyramid = new Shape(test_pyramid_mesh);
            //scene.Add(test_pyramid);

            // Start loop
            Thread thread = new Thread(Loop) { IsBackground = true };
            thread.Start();
        }

        private void Loop() //credit?
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
                    if (use_WASDQE_and_mouse)
                    {
                        const double rotation_dampener = 0.005;

                        Vector2D mouse_position = new Vector2D(Cursor.Position.X, Cursor.Position.Y);
                        Vector2D displacement = mouse_position - mouse_centre;

                        Cursor.Position = new Point((int)mouse_centre.X, (int)mouse_centre.Y);//?

                        //MessageBox.Show(Canvas_Box.Right.ToString());
                        scene.Render_Camera.Rotate_Right(displacement.X * rotation_dampener);
                        scene.Render_Camera.Rotate_Down(displacement.Y * rotation_dampener);
                    }

                    scene.Render();
                    no_frames++;
                    frame_time -= frame_minimum_time;
                }

                if (update_time >= update_minimum_time)
                {
                    //Update_Position(); uncomment
                    no_updates++;
                    update_time -= update_minimum_time;
                }

                if (now_time >= 1000 * timer)
                {
                    Invoke((MethodInvoker)delegate { Text = $"Physics Simulator - FPS: {no_frames}, UPS: {no_updates}" + ", Camera Position: " + scene.Render_Camera.World_Origin; }); // ?
                    no_frames = 0; no_updates = 0;
                    timer += 1;
                }
            }
        }

        private void Canvas_Box_Paint(object sender, PaintEventArgs e) => e.Graphics.DrawImageUnscaled(scene.Canvas, Point.Empty);

        private void Update_Position()
        {
            foreach (Shape shape in scene_shapes)
            {
                shape.Velocity += shape.Acceleration; // ?
                shape.Position += shape.Velocity * update_time; // ?

                Check_For_Collision(shape);
            }
        }

        private void Check_For_Collision(Shape shape)
        {
            if (shape.Position.Y < 0)
            {
                shape.Position = new Vector3D(-shape.Position.X, -shape.Position.Y, -shape.Position.Z); // ?
                shape.Velocity = new Vector3D(-shape.Velocity.X, -shape.Velocity.Y, -shape.Velocity.Z); // ?
            }
        }

        private void Main_Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && use_WASDQE_and_mouse) (use_keyboard_only, use_WASDQE_and_mouse) = (true, false);
            if (e.KeyCode == Keys.Z && use_WASDQE_and_mouse) (use_keyboard_only, use_WASDQE_and_mouse) = (true, false);
            if (e.KeyCode == Keys.X && use_keyboard_only) (use_keyboard_only, use_WASDQE_and_mouse) = (false, true);

            const double camera_pan_dampener = 0.0008;
            const double camera_tilt_dampener = 0.000001;

            switch (e.KeyCode)
            {
                case Keys.W:
                    // Pan forward
                    scene.Render_Camera.Pan_Forward(camera_pan_dampener * update_time);
                    break;
                case Keys.A:
                    // Pan left
                    scene.Render_Camera.Pan_Left(camera_pan_dampener * update_time);
                    break;
                case Keys.D:
                    // Pan right
                    scene.Render_Camera.Pan_Right(camera_pan_dampener * update_time);
                    break;
                case Keys.S:
                    // Pan back
                    scene.Render_Camera.Pan_Back(camera_pan_dampener * update_time);
                    break;
                case Keys.Q:
                    // Pan up
                    scene.Render_Camera.Pan_Up(camera_pan_dampener * update_time);
                    break;
                case Keys.E:
                    // Pan down
                    scene.Render_Camera.Pan_Down(camera_pan_dampener * update_time);
                    break;
            }

            if (use_keyboard_only)
            {
                switch (e.KeyCode)
                {
                    case Keys.I:
                        // Rotate up
                        scene.Render_Camera.Rotate_Up(camera_tilt_dampener * update_time);
                        break;
                    case Keys.J:
                        // Rotate left
                        scene.Render_Camera.Rotate_Left(camera_tilt_dampener * update_time);
                        break;
                    case Keys.L:
                        // Rotate right
                        scene.Render_Camera.Rotate_Right(camera_tilt_dampener * update_time);
                        break;
                    case Keys.K:
                        // Rotate down
                        scene.Render_Camera.Rotate_Down(camera_tilt_dampener * update_time);
                        break;
                    case Keys.U:
                        // Roll left
                        scene.Render_Camera.Roll_Left(camera_tilt_dampener * update_time);
                        break;
                    case Keys.O:
                        // Roll right
                        scene.Render_Camera.Roll_Right(camera_tilt_dampener * update_time);
                        break;
                }
            }
        }

        private void Main_Form_Move(object sender, System.EventArgs e) => mouse_centre = new Vector2D(Left + Canvas_Box.Left + Canvas_Box.Width / 2, Top + Canvas_Box.Top + Canvas_Box.Height / 2);
        //810,640
        private void Main_Form_MouseHover(object sender, System.EventArgs e) { }//Cursor.Hide();

        private void keyboardMenuItem_Click(object sender, System.EventArgs e) => (use_keyboard_only, use_WASDQE_and_mouse) = (true, false);

        private void mouseMenuItem_Click(object sender, System.EventArgs e) => (use_keyboard_only, use_WASDQE_and_mouse) = (false, true);

        private void aboutToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            string about_text = "Physics Simulator, 3D-Engine | Programmed by Josh Bryant.";
            MessageBox.Show(about_text,"About");
        }

        private void Main_Form_Resize(object sender, System.EventArgs e)
        {
            scene.Width = Canvas_Box.Width;
            scene.Height = Canvas_Box.Height;
            scene.Render_Camera.Width = Canvas_Box.Width / 10;
            scene.Render_Camera.Height = Canvas_Box.Height / 10;
        }
    }
}