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

    }
}
