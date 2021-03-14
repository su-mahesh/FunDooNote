using System;
using System.Collections.Generic;
using BusinessLayer.NotesInterface;
using CommonLayer.RequestModel;
using RepositoryLayer.NotesInterface;
using System.Linq;

namespace BusinessLayer.NotesServices
{
    public class NotesManagementBL : INotesManagementBL
    {
        INotesManagementRL NotesManagementRL;

        public NotesManagementBL(INotesManagementRL notesManagementRL)
        {
            NotesManagementRL = notesManagementRL;
        }

        public NoteModel AddUserNote(NoteModel note)
        {
            try
            {
                if (note.Labels != null)
                {
                    note.Labels = note.Labels.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                }
                if (note.Collaborators != null)
                {
                    note.Collaborators = note.Collaborators.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                }
                if (note.IsTrash || note.IsArchive)
                {
                    note.IsPin = false;
                }
                return NotesManagementRL.AddUserNote(note);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICollection<NoteModel> GetActiveNotes(long UserID)
        {
            try
            {
                return NotesManagementRL.GetActiveUserNotes(UserID);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
