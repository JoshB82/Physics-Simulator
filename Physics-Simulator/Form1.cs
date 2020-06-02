using _3D_Engine;
using System.Windows.Forms;

namespace Physics_Simulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Cube cube = new Cube(Vector3D.Zero, Vector3D.Unit_Negative_Z, Vector3D.Unit_X, 100);
        }

    }
}
