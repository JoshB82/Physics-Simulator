﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using _3D_Engine;

namespace Physics_Simulator
{
    public partial class Main_Form : Form
    {
        private Statistics statistics_form = new Statistics();
        public static bool statistics_form_show = false;

        private Scene scene;
        private List<Shape> shapes = new List<Shape>();

        private Vector2D mouse_centre;

        private bool use_keyboard_only = true;
        private bool use_WASDQE_and_mouse = false;

        private bool running = true;
        private long update_time;

        public Main_Form()
        {
            // Create main form
            InitializeComponent();

            // Create scene
            scene = new Scene(Canvas_Box, Canvas_Box.Width, Canvas_Box.Height); // panel?
            
            // Set settings
            Settings.Mesh_Debug_Output_Verbosity = Verbosity.All;

            // Create origin and axes
            scene.Create_Origin();
            scene.Create_Axes();

            // Create a camera
            double camera_width = Canvas_Box.Width / 10, camera_height = Canvas_Box.Height / 10;

            Perspective_Camera camera = new Perspective_Camera(new Vector3D(0, 0, -100), scene.Meshes[0], Vector3D.Unit_Y, camera_width, camera_height, 10, 750);
            scene.Add(camera);
            scene.Render_Camera = camera;

            // Add some meshes
            Cube cube_mesh = new Cube(new Vector3D(100, 100, 300), Vector3D.Unit_Z, Vector3D.Unit_Y, 100);
            scene.Add(cube_mesh);
            Shape cube = new Shape(cube_mesh, Vector3D.Zero, Constants.Grav_Acc_Vector);
            shapes.Add(cube);

            Square floor_mesh = new Square(Vector3D.Zero, Vector3D.Unit_Z, Vector3D.Unit_Y, 600);
            scene.Add(floor_mesh);
            floor_mesh.Face_Colour = Color.Brown;

            // Add some lights
            Distant_Light distant_light = new Distant_Light(new Vector3D(0, 300, 0), Vector3D.Unit_Z, Vector3D.Unit_Y, 1000);
            scene.Add(distant_light);

            distant_light.Show_Icon = true;

            // Call some methods
            /*
            scene.Generate_MWV_Matrices();
            scene.Generate_Shadow_Map(distant_light);
            distant_light.Export_Shadow_Map();
            */

            // Change some properties
            distant_light.Colour = Color.Goldenrod;

            /*
            string teapot_file_path = "C:\\Users\\jbrya\\source\\repos\\3D-Engine\\3D-Engine\\External\\Models\\teapot.obj";
            Custom teapot_mesh = new Custom(new Vector3D(500, 10, 0), Vector3D.Unit_Z, Vector3D.Unit_Y, teapot_file_path);

            teapot_mesh.Scale(50);
            teapot_mesh.Face_Colour = Color.Red;

            scene.Add(teapot_mesh);
            */
            /*
            string seahorse_file_path = "C:\\Users\\jbrya\\source\\repos\\3D-Engine\\3D-Engine\\External\\Models\\seahorse.obj";
            Custom seahorse_mesh = new Custom(new Vector3D(1000, 10, 0), Vector3D.Unit_Z, Vector3D.Unit_Y, seahorse_file_path);

            seahorse_mesh.Scale(50);
            seahorse_mesh.Face_Colour = Color.Gray;

            scene.Add(seahorse_mesh);
            */
            /*
            Cone cone_mesh = new Cone(new Vector3D(1000, 10, 0), Vector3D.Unit_Z, Vector3D.Unit_Y, 50, 25, 50);
            cone_mesh.Face_Colour = Color.Aquamarine;
            scene.Add(cone_mesh);

            Cylinder cylinder_mesh = new Cylinder(new Vector3D(1500, 10, 0), Vector3D.Unit_Z, Vector3D.Unit_Y, 100, 50, 50);
            scene.Add(cylinder_mesh);

            Ring ring_mesh = new Ring(new Vector3D(0, 500, 0), Vector3D.Unit_Z, Vector3D.Unit_Y, 100, 200, 50);
            scene.Add(ring_mesh);
            ring_mesh.Face_Colour = Color.Orange;

            Vector3D[] texture_vertices = Texture.Generate_Vertices("Square");
            Texture smiley = new Texture(Properties.Resources.smiley, texture_vertices);
            Square square_mesh = new Square(new Vector3D(500, 0, 500), Vector3D.Unit_Y, Vector3D.Unit_Negative_Z, 100, smiley);
            scene.Add(square_mesh);
            */

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

                        scene.Render_Camera.Rotate_Right(displacement.X * rotation_dampener);//use update time?
                        scene.Render_Camera.Rotate_Down(displacement.Y * rotation_dampener);//move entire keyboard here?
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
                    Invoke((MethodInvoker)delegate { Text = $"Physics Simulator - FPS: {no_frames}, UPS: {no_updates}"; }); // ?
                    no_frames = 0; no_updates = 0;
                    timer += 1;
                }
            }
        }

        private void Update_Position()
        {
            foreach (Shape shape in shapes)
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
            if ((e.KeyCode == Keys.Escape && use_WASDQE_and_mouse) || (e.KeyCode == Keys.Z && use_WASDQE_and_mouse))
            {
                (use_keyboard_only, use_WASDQE_and_mouse) = (true, false);
                Cursor.Show();
            }
            
            if (e.KeyCode == Keys.X && use_keyboard_only)
            {
                (use_keyboard_only, use_WASDQE_and_mouse) = (false, true);
                Cursor.Hide();
            }

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

            if (statistics_form_show) Update_Statistics();
        }

        private void Main_Form_Move(object sender, System.EventArgs e) => mouse_centre = new Vector2D(Left + Canvas_Box.Left + Canvas_Box.Width / 2, Top + Canvas_Box.Top + Canvas_Box.Height / 2);

        private void keyboardMenuItem_Click(object sender, System.EventArgs e) => (use_keyboard_only, use_WASDQE_and_mouse) = (true, false);

        private void mouseMenuItem_Click(object sender, System.EventArgs e) => (use_keyboard_only, use_WASDQE_and_mouse) = (false, true);

        private void aboutToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            string about_text = "Physics Simulator, 3D-Engine | Programmed by Josh Bryant.";
            MessageBox.Show(about_text, "About");
        }

        private void Main_Form_Resize(object sender, System.EventArgs e)
        {
            scene.Width = Canvas_Box.Width;
            scene.Height = Canvas_Box.Height;
            scene.Render_Camera.Width = Canvas_Box.Width / 10;
            scene.Render_Camera.Height = Canvas_Box.Height / 10;
        }

        private void statisticsMenuItem_Click(object sender, System.EventArgs e)
        {
            statistics_form.Show();
            statistics_form_show = true;
            Update_Statistics();
        }

        private void Update_Statistics()
        {
            statistics_form.listView.Items.Clear();

            foreach (Camera camera in scene.Cameras)
            {
                string[] camera_data = new string[]
                {
                    camera.ID.ToString(),
                    camera.GetType().Name,
                    $"({Math.Round(camera.World_Origin.X, 2)}, {Math.Round(camera.World_Origin.Y, 2)}, {Math.Round(camera.World_Origin.Z, 2)})",
                    $"({Math.Round(camera.World_Direction_Forward.X, 2)}, {Math.Round(camera.World_Direction_Forward.Y, 2)}, {Math.Round(camera.World_Direction_Forward.Z, 2)})",
                    $"({Math.Round(camera.World_Direction_Up.X, 2)}, {Math.Round(camera.World_Direction_Up.Y, 2)}, {Math.Round(camera.World_Direction_Up.Z, 2)})",
                    $"({Math.Round(camera.World_Direction_Right.X, 2)}, {Math.Round(camera.World_Direction_Right.Y, 2)}, {Math.Round(camera.World_Direction_Right.Z, 2)})"
                };

                ListViewItem row = new ListViewItem(camera_data);
                statistics_form.listView.Items.Add(row);
            }
            foreach (Light light in scene.Lights)
            {
                string[] light_data = new string[]
                {
                    light.ID.ToString(),
                    light.GetType().Name,
                    light.World_Origin.ToString(),
                    light.World_Direction_Forward.ToString(),
                    light.World_Direction_Up.ToString(),
                    light.World_Direction_Right.ToString()
                };

                ListViewItem row = new ListViewItem(light_data);
                statistics_form.listView.Items.Add(row);
            }
            foreach (Mesh mesh in scene.Meshes)
            {
                string[] mesh_data = new string[]
                {
                    mesh.ID.ToString(),
                    mesh.GetType().Name,
                    mesh.World_Origin.ToString(),
                    mesh.World_Direction_Forward.ToString(),
                    mesh.World_Direction_Up.ToString(),
                    mesh.World_Direction_Right.ToString()
                };

                ListViewItem row = new ListViewItem(mesh_data);
                statistics_form.listView.Items.Add(row);
            }
        }
    }
}