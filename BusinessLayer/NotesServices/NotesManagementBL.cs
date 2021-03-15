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
        readonly INotesManagementRL NotesManagementRL;

        public NotesManagementBL(INotesManagementRL notesManagementRL)
        {
            NotesManagementRL = notesManagementRL;
        }

        public NoteModel AddUserNote(NoteModel note)
        {
            try
            {
                if (note.Labels != null)
                {
                    note.Labels = note.Labels.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                }
                if (note.Collaborators != null)
                {
                    note.Collaborators = note.Collaborators.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                }
                if (note.IsTrash || note.IsArchive)
                {
                    note.IsPin = false;
                }
                return NotesManagementRL.AddUserNote(note);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteNote(long UserID, long noteID)
        {
            try
            {
                bool result = NotesManagementRL.DeleteNote(UserID, noteID);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICollection<NoteModel> GetActiveNotes(long UserID)
        {
            try
            {
                ICollection<NoteModel> result = NotesManagementRL.GetNotes(UserID, false, false);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICollection<NoteModel> GetArchiveNotes(long UserID)
        {
            try
            {
                return NotesManagementRL.GetNotes(UserID, false, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICollection<NoteModel> GetReminderNotes(long UserID)
        {
            try
            {
                return NotesManagementRL.GetReminderNotes(UserID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICollection<NoteModel> GetTrashNotes(long UserID)
        {
            try
            {
                return NotesManagementRL.GetNotes(UserID, true, false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ToggleArchive(long noteID, long userID)
        {
            try
            {
                return NotesManagementRL.ToggleArchive(noteID, userID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ToggleNotePin(long noteID, long userID)
        {
            try
            {
                return NotesManagementRL.ToggleNotePin(noteID,userID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ChangeBackgroundColor(long noteID, long userID, string colorCode)
        {
            try
            {
                return NotesManagementRL.ChangeBackgroundColor(noteID, userID, colorCode);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NoteModel UpdateNote(NoteModel note)
        {
            try
            {
                if (note.Labels != null)
                {
                    note.Labels = note.Labels.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                }
                if (note.Collaborators != null)
                {
                    note.Collaborators = note.Collaborators.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                }
                if (note.IsTrash || note.IsArchive)
                {
                    note.IsPin = false;
                }
                return NotesManagementRL.UpdateNote(note);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SetNoteReminder(NoteReminder reminder)
        {
            try
            {
                if (reminder.ReminderOn < DateTime.Now)
                {
                    throw new Exception("Time is passed");
                }
                if (reminder.NoteID == default)
                {
                    throw new Exception("NoteID missing");
                }
                return NotesManagementRL.SetNoteReminder(reminder);
            }
            catch (Exception)
            {

                throw;
            }           
        }
    }
}
