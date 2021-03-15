using System.Collections.Generic;
using CommonLayer.ResponseModel;

namespace RepositoryLayer.LabelInterfeces
{
    public interface ILabelManagementRL
    {
        ICollection<ResponseLabel> GetUserLabels(long userID);
        bool DeleteUserLabel(long userID, long labelID);
        bool ChangeLabelName(long userID, long labelID, string labelName);
    }
}