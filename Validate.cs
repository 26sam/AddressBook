using System.Net.Mail;
using System.Text.RegularExpressions;

namespace AddressBook
{
    internal static class Validate
    {
        internal static bool IsValidName(string name, out string warning)
        {
            if(name == string.Empty)
            {
                warning = "Field cannot be empty";
                return false;
            }
            Regex regex = new Regex(@"^[a-zA-Z ]+$");
            if(!regex.IsMatch(name))
            {
                warning = "Invalid input. Name should only contain letters.";
                return false;
            }
            warning = string.Empty;
            return true;
        }
        internal static bool IsValidEmail(string emailAddress, out string warning)
        {
            try
            {
                var email = new MailAddress(emailAddress);
                warning = string.Empty;
                return true;
            }
            catch(Exception ex)
            {
                warning = ex.Message;
                return false;
            }
        }
        internal static bool IsValidNumber(string number, out string warning, int count)
        {
            Regex regex = new Regex(@"^[\d]+$");
            if (number == string.Empty)
            {
                warning = "This field is mandatory.";
                return false;
            }
            else if (!regex.IsMatch(number))
            {
                warning = "Invalid input. Number contains only digits";
                return false;
            }
            else if (number.Length != count)
            {
                warning = $"Number can contain only {count} digits.";
                return false;
            } 

            warning = string.Empty;
            return true;
        }
        internal static bool IsValidGender(string gender, out string warning)
        {
            if (gender.Length == 0 || gender.Length > 1 || (gender != "M" && gender != "F"))
            {
                warning = "\tInvalid Input.";
                return false;
            }
            warning = string.Empty;
            return true;
        }
    }
}
