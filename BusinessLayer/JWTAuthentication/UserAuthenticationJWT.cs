using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CommonLayer.ResponseModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLayer.JWTAuthentication
{
    /// <summary>
    /// JW Token generator
    /// </summary>
    public class UserAuthenticationJWT
    {
        private readonly IConfiguration config;
        public UserAuthenticationJWT(IConfiguration config)
        {
            this.config = config;
        }
        /// <summary>
        /// Generates the session JWT.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns></returns>
        public string GenerateSessionJWT(ResponseUserAccount userInfo)
        {
            DateTime ExpireTime = DateTime.Now.AddMinutes(120);
            return GenerateJSONWebToken(userInfo, ExpireTime);
        }
        /// <summary>
        /// Generates the password reset JWT.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns></returns>
        public string GeneratePasswordResetJWT(ResponseUserAccount userInfo)
        {
            DateTime ExpireTime = DateTime.Now.AddHours(2);
            return GenerateJSONWebToken(userInfo, ExpireTime);
        }
        /// <summary>
        /// Generates the json web token.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <param name="ExpireTime">The expire time.</param>
        /// <returns></returns>
        public string GenerateJSONWebToken(ResponseUserAccount userInfo, DateTime ExpireTime)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            IEnumerable<Claim> Claims = new Claim[] {
           //     new Clain("FundooNotes", "Notes");
                new Claim("UserID", userInfo.UserID.ToString()),
                new Claim("Email", userInfo.Email) };

            var token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"],
              claims: Claims,
              expires: ExpireTime,
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
