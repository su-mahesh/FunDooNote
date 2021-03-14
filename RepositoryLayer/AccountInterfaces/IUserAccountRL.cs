using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;

namespace RepositoryLayer.Interfaces
{
    public interface IUserAccountRL
    {
        ResponseUserAccount RegisterUser(RegisterUserAccount user);
        public ResponseUserAccount AthenticateUser(LoginUser loginUser);
        bool ResetAccountPassword(ResetPasswordModel user);
        ResponseUserAccount GetUserAccount(ForgetPasswordModel user);
    }
}
