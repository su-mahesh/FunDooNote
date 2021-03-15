using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.Interfaces;
using BusinessLayer.JWTAuthentication;
using BusinessLayer.MSMQ;
using CommonLayer.EmailMessageModel;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class UserAccountBL : IUserAccountBL
    {
        readonly IUserAccountRL userAccountRL;
        readonly UserAuthenticationJWT userAuthentication;
        readonly UserDetailValidation userDetailValidation;
        readonly MSMQService msmq;
        public UserAccountBL(IUserAccountRL userRegistrationsRL, IConfiguration config)
        {
            this.userAccountRL = userRegistrationsRL;
            userDetailValidation = new UserDetailValidation();
            userAuthentication = new UserAuthenticationJWT(config);
            msmq = new MSMQService(config);
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
                    throw new UserDetailException(UserDetailException.ExceptionType.ENTERED_INVALID_USER_DETAILS, "user details are invalid");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ResetAccountPassword(ResetPasswordModel user)
        {
            try
            {
                if (user.NewPassword.Equals(user.ConfirmNewPassword) &&
                    userDetailValidation.ValidatePassword(user.NewPassword))
                {
                    return userAccountRL.ResetAccountPassword(user);
                }
                else
                {
                    throw new UserDetailException(UserDetailException.ExceptionType.CONFIRM_PASSWORD_DO_NO_MATCH, "New and comfirm password do not match");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SendForgottenPasswordLink(ForgetPasswordModel user)
        {
            try
            {
                ResponseUserAccount u = userAccountRL.GetUserAccount(user);
                if (u != null)
                {
                    var jwt = userAuthentication.GeneratePasswordResetJWT(u);
                    user.JwtToken = jwt;
                    msmq.SendPasswordResetLink(user);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}
