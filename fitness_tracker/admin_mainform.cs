using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fitness_tracker
{
    public partial class admin_mainform : Form
    {
        private string loggedInAdminId; // Store logged-in Admin ID
        private string loggedInAdminName;

        public admin_mainform(string adminId, string adminName)
        {
            InitializeComponent();
            loggedInAdminId = adminId; // Store Admin ID
            loggedInAdminName = adminName;
            lbl_admin_name.Text = $"Welcome, {loggedInAdminName}!"; // Display Admin Name
        }

        private void LoadUserControl(UserControl userControl)
        {
            panel_container.Controls.Clear(); // Remove previous controls
            userControl.Dock = DockStyle.Fill; // Fill the panel
            panel_container.Controls.Add(userControl); // Add the new control
        }

        public void LoadAdminInfo()
        {
            LoadUserControl(new admin_info_form(loggedInAdminId));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadUserControl(new admin_activity());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadAdminInfo(); // Load admin info with stored Admin ID
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
