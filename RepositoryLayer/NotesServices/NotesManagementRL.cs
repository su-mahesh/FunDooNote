using System;
using System.Collections.Generic;
using System.Linq;
using CommonLayer.RequestModel;
using RepositoryLayer.ContextDB;
using RepositoryLayer.NotesInterface;
using RepositoryLayer.NotesServices;


namespace RepositoryLayer.NotesServises
{
    public class NotesManagementRL : INotesManagementRL
    {
        readonly NotesContext NotesDB;
        ICollection<ResponseNoteModel> responseNoteModels;
        public NotesManagementRL(NotesContext notesDB)
        {
            NotesDB = notesDB;
        }     

        public ICollection<ResponseNoteModel> GetActiveUserNotes(long UserID)
        {
            try
            {
                responseNoteModels = NotesDB.Notes.Where(N => N.UserId.Equals(UserID)
                && N.IsTrash == false && N.IsArchive == false).Select(N =>
                    new ResponseNoteModel
                    {
                        UserID = (long)N.UserId,
                        NoteID = N.NoteId,
                        Title = N.Title,
                        Text = N.Text,
                        CreatedOn = N.CreatedOn,
                        ReminderOn = N.ReminderOn,
                        BackgroundColor = N.BackgroundColor,
                        BackgroundImage = N.BackgroundImage,
                        IsArchive = N.IsArchive,
                        IsPin = N.IsPin,
                        IsTrash = N.IsTrash,
                        Labels = N.NoteLabels.Select(N => N.Label.LabelName).ToList(),
                        Collaborators = N.Collaborators.Select(C => C.CollaboratorEmail ).ToList()
                    }
                    ).ToList();
                return responseNoteModels;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
