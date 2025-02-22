using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace fitness_tracker
{
    public class AdminService
    {
        private readonly fitness_treacker_dbTableAdapters.admins_tblTableAdapter dbAdapter;

        public AdminService()
        {
            dbAdapter = new fitness_treacker_dbTableAdapters.admins_tblTableAdapter();
        }

        // Validate email format
        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        // Validate phone number (only digits, 7 to 15 characters)
        public static bool IsValidPhoneNumber(string phone)
        {
            return Regex.IsMatch(phone, @"^\d{7,15}$");
        }

        // Check if admin email already exists
        public bool IsAdminEmailExists(string email)
        {
            object result = dbAdapter.CheckAdminEmailExists(email);
            return ConvertToInt(result) > 0;
        }

        // Generate a new Admin ID in format "admin-001"
        private string GenerateNewAdminId()
        {
            DataTable adminData = dbAdapter.GetData();
            if (adminData.Rows.Count > 0)
            {
                int maxId = adminData.AsEnumerable()
                                     .Select(row => int.Parse(row.Field<string>("admin_id").Replace("admin-", "")))
                                     .Max();
                return $"admin-{(maxId + 1):D3}";
            }
            return "admin-001"; // Start from admin-001
        }

        // Register a new admin
        public bool RegisterAdmin(string adminName, string email, string position, string phone, string passport, string password)
        {
            string newAdminId = GenerateNewAdminId();
            DateTime dateCreated = DateTime.Now;

            try
            {
                dbAdapter.Insert(newAdminId, adminName, email, position, phone, passport, password, dateCreated);
                return true; // Successfully registered
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Safely convert object to int (handles null values)
        private int ConvertToInt(object obj)
        {
            return obj != null ? Convert.ToInt32(obj) : 0;
        }
    }
}

