using System;
using System.Collections.Generic;
using System.Linq;
using CommonLayer.ResponseModel;
using RepositoryLayer.ContextDB;

namespace RepositoryLayer.LabelInterfeces
{
    public class LabelManagementRL : ILabelManagementRL
    {
        readonly NotesContext NotesDB;
        public LabelManagementRL(NotesContext notesDB)
        {
            NotesDB = notesDB;
        }
        public ICollection<Label> GetUserLabels(long userID)
        {
            try
            {
                if (NotesDB.Labels.Any(N => N.UserId == userID))
                {
                    return NotesDB.Labels.Where(N => N.UserId == userID).
                    Select(N => new Label { UserId = N.UserId, LabelId = N.LabelId, LabelName = N.LabelName }).ToList();

                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
