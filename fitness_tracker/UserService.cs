﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace fitness_tracker
{
    public class UserService
    {
        private readonly fitness_treacker_dbTableAdapters.users_tblTableAdapter userAdapter;

        public UserService()
        {
            userAdapter = new fitness_treacker_dbTableAdapters.users_tblTableAdapter();
        }

        // Check if a user already exists by username or email
        public bool IsUsernameExists(string username)
        {
            object result = userAdapter.CheckUsernameExists(username);
            return result != null && Convert.ToInt32(result) > 0;
        }

        public bool IsEmailExists(string email)
        {
            object result = userAdapter.CheckEmailExists(email);
            return result != null && Convert.ToInt32(result) > 0;
        }

        // Register a new user
        public void Register(string username, string email, string password)
        {
            int newUserId = GenerateNewUserId();
            string hashedPassword = HashPassword(password);
            userAdapter.Insert(newUserId, username, email, hashedPassword, DateTime.Now);
        }

        // Validate username and password against stored credentials
        public bool ValidateUser(string username, string password)
        {
            DataTable userdt = userAdapter.GetDataByUsername(username);
            return userdt.Rows.Count > 0 && userdt.Rows[0]["passwords"].ToString() == HashPassword(password);
        }

        // Generate a new user ID by incrementing the highest existing ID
        public int GenerateNewUserId()
        {
            DataTable userdt = userAdapter.GetData();
            return userdt.Rows.Count > 0 ? userdt.AsEnumerable().Max(row => row.Field<int>("id")) + 1 : 1;
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
    }
}
