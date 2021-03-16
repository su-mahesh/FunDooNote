using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.RequestModel;

namespace RepositoryLayer.NotesInterface

{
    public interface INotesManagementRL
    {
        public ICollection<ResponseNoteModel> GetNotes(long UserID, bool IsTrash, bool IsArchieve);
        public ResponseNoteModel AddUserNote(ResponseNoteModel note);
        bool DeleteNote(long UserID, long noteID);
        ResponseNoteModel UpdateNote(ResponseNoteModel note);
        ICollection<ResponseNoteModel> GetReminderNotes(long userID);
        bool ToggleNotePin(long noteID, long userID);
        bool ToggleArchive(long noteID, long userID);
        bool ChangeBackgroundColor(long noteID, long userID, string colorCode);
        bool SetNoteReminder(NoteReminder reminder);
        bool UpdateCollaborators(AddCollaboratorsModel collaborators);
        bool RemoveReminder(long userID, long noteID);
    }
}
