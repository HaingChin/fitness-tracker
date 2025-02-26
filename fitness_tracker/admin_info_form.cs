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
    public partial class admin_info_form : UserControl
    {
        private readonly AdminService adminService;
        private string loggedInAdminId; // Store the logged-in admin's ID


        public admin_info_form()
        {
            InitializeComponent();
            adminService = new AdminService();
        }

        public admin_info_form(string adminId)
        {
            InitializeComponent();
            adminService = new AdminService();
            loggedInAdminId = adminId ?? throw new ArgumentNullException(nameof(adminId)); // Prevent null issues
            LoadAdminInfo();
        }


        // Load admin details into the form
        private void admin_info_form_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(loggedInAdminId))
            {
                MessageBox.Show("Admin ID is missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadAdminInfo();
        }

        // Load admin details into the form
        private void LoadAdminInfo()
        {
            var admin = adminService.GetAdminById(loggedInAdminId);
            if (admin != null)
            {
                txt_admin_id.Text = admin.AdminId;
                txt_admin_name.Text = admin.AdminName;
                txt_email.Text = admin.Email;
                txt_position.Text = admin.Position;
                txt_phone.Text = admin.Phone;
                txt_passport.Text = admin.Passport;
            }
            else
            {
                MessageBox.Show("Admin data not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Save updated admin information
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(loggedInAdminId))
            {
                MessageBox.Show("Admin ID is missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string name = txt_admin_name.Text.Trim();
            string email = txt_email.Text.Trim();
            string position = txt_position.Text.Trim();
            string phone = txt_phone.Text.Trim();
            string passport = txt_passport.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(position) || string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(passport))
            {
                MessageBox.Show("Please fill all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                adminService.UpdateAdminInfo(loggedInAdminId, name, email, position, phone, passport);
                MessageBox.Show("Admin details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating admin info: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Change password
        private void btn_change_password_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(loggedInAdminId))
            {
                MessageBox.Show("Admin ID is missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string oldPassword = txt_old_password.Text;
            string newPassword = txt_new_password.Text;
            string confirmPassword = txt_confirm_password.Text;

            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please enter all password fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("New passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                bool isUpdated = adminService.ChangePassword(loggedInAdminId, oldPassword, newPassword);
                if (isUpdated)
                {
                    MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Old password is incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing password: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void check_pw_CheckedChanged(object sender, EventArgs e)
        {
            txt_new_password.PasswordChar = check_pw.Checked ? '\0' : '•';
            txt_old_password.PasswordChar = check_pw.Checked ? '\0' : '•';
            txt_confirm_password.PasswordChar = check_pw.Checked ? '\0' : '•';
        }
    }    
}
