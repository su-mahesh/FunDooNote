using System.Collections.Generic;
using CommonLayer.ResponseModel;

namespace LabelInterfaces
{
    public interface ILabelManagementBL
    {
        ICollection<Label> GetUserLabels(long userID);
    }
}