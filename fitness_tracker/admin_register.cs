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
    public partial class admin_register : Form
    {
        private readonly AdminService adminService;
        public admin_register()
        {
            InitializeComponent();
            adminService = new AdminService();
        }

        private void link_login_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new admin_login().Show();
            Hide();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_register_login_Click(object sender, EventArgs e)
        {
            string adminId = txt_admin_id.Text;
            string adminName = txt_admin_name.Text.Trim();
            string email = txt_email.Text.Trim();
            string position = txt_position.Text.Trim();
            string phone = txt_phone.Text.Trim();
            string passport = txt_passport.Text.Trim();
            string password = txt_password.Text;
            string confirmPassword = txt_confirm_password.Text;


            // Ensure all fields are filled
            if (string.IsNullOrWhiteSpace(adminName) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(position) || string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(passport) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill all blank fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_admin_name.Focus();
                return;
            }

            // Validate input formats

            // Validate admin name (only alphanumeric & spaces)
            if (!InputValidator.IsValidUsername(adminName))
            {
                MessageBox.Show("Admin Name must be alphanumeric!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_admin_name.Focus();
                return;
            }

            // Validate email
            if (!InputValidator.IsValidEmail(email))
            {
                MessageBox.Show("Invalid email format!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_email.Focus();
                return;
            }

            // Validate phone number (7 to 15 digits)
            if (!InputValidator.IsValidPhoneNumber(phone))
            {
                MessageBox.Show("Invalid phone number!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_phone.Focus();
                return;
            }

            // Validate password
            if (!InputValidator.IsValidPassword(password))
            {
                MessageBox.Show("Password must be at least 12 characters with 1 uppercase & 1 lowercase letter!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_password.Focus();
                return;
            }

            // Ensure passwords match
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_password.Focus();
                return;
            }

            // Check if email is already taken
            if (adminService.IsEmailExists(email))
            {
                MessageBox.Show("This email is already registered!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_email.Focus();
                return;
            }

            // Confirm Registration
            try
            {
                string message = $"Registration Successful!\n\n" +
                                 $"Admin ID: {adminId}\n" +
                                 $"Name: {adminName}\n" +
                                 $"Email: {email}\n" +
                                 $"Position: {position}\n" +
                                 $"Phone: {phone}\n" +
                                 $"Passport: {passport}\n" +
                                 "Confirm Registration?";

                DialogResult result = MessageBox.Show(message, "Confirm Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("Registration Confirmed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    adminService.Register(adminName, email, position, phone, passport, password);
                    ClearFields(); // Clear input fields

                    // Generate a new Admin ID for the next registration
                    txt_admin_id.Text = adminService.GenerateNewAdminId();
                }
                else
                {
                    MessageBox.Show("Registration Not Confirmed!", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        // Clear input fields after registration
        private void ClearFields()
        {
            txt_admin_name.Clear();
            txt_email.Clear();
            txt_position.Clear();
            txt_phone.Clear();
            txt_passport.Clear();
            txt_password.Clear();
            txt_confirm_password.Clear();
        }

        // Form Load - Show Admin ID
        private void admin_register_Load(object sender, EventArgs e)
        {
            txt_admin_id.Text = adminService.GenerateNewAdminId();
            txt_admin_id.ReadOnly = true; // Ensure Admin ID is not editable
        }

        private void check_pw_CheckedChanged(object sender, EventArgs e)
        {
            txt_password.PasswordChar = check_pw.Checked ? '\0' : '•';
            txt_confirm_password.PasswordChar = check_pw.Checked ? '\0' : '•';
        }
    }
}
