﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using CommonLayer.UserAccountException;
using RepositoryLayer.ContextDB;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class UserAccountRL : IUserAccountRL
    {
        readonly NotesContext UserDB;
        readonly PasswordEncryption passwordEncryption;

        public UserAccountRL(NotesContext UserDB)
        {
            this.UserDB = UserDB;
            passwordEncryption = new PasswordEncryption();
        }
        public ResponseUserAccount RegisterUser(RegisterUserAccount user)
        {
            if (!UserDB.UserAccounts.Any(u => u.Email == user.Email))
            {
                user.Password = passwordEncryption.EncryptPassword(user.Password);
                UserDB.UserAccounts.Add(
                    new UserAccount { FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password
                    });
                UserDB.SaveChanges();
                return UserDB.UserAccounts.Where(u => u.Email.Equals(user.Email)).Select(u => new ResponseUserAccount
                {
                    UserID = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                }).ToList().First();
            }
            else
            {
                throw new UserAccountException(UserAccountException.ExceptionType.EMAIL_ALREADY_EXIST, "email id already registered");
            }
        }
        public ResponseUserAccount AthenticateUser(LoginUser loginUser)
        {
            if (UserDB.UserAccounts.Any(U => U.Email.Equals(loginUser.Email)))
            {
                string Password = passwordEncryption.EncryptPassword(loginUser.Password);
                if (UserDB.UserAccounts.FirstOrDefault(u => u.Email == loginUser.Email).Password.Equals(Password))
                {
                    ResponseUserAccount User = UserDB.UserAccounts.Where(u => u.Email == loginUser.Email).
                        Select(u => new ResponseUserAccount { 
                        UserID = u.UserId,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        }).ToList().First();
                    return User;
                }
                else
                    throw new UserAccountException(UserAccountException.ExceptionType.WRONG_PASSWORD, "wrong password");
            }
            else
            {
                throw new UserAccountException(UserAccountException.ExceptionType.EMAIL_DONT_EXIST, "email address is not registered");
            }
        }
    }
}