using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserDetailException: Exception
    {
        public enum ExceptionType
        {
            ENTERED_LESSTHAN_MINIMUM_LENGTH,
            ENTERED_NULL,
            ENTERED_EMPTY,
            ENTERED_DIGIT_IN_NAME,
            ENTERED_INVALID_EMAIL_TLD,
            ENTERED_INVALID_EMAIL_USERNAME,
            ENTERED_DIGIT_IN_COUNTRY_TLD,
            ENTERED_PUNCTUATION_IN_START_OF_TLD,
            ENTERED_INVALID_USER_DETAILS,
            ENTERED_FIRST_LETTER_LOWER,
            ENTERED_OTHER_THAN_FIRST_LETTER_UPPER,
            ENTERED_PUNCTUATION_IN_NAME,
            ENTERED_WITHOUT_,
            ENTERED_WITHOUT_ATRATE_SYMBOL,
            ENTERED_WITHOUT_AT_LEAST_ONE_SPEACIAL_CHAR,
            ENTERED_WITHOUT_AT_LEAST_ONE_UPPER_CHAR,
            ENTERED_WITHOUT_AT_LEAST_ONE_LOWER_CHAR,
            ENTERED_FIRST_CHAR_DOT,
            CONFIRM_PASSWORD_DO_NO_MATCH,
            WRONG_CURRENT_PASSWORD
        }
        public ExceptionType exceptionType;
        public UserDetailException(ExceptionType exceptionType, string message) : base(message)
        {
            this.exceptionType = exceptionType;
        }
    }
}
