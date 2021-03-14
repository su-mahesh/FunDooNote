using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.Interfaces;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class UserAccountBL : IUserAccountBL
    {
        readonly IUserAccountRL userAccountRL;
        readonly UserDetailValidation userDetailValidation;
        public UserAccountBL(IUserAccountRL userRegistrationsRL)
        {
            this.userAccountRL = userRegistrationsRL;
            userDetailValidation = new UserDetailValidation();
        }
 
        public ResponseUserAccount RegisterUser(RegisterUserAccount user)
        {
            try
            {
                if (userDetailValidation.ValidateFirstName(user.FirstName) &&
                userDetailValidation.ValidateLastName(user.LastName) &&
                userDetailValidation.ValidateEmailAddress(user.Email) &&
                userDetailValidation.ValidatePassword(user.Password))
                {
                    return userAccountRL.RegisterUser(user);
                }
                else
                {
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_INVALID_USER_DETAILS, "user details are invalid");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseUserAccount AthenticateUser(LoginUser user)
        {
            try
            {
                if (userDetailValidation.ValidateEmailAddress(user.Email) &&
                userDetailValidation.ValidatePassword(user.Password))
                {
                    return userAccountRL.AthenticateUser(user);
                }
                else
                {
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_INVALID_USER_DETAILS, "user details are details");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
