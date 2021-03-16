using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.RequestModel;

namespace BusinessLayer.NotesInterface

{
    public interface INotesManagementBL
    {
        ICollection<ResponseNoteModel> GetActiveNotes(long UserID);
        public ResponseNoteModel AddUserNote(ResponseNoteModel note);
        ICollection<ResponseNoteModel> GetArchiveNotes(long userID);
        ICollection<ResponseNoteModel> GetTrashNotes(long userID);
        bool DeleteNote(long UserID, long noteID);
        ResponseNoteModel UpdateNote(ResponseNoteModel note);
        ICollection<ResponseNoteModel> GetReminderNotes(long userID);
        bool ToggleNotePin(long noteID, long userID);
        bool ToggleArchive(long noteID, long userID);
        bool ChangeBackgroundColor(long noteID, long userID, string colorCode);
        bool SetNoteReminder(NoteReminder reminder);
        bool UpdateCollaborators(AddCollaboratorsModel collaborators);
    }
}
