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
    public partial class login_form : Form
    {

        private readonly UserService userService;
        private int failedAttempts = 0;
        private const int MaxAttempts = 3;

        public login_form()
        {
            InitializeComponent();
            userService = new UserService();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            // Lock account after too many failed attempts
            if (failedAttempts >= MaxAttempts)
            {
                MessageBox.Show("Too many failed login attempts.", "Account Locked", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string username = txt_username.Text.Trim();
            string passwords = txt_pw.Text;

            // Ensure both username and password are entered
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(passwords))
            {
                MessageBox.Show("Enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate user credentials
            if (!userService.ValidateUser(username, passwords))
            {
                failedAttempts++;
                MessageBox.Show("Invalid username or password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            failedAttempts = 0; // Reset failed attempts on success
            MessageBox.Show("Login Successful!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);
            new main_form().Show(); // Open the main form
            Hide(); // Hide the login form
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_register_Click(object sender, EventArgs e)
        {
            register_form RegisterForm = new register_form();
            RegisterForm.Show();
            this.Hide();
        }

        private void check_pw_CheckedChanged(object sender, EventArgs e)
        {
            txt_pw.PasswordChar = check_pw.Checked ? '\0' : '•';
        }
    }
}
