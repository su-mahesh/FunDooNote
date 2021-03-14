using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BusinessLayer.Interfaces;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using FundooNotes.JWTAuthentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration config;  
        readonly IUserAccountBL userAccountBL;
        readonly UserAuthenticationJWT userAuthentication;

        public AccountController(IUserAccountBL userRegistrationsBL, IConfiguration config)
        {
            this.config = config;
            this.userAccountBL = userRegistrationsBL;
            userAuthentication = new UserAuthenticationJWT(this.config);
        }
       
        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser(RegisterUserAccount user)
        {
            if (user == null)
            {
                return BadRequest("user is null.");
            }
            try
            {
                ResponseUserAccount result = userAccountBL.RegisterUser(user);               
                if (result != null)
                {
                    var NewUser = new
                    {
                        result.UserID,
                        result.FirstName,
                        result.LastName,
                        result.Email
                    };

                    return Ok(new { success = true, Message = "User Registration Successful", user = NewUser });
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
    }
}
