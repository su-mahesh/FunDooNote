using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.RequestModel;

namespace RepositoryLayer.NotesInterface

{
    public interface INotesManagementRL
    {
        public ICollection<NoteModel> GetNotes(long UserID, bool IsTrash, bool IsArchieve);
        public NoteModel AddUserNote(NoteModel note);
        bool DeleteNote(long UserID, long noteID);
        NoteModel UpdateNote(NoteModel note);
        ICollection<NoteModel> GetReminderNotes(long userID);
        bool ToggleNotePin(long noteID, long userID);
    }
}
