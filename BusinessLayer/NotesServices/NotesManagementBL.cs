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
using BusinessLayer.RedisCacheService;

namespace BusinessLayer.NotesServices
{
    public class NotesManagementBL : INotesManagementBL
    {
        private readonly IDistributedCache distributedCache;
        readonly INotesManagementRL NotesManagementRL;
        RedisCacheServiceBL redis;

        public NotesManagementBL(INotesManagementRL notesManagementRL, IDistributedCache distributedCache)
        {
            NotesManagementRL = notesManagementRL;
            this.distributedCache = distributedCache;
            redis = new RedisCacheServiceBL(this.distributedCache);
        }

        public ResponseNoteModel AddUserNote(ResponseNoteModel note)
        {
            return AddUserNoteTask(note).Result;
        }
        public async Task<ResponseNoteModel> AddUserNoteTask(ResponseNoteModel note) {
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
                await redis.RemoveNotesRedisCache(note.UserID);
                return NotesManagementRL.AddUserNote(note);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteNote(long UserID, long noteID)
        {
            try
            {
                await redis.RemoveNotesRedisCache(UserID);
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
            var cacheKey = "ActiveNotes:" + UserID.ToString();
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
                    await redis.AddRedisCache(cacheKey, Notes);
                }
                return Notes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<ResponseNoteModel>> GetArchiveNotes(long UserID)
        {
            var cacheKey = "ArchiveNotes:" + UserID.ToString();
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
                    Notes = NotesManagementRL.GetNotes(UserID, false, true);
                    await redis.AddRedisCache(cacheKey, Notes);
                }
                return Notes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<ResponseNoteModel>> GetReminderNotes(long UserID)
        {
            var cacheKey = "ReminderNotes:" + UserID.ToString();
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
                    Notes = NotesManagementRL.GetReminderNotes(UserID);
                    await redis.AddRedisCache(cacheKey, Notes);
                }
                return Notes;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ICollection<ResponseNoteModel>> GetTrashNotes(long UserID)
        {

            var cacheKey = "ReminderNotes:" + UserID.ToString();
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
                    Notes = NotesManagementRL.GetNotes(UserID, true, false);
                    await redis.AddRedisCache(cacheKey, Notes);
                }
                return Notes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ToggleArchive(long noteID, long userID)
        {
            try
            {
                await redis.RemoveNotesRedisCache(userID);
                return NotesManagementRL.ToggleArchive(noteID, userID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ToggleNotePin(long noteID, long userID)
        {
            try
            {
                await redis.RemoveNotesRedisCache(userID);
                return NotesManagementRL.ToggleNotePin(noteID,userID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ChangeBackgroundColor(long noteID, long userID, string colorCode)
        {
            try
            {
                await redis.RemoveNotesRedisCache(userID);
                return NotesManagementRL.ChangeBackgroundColor(noteID, userID, colorCode);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ResponseNoteModel> UpdateNote(ResponseNoteModel note)
        {
            try
            {
                await redis.RemoveNotesRedisCache(note.UserID);
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

        public async Task<bool> SetNoteReminder(NoteReminder reminder)
        {
            try
            {
                await redis.RemoveNotesRedisCache(reminder.UserID);
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

        public async Task<bool> UpdateCollaborators(AddCollaboratorsModel collaborators)
        {
            try
            {
                await redis.RemoveNotesRedisCache(collaborators.UserID);
                return NotesManagementRL.UpdateCollaborators(collaborators);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
