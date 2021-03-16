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
        /// <summary>
        /// Adds the user note.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        public ResponseNoteModel AddUserNote(ResponseNoteModel note)
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
        /// <summary>
        /// Deletes the note.
        /// </summary>
        /// <param name="UserID">The user identifier.</param>
        /// <param name="noteID">The note identifier.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets the active notes.
        /// </summary>
        /// <param name="UserID">The user identifier.</param>
        /// <returns></returns>
        public ICollection<ResponseNoteModel> GetActiveNotes(long UserID)
        {
            try
            {
                ICollection<ResponseNoteModel> result = NotesManagementRL.GetNotes(UserID, false, false);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the archive notes.
        /// </summary>
        /// <param name="UserID">The user identifier.</param>
        /// <returns></returns>
        public ICollection<ResponseNoteModel> GetArchiveNotes(long UserID)
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
        /// <summary>
        /// Gets the reminder notes.
        /// </summary>
        /// <param name="UserID">The user identifier.</param>
        /// <returns></returns>
        public ICollection<ResponseNoteModel> GetReminderNotes(long UserID)
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
        /// <summary>
        /// Gets the trash notes.
        /// </summary>
        /// <param name="UserID">The user identifier.</param>
        /// <returns></returns>
        public ICollection<ResponseNoteModel> GetTrashNotes(long UserID)
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
        /// <summary>
        /// Toggles the archive.
        /// </summary>
        /// <param name="noteID">The note identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Toggles the note pin.
        /// </summary>
        /// <param name="noteID">The note identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Changes the color of the background.
        /// </summary>
        /// <param name="noteID">The note identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="colorCode">The color code.</param>
        /// <returns></returns>
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
        public ResponseNoteModel UpdateNote(ResponseNoteModel note)
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
        /// <summary>
        /// Sets the note reminder.
        /// </summary>
        /// <param name="reminder">The reminder.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Time is passed
        /// or
        /// NoteID missing
        /// </exception>
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
        /// <summary>
        /// Updates the collaborators.
        /// </summary>
        /// <param name="collaborators">The collaborators.</param>
        /// <returns></returns>
        public bool UpdateCollaborators(AddCollaboratorsModel collaborators)
        {
            try
            {
                return NotesManagementRL.UpdateCollaborators(collaborators);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Removes the reminder.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="noteID">The note identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception">NoteID missing</exception>
        public bool RemoveReminder(long userID, long noteID)
        {
            try
            {
                if (noteID == default)
                {
                    throw new Exception("NoteID missing");
                }
                return NotesManagementRL.RemoveReminder(userID, noteID);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
