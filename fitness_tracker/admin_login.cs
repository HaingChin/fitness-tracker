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
    public partial class admin_login : Form
    {
        private readonly AdminService adminService;
        private int failedAttempts = 0;
        private const int MaxAttempts = 3;
        public admin_login()
        {
            InitializeComponent();
            adminService = new AdminService();
        }

        private void admin_login_Load(object sender, EventArgs e)
        {

        }

        private void link_login_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new admin_register().Show();
            Hide();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_register_login_Click(object sender, EventArgs e)
        {
            // Lock account after too many failed attempts
            if (failedAttempts >= MaxAttempts)
            {
                MessageBox.Show("Too many failed login attempts. Please try again later.", "Account Locked", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string adminName = txt_admin_name.Text.Trim();
            string password = txt_password.Text;

            // Ensure both fields are filled
            if (string.IsNullOrWhiteSpace(adminName) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Enter both admin name and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate admin credentials
            if (!adminService.ValidateAdmin(adminName, password))
            {
                failedAttempts++;
                MessageBox.Show("Invalid admin name or password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Reset failed attempts on successful login
            failedAttempts = 0;
            MessageBox.Show("Login Successful!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);


            string loggedInAdminId = adminService.GetAdminIdByUsername(adminName); // Fetch Admin ID
            string loggedInAdminName = adminName;
            admin_mainform dashboard = new admin_mainform(loggedInAdminId, loggedInAdminName); // Pass Admin ID
            dashboard.Show();
            this.Hide();
        }

        private void check_pw_CheckedChanged(object sender, EventArgs e)
        {
            txt_password.PasswordChar = check_pw.Checked ? '\0' : '•';
        }
    }
}
