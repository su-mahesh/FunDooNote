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
  
        public ICollection<ResponseNoteModel> GetActiveNotes(long UserID)
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
