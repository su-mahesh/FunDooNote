using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BusinessLayer.Services
{
    public class UserDetailValidation
    {
        public static Regex FirstNameRegex = new Regex(@"^[A-Z][a-z]{2,}$");
        public static Regex LastNameRegex = new Regex(@"^[A-Z][a-zA-Z]{2,}$");
        public static Regex EmailAddressRegex = new Regex(@"^[a-zA-Z0-9]+([._+-][a-zA-Z0-9]+)*@[a-zA-Z0-9]+.[a-zA-Z]{2,4}([.][a-zA-Z]{2,})?$");
        public static Regex PasswordRegex = new Regex(@"^(?=.{8,20}$)(?=.*[\d])(?=.*[A-Z])[\w]*[\W][\w]*$");
        public static char[] SpecialChars = "@#%^_/<>`~.&!+*-?^$()[]{}|\\".ToCharArray();
        readonly Func<Regex, string, bool> IsValid = (reg, field) => reg.IsMatch(field);

        public bool ValidateFirstName(string FirstName)
        {
            try
            {
                if (FirstName == null)         
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_NULL, "first name should not be null");
                if (FirstName.Equals(string.Empty))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_EMPTY, "first name should not be empty");
                if (Char.IsLower(FirstName, 0))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_FIRST_LETTER_LOWER, "first letter should not be lower");
                if (FirstName.Any(Char.IsDigit))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_DIGIT_IN_NAME, "first name should not contain digits");
                if (FirstName.Length < 3)
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_LESSTHAN_MINIMUM_LENGTH, "first name should not be less than minimum length");
                if (FirstName.Any(Char.IsPunctuation))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_PUNCTUATION_IN_NAME, "first name should not contain punctuations");
                return IsValid(FirstNameRegex, FirstName);

            }
            catch (NullReferenceException)
            {
                throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_NULL, "first name should not be null");
            }
            
        }
        public bool ValidateLastName(string LastName)
        {
            try
            {
                if (LastName == null)
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_NULL, "last name should not be null");
                if (LastName.Equals(string.Empty))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_EMPTY, "last name should not be empty");
                if (Char.IsLower(LastName, 0))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_FIRST_LETTER_LOWER, "first letter should not be lower");
                if (LastName[1..].Any(Char.IsUpper))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_OTHER_THAN_FIRST_LETTER_UPPER, "other than first letter should be lower");
                if (LastName.Any(Char.IsDigit))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_DIGIT_IN_NAME, "last name should not contain digits");
                if (LastName.Length < 3)
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_LESSTHAN_MINIMUM_LENGTH, "last name should not be less than minimum length");
                if (LastName.Any(Char.IsPunctuation))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_PUNCTUATION_IN_NAME, "first name should not contain punctuations");
                return IsValid(LastNameRegex, LastName);
            }
            catch (NullReferenceException)
            {
                throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_NULL, "last name should not be null");
            }            
        }
        public bool ValidateEmailAddress(string EmailAddress)
        {
            try
            {
                if (EmailAddress == null)
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_NULL, "Email Address should not be null");
                if (EmailAddress.Equals(string.Empty))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_EMPTY, "email address should not be empty");
                string Username = EmailAddress.Substring(0, 1);
                if (Username.Any(Char.IsPunctuation))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_INVALID_EMAIL_USERNAME, "email address username should not start with spacial character");
                if (EmailAddress.Length < 6)
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_LESSTHAN_MINIMUM_LENGTH, "email address should not be less than minimum length");
                string Country_Tld = EmailAddress[(EmailAddress.LastIndexOf(".") + 1)..];
                if (Country_Tld.Any(Char.IsDigit))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_DIGIT_IN_COUNTRY_TLD, "email address country tld should not contain spacial characters");
                if (!EmailAddress.Contains('@'))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_WITHOUT_ATRATE_SYMBOL, "email should contain @ symbol");

                return IsValid(EmailAddressRegex, EmailAddress);
            }
            catch (NullReferenceException)
            {
                throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_NULL, "email address should not be null");
            }
        }
        public bool ValidatePassword(string Password)
        {
            try
            {
                if (Password == null)
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_NULL, "Password should not be null");
                if (Password.Length.Equals(0))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_EMPTY, "password should not be empty");
                if (Password.Length < 8)
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_LESSTHAN_MINIMUM_LENGTH, "password should not be less than minimum length");
                if (Password.IndexOfAny(SpecialChars) == -1)
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_WITHOUT_AT_LEAST_ONE_SPEACIAL_CHAR, "password should contain at least one special character");
                if (!Password.Any(Char.IsUpper))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_WITHOUT_AT_LEAST_ONE_UPPER_CHAR, "password should contain at least one upper character");
                if (!Password.Any(Char.IsLower))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_WITHOUT_AT_LEAST_ONE_LOWER_CHAR, "password should contain at least one lower character");
                if (!Password.Any(Char.IsDigit))
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_WITHOUT_AT_LEAST_ONE_LOWER_CHAR, "password should contain at least one lower character");

                return IsValid(PasswordRegex, Password);
            }
            catch (NullReferenceException)
            {
                throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_NULL, "password should not be null");
            }           
        }
    }
}
