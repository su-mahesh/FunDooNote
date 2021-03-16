using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommonLayer.RequestModel;

namespace BusinessLayer.NotesInterface

{
    public interface INotesManagementBL
    {
        public Task<ICollection<ResponseNoteModel>> GetActiveNotes(long UserID);
        public ResponseNoteModel AddUserNote(ResponseNoteModel note);
        Task<ICollection<ResponseNoteModel>> GetArchiveNotes(long userID);
        Task<ICollection<ResponseNoteModel>> GetTrashNotes(long userID);
        Task<bool> DeleteNote(long UserID, long noteID);
        Task<ResponseNoteModel> UpdateNote(ResponseNoteModel note);
        Task<ICollection<ResponseNoteModel>> GetReminderNotes(long userID);
        Task<bool> ToggleNotePin(long noteID, long userID);
        Task<bool> ToggleArchive(long noteID, long userID);
        Task<bool> ChangeBackgroundColor(long noteID, long userID, string colorCode);
        Task<bool> SetNoteReminder(NoteReminder reminder);
        Task<bool> UpdateCollaborators(AddCollaboratorsModel collaborators);
    }
}
