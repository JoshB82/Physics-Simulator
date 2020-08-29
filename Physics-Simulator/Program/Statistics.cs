using System.Windows.Forms;

namespace Physics_Simulator
{
    public partial class Statistics : Form
    {
        public Statistics()
        {
            InitializeComponent();
        }

        private void Statistics_FormClosing(object sender, FormClosingEventArgs e) => Main_Form.statistics_form_show = false;
    }
}