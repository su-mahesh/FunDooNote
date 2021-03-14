using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;

namespace BusinessLayer.Interfaces
{
    public interface IUserAccountBL
    {
        public ResponseUserAccount RegisterUser(RegisterUserAccount user);
        public ResponseUserAccount AthenticateUser(LoginUser user);
        bool ResetAccountPassword(ResetPasswordModel resetPasswordModel);
        bool SendForgottenPasswordLink(ForgetPasswordModel forgetPasswordModel);
    }
}
