using System;
using System.Collections.Generic;
using System.Linq;
using CommonLayer.RequestModel;
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

        public bool AddUserLabel(long userID, string labelName)
        {
            try
            {
                if (!NotesDB.Labels.Any(N => N.UserId == userID && N.LabelName == labelName))
                {
                    NotesDB.Labels.Add(new Label {UserId = userID, LabelName = labelName });
                    NotesDB.SaveChanges();
                    return true;                  
                }
                else
                {
                    throw new Exception("Label already exist");
                }
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

        public ICollection<ResponseNoteModel> GetLabelNotes(long userID, string labelName)
        {
            if (NotesDB.NoteLabels.Any(L => L.UserId == userID && L.Label.LabelName == labelName))
            {
                return NotesDB.NoteLabels.
               Where(NL => NL.Note.UserId == userID && NL.Label.LabelName == labelName).
               Select(N =>
               new ResponseNoteModel
               {
                   UserID = (long)N.UserId,
                   NoteID = (long)N.NoteId,
                   Title = N.Note.Title,
                   Text = N.Note.Text,
                   ReminderOn = N.Note.ReminderOn,
                   BackgroundColor = N.Note.BackgroundColor,
                   BackgroundImage = N.Note.BackgroundImage,
                   IsArchive = N.Note.IsArchive,
                   IsPin = N.Note.IsPin,
                   IsTrash = N.Note.IsTrash,
                   Labels = N.Note.NoteLabels.Select(N => N.Label.LabelName).ToList(),
                   Collaborators = N.Note.Collaborators.Select(C => C.CollaboratorEmail).ToList()
               }
               ).ToList();
            }
            return null;
        }
    }
}
