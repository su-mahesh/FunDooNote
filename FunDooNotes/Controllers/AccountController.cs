using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BusinessLayer.Interfaces;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using BusinessLayer.JWTAuthentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Controllers
{
    /// <summary>
    /// Manage account Register, Login, Reset Password
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("[controller]")] 
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration config;  
        readonly IUserAccountBL userAccountBL;
        readonly UserAuthenticationJWT userAuthentication;

        public AccountController(IUserAccountBL userAccountBL, IConfiguration config)
        {
            this.config = config;
            userAuthentication = new UserAuthenticationJWT(this.config);
            this.userAccountBL = userAccountBL;
        }

        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser(RegisterUserAccount user)
        {
            if (user == null)
            {
                return BadRequest("user is null.");
            }
            try
            {
                ResponseUserAccount result = userAccountBL.RegisterNewUser(user);               
                if (result != null)
                {
                    return Ok(new { success = true, Message = "User Registration Successful", user = result });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "User Registration Unsuccessful" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }                     
        }
        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="loginUser">The login user.</param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult AuthenticateUser(LoginUser loginUser)
        {
            if (loginUser == null)
            {
                return BadRequest("user is null.");
            }
            try
            {
                ResponseUserAccount user = userAccountBL.AthenticateUser(loginUser);
                if (user != null)
                {
                    var tokenString = userAuthentication.GenerateSessionJWT(user);
                    return Ok(new
                    {
                        success = true,
                        Message = "User Login Successful",
                        user,
                        token = tokenString
                    });
                }
                return BadRequest(new { success = false, Message = "User Login Unsuccessful" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetPasswordModel">The reset password model.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    var Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;
                    resetPasswordModel.Email = Email;
                    bool result = userAccountBL.ResetAccountPassword(resetPasswordModel);
                    if (result)
                    {
                        return Ok(new { success = true, Message = "password changed successfully" });
                    }
                }
                return BadRequest(new { success = false, Message = "password change unsuccessfull" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Resets the forgotten password.
        /// </summary>
        /// <param name="forgetPasswordModel">The forget password model.</param>
        /// <returns></returns>
        [HttpPost("ForgetPassword")]
        public IActionResult ResetForgottenPassword(ForgetPasswordModel forgetPasswordModel)
        {
            try
            {
                bool result = userAccountBL.SendForgottenPasswordLink(forgetPasswordModel);

                if (result)
                {
                    
                    return Ok(new { success = true, Message = "password reset link has been sent to your email id", email = forgetPasswordModel.Email });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "email id don't exist" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
    }
}
