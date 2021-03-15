﻿using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using LabelInterfaces;
using RepositoryLayer.LabelInterfeces;

namespace BusinessLayer.LabelServices
{
    /// <summary>
    /// manages user's note Label 
    /// </summary>
    /// <seealso cref="LabelInterfaces.ILabelManagementBL" />
    public class LabelManagementBL : ILabelManagementBL
    {
        readonly ILabelManagementRL labelManagementRL;
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelManagementBL"/> class.
        /// </summary>
        /// <param name="labelManagementRL">The label management rl.</param>
        public LabelManagementBL(ILabelManagementRL labelManagementRL)
        {
            this.labelManagementRL = labelManagementRL;
        }
        /// <summary>
        /// Adds the new user label.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="labelName">Name of the label.</param>
        /// <returns></returns>
        public bool AddUserLabel(long userID, string labelName)
        {
            try
            {
                return labelManagementRL.AddNewUserLabel(userID, labelName);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Changes the name of the label.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="labelID">The label identifier.</param>
        /// <param name="labelName">Name of the label.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Deletes the user label.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="labelID">The label identifier.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets the label notes.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="labelName">Name of the label.</param>
        /// <returns></returns>
        public ICollection<ResponseNoteModel> GetLabelNotes(long userID, string labelName)
        {
            try
            {
                return labelManagementRL.GetLabelNotes(userID, labelName);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the user labels.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
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
