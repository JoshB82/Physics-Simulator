using _3D_Engine;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Physics_Simulator
{
    public partial class Main_Form : Form
    {
        private Scene scene;
        private bool running = true;

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

            // Start loop
            Thread thread = new Thread(Loop);
            thread.Start();
            thread.IsBackground = true;
        }

        private void Loop()
        {
            const int max_frames_per_second = 60;
            const int max_updates_per_second = 60;
            const long frame_minimum_time = 1000 / max_frames_per_second;
            const long update_minimum_time = 1000 / max_updates_per_second;

            int no_frames = 0, no_updates = 0, timer = 1;

            long now_time, delta_time;
            long frame_time = 0, update_time = 0;
            
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
                    Update_Position(update_time);
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

        private void Update_Position(long time)
        {

        }
    }
}