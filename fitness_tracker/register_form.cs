using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace fitness_tracker
{
    public partial class register_form : Form
    {
        private readonly UserService userService;

        public register_form()
        {
            InitializeComponent();
            userService = new UserService();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_register_login_Click(object sender, EventArgs e)
        {
            new login_form().Show();
            Hide();
        }

        private void btn_register_Click(object sender, EventArgs e)
        {
            int newUserId = userService.GenerateNewUserId();
            string username = txt_username.Text.Trim();
            string email = txt_email.Text.Trim();
            string passwords = txt_pw.Text;
            string confirmPassword = txt_cpw.Text;


            // Ensure all fields are filled
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(passwords) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill all blank fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_username.Focus();
                return;
            }

            // Validate input formats

            // Validate username (only alphanumeric)
            if (!Validator.IsValidUsername(username))
            {
                MessageBox.Show("Username must be alphanumeric!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_username.Focus();
                return;
            }

            // Validate email
            if (!Validator.IsValidEmail(email))
            {
                MessageBox.Show("Invalid email format!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_email.Focus();
                return;
            }

            // Validate password
            if (!Validator.IsValidPassword(passwords))
            {
                MessageBox.Show("Password must be at least 12 characters with 1 uppercase & 1 lowercase letter!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_pw.Focus();
                return;
            }


            // Ensure passwords match
            if (passwords != confirmPassword)
            {
                MessageBox.Show("Passwords do not match!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_pw.Focus();
                return;
            }

            // Check if username or email is already taken
            if (userService.IsUsernameExists(username))
            {
                MessageBox.Show("This username is already taken", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_username.Focus();
                return;
            }

            if (userService.IsEmailExists(email))
            {
                MessageBox.Show("This email is already registered!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_email.Focus();
                return;
            }

            // User comfirm  information and Register or not
            try
            {
                string message = $"Registration Successful!\n\n" +
                                     $"User ID: {newUserId:000000}\n" +
                                     $"Username: {username}\n" +
                                     $"Email: {email}\n" +
                                     $"Password: {passwords}\n" +
                                     "Confirm Registration?";


                DialogResult result = MessageBox.Show(message, "Confirm Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("Registration Confirmed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    userService.Register(username, email, passwords);
                                
                }
                else
                {
                    MessageBox.Show("Registration Not Confirmed!", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                ClearFields(); // Clear input fields
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ClearFields()
        {
            txt_username.Clear();
            txt_email.Clear();
            txt_pw.Clear();
            txt_cpw.Clear();
        }

        public static class Validator
        {
            // Username must be alphanumeric
            public static bool IsValidUsername(string username) => Regex.IsMatch(username, "^[a-zA-Z0-9]+$");

            // Basic email validation pattern
            public static bool IsValidEmail(string email) => Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            // Password must be at least 12 characters with uppercase and lowercase letters
            public static bool IsValidPassword(string password) => password.Length >= 12 && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[a-z]");
        }


        private void check_pw_CheckedChanged(object sender, EventArgs e)
        {
            txt_pw.PasswordChar = check_pw.Checked ? '\0' : '•';
            txt_cpw.PasswordChar = check_pw.Checked ? '\0' : '•';
        }

        private void register_form_Load(object sender, EventArgs e)
        {

        }
    }
}
