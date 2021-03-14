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
    }
}
