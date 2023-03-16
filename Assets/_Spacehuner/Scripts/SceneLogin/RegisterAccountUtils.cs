
using System.Linq;

namespace SH.Account
{
    public static class RegisterAccountUtils
    {
        public static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();
            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public static (bool isEnoughLength, bool hasUpperCase, bool hasNumber) IsValidPassword(string password)
        {
            bool isEnoughLength = false;
            bool hasUpperCase = false;
            bool hasNumber = false;
            if (password.Length >= 8)
            {
                isEnoughLength = true;
            }

            if (password.Any(char.IsUpper))
            {
                hasUpperCase = true;
            }

            if (password.Any(char.IsDigit))
            {
                hasNumber = true;
            }
            return (isEnoughLength, hasUpperCase, hasNumber);
        }

        public static bool IsMatchPassword(string password, string confirmPassword) => password == confirmPassword;
    }
}
