using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.RequestModel;

namespace RepositoryLayer.NotesInterface

{
    public interface INotesManagementRL
    {
        ICollection<NoteModel> GetActiveUserNotes(long UserID);
        NoteModel AddUserNote(NoteModel note);
    }
}
