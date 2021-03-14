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
        public ResponseUserAccount GetUserAccount(LoginUser user);
    }
}
