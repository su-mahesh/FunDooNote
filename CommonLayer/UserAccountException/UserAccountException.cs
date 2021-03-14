using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.UserAccountException
{
    public class UserAccountException : Exception
    {
        public enum ExceptionType { 
            EMAIL_ALREADY_EXIST,
            EMAIL_DONT_EXIST,
            WRONG_PASSWORD
        }
        public ExceptionType exceptionType;
        public UserAccountException(ExceptionType exceptionType,string message) : base(message)
        {
            this.exceptionType = exceptionType;
        }
    }
}
