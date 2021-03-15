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
        readonly ILabelManagementRL labelManagementRL;

        public LabelManagementBL(ILabelManagementRL labelManagementRL)
        {
            this.labelManagementRL = labelManagementRL;
        }

        public bool AddUserLabel(long userID, string labelName)
        {
            try
            {
                return labelManagementRL.AddUserLabel(userID, labelName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ChangeLabelName(long userID, long labelID, string labelName)
        {
            try
            {
                return labelManagementRL.ChangeLabelName(userID, labelID, labelName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteUserLabel(long userID, long labelID)
        {
            try
            {
                return labelManagementRL.DeleteUserLabel(userID, labelID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICollection<ResponseLabel> GetUserLabels(long userID)
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
