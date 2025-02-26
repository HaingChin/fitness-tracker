using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace fitness_tracker
{
    public class AdminService
    {
        private readonly fitness_treacker_dbTableAdapters.admins_tblTableAdapter adminAdapter;

        public AdminService()
        {
            adminAdapter = new fitness_treacker_dbTableAdapters.admins_tblTableAdapter();
        }

        // Check if an admin already exists by admin name or email

        public bool IsAdminExists(string admin_name)
        {
            object result = adminAdapter.CheckAdminnameExists(admin_name);
            return result != null && Convert.ToInt32(result) > 0;
        }

        public bool IsEmailExists(string email)
        {
            object result = adminAdapter.CheckEmailExist(email);
            return result != null && Convert.ToInt32(result) > 0;
        }

        // Register a new admin
        public void Register(string adminName, string email, string position, string phone, string passport, string password)
        {
            string newAdminId = GenerateNewAdminId();
            string hashedPassword = HashPassword(password);
            adminAdapter.Insert(newAdminId, adminName, email, position, phone, passport, hashedPassword, DateTime.Now);
        }

        // Validate admin login credentials
        public bool ValidateAdmin(string adminName, string password)
        {
            DataTable adminDt = adminAdapter.GetDataByAdminname(adminName);
            return adminDt.Rows.Count > 0 && adminDt.Rows[0]["password"].ToString() == HashPassword(password);
        }

        public string GetAdminIdByUsername(string adminName)
        {
            var adminData = adminAdapter.GetDataByAdminname(adminName);
            if (adminData.Rows.Count > 0)
            {
                return adminData.Rows[0]["admin_id"].ToString();
            }
            return null;
        }


        // Generate a new admin ID by incrementing the highest existing ID (admin-001, admin-002, etc.)
        public string GenerateNewAdminId()
        {
            DataTable adminDt = adminAdapter.GetData();
            if (adminDt.Rows.Count > 0)
            {
                int maxId = adminDt.AsEnumerable()
                                   .Select(row => int.Parse(row.Field<string>("admin_id").Replace("admin-", "")))
                                   .Max();
                return $"admin-{(maxId + 1):D3}";
            }
            return "admin-001";
        }

        // Securely hash the password using SHA-256
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        // Get data by AdminID
        public Admin GetAdminById(string adminId)
        {
            if (string.IsNullOrEmpty(adminId)) return null;

            var adminData = adminAdapter.GetAdminById(adminId);
            if (adminData.Rows.Count > 0)
            {
                var row = adminData.Rows[0];
                return new Admin
                {
                    AdminId = row["admin_id"].ToString(),
                    AdminName = row["admin_name"].ToString(),
                    Email = row["email"].ToString(),
                    Position = row["position"].ToString(),
                    Phone = row["phone"].ToString(),
                    Passport = row["passport"].ToString()
                };
            }
            return null;
        }

        //Update Admin information
        public void UpdateAdminInfo(string adminId, string name, string email, string position, string phone, string passport)
        {
            if (string.IsNullOrEmpty(adminId)) throw new ArgumentNullException(nameof(adminId));
            adminAdapter.UpdateAdminInfo(name, email, position, phone, passport, adminId);
        }

        public bool ChangePassword(string adminId, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(adminId)) return false;

            var adminData = adminAdapter.GetAdminById(adminId);
            if (adminData.Rows.Count > 0)
            {
                string storedPassword = adminData.Rows[0]["password"].ToString();
                string hashedOldPassword = HashPassword(oldPassword);

                if (storedPassword == hashedOldPassword)
                {
                    string hashedNewPassword = HashPassword(newPassword);
                    adminAdapter.UpdatePassword(hashedNewPassword, adminId);
                    return true;
                }
            }
            return false;
        }



    }
}

