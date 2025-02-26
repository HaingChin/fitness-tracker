using System;
using System.Text.RegularExpressions;

namespace fitness_tracker
{
    public static class InputValidator
    {
        // Username must be alphanumeric
        public static bool IsValidUsername(string username) => Regex.IsMatch(username, "^[a-zA-Z0-9]+$");

        // Basic email validation pattern
        public static bool IsValidEmail(string email) => Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        // Password must be at least 12 characters with uppercase and lowercase letters
        public static bool IsValidPassword(string password) =>
            password.Length >= 12 && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[a-z]");

        // Validate phone number (7-15 digits only)
        public static bool IsValidPhoneNumber(string phone) => Regex.IsMatch(phone, @"^\d{7,15}$");

    }
}

