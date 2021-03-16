using System;
using System.Collections.Generic;
using BusinessLayer.NotesInterface;
using CommonLayer.RequestModel;
using RepositoryLayer.NotesInterface;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BusinessLayer.NotesServices
{
    public class NotesManagementBL : INotesManagementBL
    {
        private readonly IDistributedCache distributedCache;
        readonly INotesManagementRL NotesManagementRL;

        public NotesManagementBL(INotesManagementRL notesManagementRL, IDistributedCache distributedCache)
        {
            NotesManagementRL = notesManagementRL;
            this.distributedCache = distributedCache;
        }

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

        public async Task<ICollection<ResponseNoteModel>> GetActiveNotes(long UserID)
        {
            var cacheKey = UserID.ToString();
            string serializedNotes;
            ICollection<ResponseNoteModel> Notes;
            try
            {
                   var redisNoteCollection = await distributedCache.GetAsync(cacheKey);
                if (redisNoteCollection != null)
                {
                    serializedNotes = Encoding.UTF8.GetString(redisNoteCollection);
                    Notes = JsonConvert.DeserializeObject<List<ResponseNoteModel>>(serializedNotes);
                }
                else
                {
                    Notes = NotesManagementRL.GetNotes(UserID, false, false);
                    serializedNotes = JsonConvert.SerializeObject(Notes);
                    redisNoteCollection = Encoding.UTF8.GetBytes(serializedNotes);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(cacheKey, redisNoteCollection);
                }
                return Notes;
            }
            catch (Exception)
            {
                throw;
            }
        }

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
    }
}
