using System.Collections.Generic;
using CommonLayer.ResponseModel;

namespace RepositoryLayer.LabelInterfeces
{
    public interface ILabelManagementRL
    {
        ICollection<Label> GetUserLabels(long userID);
    }
}