using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.RequestModel;

namespace BusinessLayer.NotesInterface

{
    public interface INotesManagementBL
    {
        ICollection<ResponseNoteModel> GetActiveNotes(long UserID);
    }
}
