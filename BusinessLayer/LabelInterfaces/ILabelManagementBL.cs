using System.Collections.Generic;
using CommonLayer.ResponseModel;

namespace LabelInterfaces
{
    public interface ILabelManagementBL
    {
        ICollection<ResponseLabel> GetUserLabels(long userID);
        bool DeleteUserLabel(long userID, long labelID);
    }
}