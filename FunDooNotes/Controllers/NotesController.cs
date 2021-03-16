using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessLayer.NotesInterface;
using CommonLayer.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FundooNotes.Controllers
{
    /// <summary>
    /// User Notes Management
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiController]
    [Route("[controller]")]
    public class NotesController : Controller
    {
        readonly INotesManagementBL notesManagementBL;

        public NotesController(INotesManagementBL notesManagementBL)
        {
            this.notesManagementBL = notesManagementBL;
        }
        /// <summary>
        /// Gets the all active notes.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IActionResult GetActiveNotes()
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    string Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;

                    var result =  notesManagementBL.GetActiveNotes(UserID);
                    return Ok(new { success = true, user = Email, Notes = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.InnerException });
            }
        }
        /// <summary>
        /// Adds the new note.
        /// </summary>
        /// <param name="Note">The note.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("AddNote")]
        public IActionResult AddUserNote(ResponseNoteModel Note)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    Note.UserID = UserID;
                    ResponseNoteModel result = notesManagementBL.AddUserNote(Note);
                    return Ok(new { success = true, Note = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Gets the archived notes.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Archive")]
        public IActionResult GetArchivedNotes()
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    string Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;

                    var result = notesManagementBL.GetArchiveNotes(UserID);
                    return Ok(new { success = true, user = Email, Notes = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.InnerException });
            }
        }
        /// <summary>
        /// Gets the trashed notes.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Trash")]
        public IActionResult GetTrashNotes()
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    string Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;

                    var result = notesManagementBL.GetTrashNotes(UserID);
                    return Ok(new { success = true, user = Email, Notes = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Deletes the note by note id.
        /// </summary>
        /// <param name="NoteID">The note identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("Delete/{NoteID}")]
        public IActionResult DeleteNote(long NoteID)
        {
            try
            {
                if (User.Identity is ClaimsIdentity identity)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    string Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;

                    bool result = notesManagementBL.DeleteNote(UserID, NoteID);
                    return Ok(new { success = true, user = Email, Message = "note deleted" });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Gets the reminder notes.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Reminder")]
        public IActionResult GetReminderNotes()
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    string Email = claims.Where(p => p.Type == "Email").FirstOrDefault()?.Value;

                    var result = notesManagementBL.GetReminderNotes(UserID);
                    return Ok(new { success = true, user = Email, Notes = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Updates the note.
        /// </summary>
        /// <param name="Note">The note.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("Update")]
        public IActionResult UpdateNote(ResponseNoteModel Note)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    Note.UserID = UserID;
                    ResponseNoteModel result = notesManagementBL.UpdateNote(Note);
                    return Ok(new { success = true, Message =  "note updated", Note = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Toggles the pin, pin or unpin.
        /// </summary>
        /// <param name="NoteID">The note identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("Pin/{NoteID}")]
        public IActionResult TogglePin(long NoteID)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    bool result = notesManagementBL.ToggleNotePin(NoteID, UserID);
                    return Ok(new { success = true, Message = "note pin toggled", Note = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Toggles the archive, archive or unarchive note
        /// </summary>
        /// <param name="NoteID">The note identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("Archive/{NoteID}")]
        public IActionResult ToggleArchive(long NoteID)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    bool result = notesManagementBL.ToggleArchive(NoteID, UserID);
                    return Ok(new { success = true, Message = "note archive toggled", Note = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Changes the color of the note background.
        /// </summary>
        /// <param name="NoteID">The note identifier.</param>
        /// <param name="ColorCode">The color code.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("Color/{NoteID}/{ColorCode}")]
        public IActionResult ChangeNoteBackgroundColor(long NoteID, string ColorCode)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    bool result = notesManagementBL.ChangeBackgroundColor(NoteID, UserID, ColorCode);
                    return Ok(new { success = true, Message = "note background color changed", Note = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        /// <summary>
        /// Sets the note reminder.
        /// </summary>
        /// <param name="Reminder">The reminder.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("SetReminder")]
        public IActionResult SetNoteReminder(NoteReminder Reminder)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    Reminder.UserID = UserID;
                    bool result = notesManagementBL.SetNoteReminder(Reminder);
                    return Ok(new { success = true, Message = "note reminder added" });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
        [Authorize]
        [HttpPatch("UpdateCollaborators")]
        public IActionResult UpdateCollaborators(AddCollaboratorsModel Collaborators)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    long UserID = Convert.ToInt64(claims.Where(p => p.Type == "UserID").FirstOrDefault()?.Value);
                    Collaborators.UserID = UserID;
                    bool result = notesManagementBL.UpdateCollaborators(Collaborators);
                    return Ok(new { success = true, Message = "collaborators updated", Note = result });
                }
                return BadRequest(new { success = false, Message = "no user is active please login" });
            }
            catch (Exception exception)
            {
                return BadRequest(new { success = false, exception.Message });
            }
        }
    }
}
