using System;
using System.Collections.Generic;
using System.Linq;
using CommonLayer.ResponseModel;
using RepositoryLayer.ContextDB;
using RepositoryLayer.Models;

namespace RepositoryLayer.LabelInterfeces
{
    public class LabelManagementRL : ILabelManagementRL
    {
        readonly NotesContext NotesDB;
        public LabelManagementRL(NotesContext notesDB)
        {
            NotesDB = notesDB;
        }

        public bool ChangeLabelName(long userID, long labelID, string labelName)
        {
            try
            {
                if (NotesDB.Labels.Any(N => N.UserId == userID && N.LabelId == labelID))
                {
                    if (NotesDB.Labels.Any(N => N.LabelId == labelID && N.LabelName == labelName))
                    {
                        return true;
                    }
                    if (!NotesDB.Labels.Any(N => N.UserId == userID && N.LabelName == labelName))
                    {
                        NotesDB.Labels.First(N => N.LabelId == labelID).LabelName = labelName;
                        NotesDB.SaveChanges();
                        return true;
                    }
                    throw new Exception("Label already exist");
                }
                return false;
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
                if (NotesDB.Labels.Any(N => N.UserId == userID && N.LabelId == labelID))
                {
                    NotesDB.Set<NoteLabel>().
                        RemoveIfExists(new NoteLabel { LabelId = labelID });
                    NotesDB.Set<Label>().
                        RemoveIfExists(new Label { LabelId = labelID });
                    NotesDB.SaveChanges();
                    return true;
                }
                return false;
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
                if (NotesDB.Labels.Any(N => N.UserId == userID))
                {
                    return NotesDB.Labels.Where(N => N.UserId == userID).
                    Select(N => new ResponseLabel { UserId = N.UserId, LabelId = N.LabelId, LabelName = N.LabelName }).ToList();

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
