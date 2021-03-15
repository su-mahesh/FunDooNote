using System.Collections.Generic;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;

namespace LabelInterfaces
{
    public interface ILabelManagementBL
    {
        ICollection<ResponseLabel> GetUserLabels(long userID);
        bool DeleteUserLabel(long userID, long labelID);
        bool ChangeLabelName(long userID, long labelID, string labelName);
        bool AddUserLabel(long userID, string labelName);
        public ICollection<ResponseNoteModel> GetLabelNotes(long userID, string labelName);
    }
}