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
    /// <summary>
    /// User account business logic
    /// </summary>
    /// <seealso cref="BusinessLayer.Interfaces.IUserAccountBL" />
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
        /// <summary>
        /// Registers the new user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="UserDetailException">user details are invalid</exception>
        public ResponseUserAccount RegisterNewUser(RegisterUserAccount user)
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
        /// <summary>
        /// Athenticates the user using email and password.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="UserDetailException">user details are invalid</exception>
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
        /// <summary>
        /// Resets the account password when password is known
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="UserDetailException">New and comfirm password do not match</exception>
        public bool ResetAccountPassword(ResetPasswordModel user)
        {
            try
            {
                if ( userDetailValidation.ValidatePassword(user.NewPassword))
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
        /// <summary>
        /// Sends the forgotten password link to email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
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
