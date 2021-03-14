using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.RequestModel;

namespace BusinessLayer.NotesInterface

{
    public interface INotesManagementBL
    {
        ICollection<NoteModel> GetActiveNotes(long UserID);
        public NoteModel AddUserNote(NoteModel note);
        ICollection<NoteModel> GetArchiveNotes(long userID);
        ICollection<NoteModel> GetTrashNotes(long userID);
    }
}
