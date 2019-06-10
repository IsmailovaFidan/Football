
using FootballApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballApp
{
    public partial class Home_Page : Form
    {
        FootballEntities db = new FootballEntities();
        public Home_Page()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string Email = txtEmail.Text;
            string Password = txtPassword.Text;
            bool allempty = Extention.isNotEmpty(new string[] {
                Email,Password
            }, string.Empty);
            if (allempty) {
                string adminEmail = db.Admins.First(a => a.Id == 1).Email;
                string adminPassword = db.Admins.First(a => a.Id == 1).Password;
                if (Email == adminEmail && Password == adminPassword)
                {
                    if (chkKeep.Checked)
                    {
                        Properties.Settings.Default.Email = Email;
                        Properties.Settings.Default.Password = Password;
                        Properties.Settings.Default.Remember = true;


                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Properties.Settings.Default.Email = "";
                        Properties.Settings.Default.Password = "";
                        Properties.Settings.Default.Remember = false;

                        Properties.Settings.Default.Save();

                    }
                    Dashboard dash = new Dashboard();
                    dash.ShowDialog();
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Email or Password doesn't correct";

                }
            }

        }

       

        private void Button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Home_Page_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Remember)
            {
                txtEmail.Text = Properties.Settings.Default.Email;
                txtPassword.Text = Properties.Settings.Default.Password;
                chkKeep.Checked = true;
            }
        }
    }
}
