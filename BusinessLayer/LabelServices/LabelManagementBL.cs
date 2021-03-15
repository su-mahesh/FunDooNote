using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.ResponseModel;
using LabelInterfaces;
using RepositoryLayer.LabelInterfeces;

namespace BusinessLayer.LabelServices
{
    public class LabelManagementBL : ILabelManagementBL
    {
       ILabelManagementRL labelManagementRL;

        public LabelManagementBL(ILabelManagementRL labelManagementRL)
        {
            this.labelManagementRL = labelManagementRL;
        }

        public ICollection<Label> GetUserLabels(long userID)
        {
            try
            {
                return labelManagementRL.GetUserLabels(userID);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
