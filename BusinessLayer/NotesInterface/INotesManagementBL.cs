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
        bool DeleteNote(long UserID, long noteID);
        NoteModel UpdateNote(NoteModel note);
        ICollection<NoteModel> GetReminderNotes(long userID);
        bool ToggleNotePin(long noteID, long userID);
        bool ToggleArchive(long noteID, long userID);
    }
}
