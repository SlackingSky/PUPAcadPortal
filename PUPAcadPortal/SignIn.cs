using System.Drawing.Drawing2D;

namespace PUPAcadPortal
{
    public partial class SignIn : Form
    {
        public SignIn()
        {
            InitializeComponent();
            this.SetClientSizeCore(1513, 823);
        }

        private void btnShowPass_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentPortal studentPortal = new StudentPortal();
            studentPortal.Show();
        }
    }
}
