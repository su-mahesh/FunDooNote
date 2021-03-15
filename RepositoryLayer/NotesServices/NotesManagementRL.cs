using System;
using System.Collections.Generic;
using System.Linq;
using CommonLayer.RequestModel;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.ContextDB;
using RepositoryLayer.Models;
using RepositoryLayer.NotesInterface;
using RepositoryLayer.NotesServices;


namespace RepositoryLayer.NotesServises
{
    public class NotesManagementRL : INotesManagementRL
    {
        readonly NotesContext NotesDB;
        ICollection<NoteModel> responseNoteModels;
        public NotesManagementRL(NotesContext notesDB)
        {
            NotesDB = notesDB;
        }

        public NoteModel AddUserNote(NoteModel note)
        {
            try
            {
                var NewNote = new Note
                {
                    UserId = note.UserID,
                    Title = note.Title,
                    Text = note.Text,
                    ReminderOn = note.ReminderOn,
                    BackgroundColor = note.BackgroundColor,
                    BackgroundImage = note.BackgroundImage,
                    IsArchive = note.IsArchive,
                    IsPin = note.IsPin,
                    IsTrash = note.IsTrash,
                };
                NotesDB.Notes.Add(NewNote);
                NotesDB.SaveChanges();

                if (note.Labels != null)
                {
                    note.Labels.ToList().ForEach(L =>
                    NotesDB.Set<Label>().AddIfNotExists(new Label { UserId = NewNote.UserId, LabelName = L }, x => x.LabelName.Equals(L)));
                    NotesDB.SaveChanges();
                    note.Labels.ToList().ForEach(L => NotesDB.NoteLabels.Add(
                        new NoteLabel
                        {
                            NoteId = NewNote.NoteId,
                            LabelId = NotesDB.Labels.FirstOrDefault(a => a.LabelName == L).LabelId
                        }));
                }
                if (note.Collaborators != null)
                {
                    note.Collaborators.ToList().ForEach(C =>
                    NotesDB.Collaborators.Add(
                        new Collaborator { UserId = NewNote.UserId, NoteId = NewNote.NoteId, CollaboratorEmail = C }));
                    NotesDB.SaveChanges();
                }
                var NewResponseNote = new NoteModel
                {
                    UserID = (long)NewNote.UserId,
                    NoteID = NewNote.NoteId,
                    Title = NewNote.Title,
                    Text = NewNote.Text,
                    CreatedOn = NewNote.CreatedOn,
                    ReminderOn = NewNote.ReminderOn,
                    BackgroundColor = NewNote.BackgroundColor,
                    BackgroundImage = NewNote.BackgroundImage,
                    IsArchive = NewNote.IsArchive,
                    IsPin = NewNote.IsPin,
                    IsTrash = NewNote.IsTrash,
                    Labels = NewNote.NoteLabels.Select(N => N.Label.LabelName).ToList(),
                    Collaborators = NewNote.Collaborators.Select(C => C.CollaboratorEmail).ToList()
                };
                return NewResponseNote;
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
                if (NotesDB.Notes.Any(n => n.NoteId == noteID && n.UserId == UserID))
                {
                   var note = NotesDB.Notes.Find(noteID );
                    if (note.IsTrash)
                    {
                        NotesDB.Entry(note).State = EntityState.Deleted;
                    }
                    else
                    {
                        note.IsTrash = true;
                        note.IsPin = false;
                        note.IsArchive = false;
                    }
                    NotesDB.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICollection<NoteModel> GetNotes(long UserID, bool IsTrash, bool IsArchieve)
        {
            try
            {
                responseNoteModels = NotesDB.Notes.Where(N => N.UserId.Equals(UserID) 
                && N.IsTrash == IsTrash && N.IsArchive == IsArchieve).OrderBy(N => N.CreatedOn).Select(N =>
                    new NoteModel
                    {
                        UserID = (long)N.UserId,
                        NoteID = N.NoteId,
                        Title = N.Title,
                        Text = N.Text,
                        ReminderOn = N.ReminderOn,
                        BackgroundColor = N.BackgroundColor,
                        BackgroundImage = N.BackgroundImage,
                        IsArchive = N.IsArchive,
                        IsPin = N.IsPin,
                        IsTrash = N.IsTrash,
                        Labels = N.NoteLabels.Select(N => N.Label.LabelName).ToList(),
                        Collaborators = N.Collaborators.Select(C => C.CollaboratorEmail ).ToList()
                    }
                    ).ToList();
                return responseNoteModels;
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
                responseNoteModels = NotesDB.Notes.Where(N => N.UserId.Equals(UserID)
                && N.IsTrash == false && N.ReminderOn > DateTime.Now).OrderBy(N => N.ReminderOn).Select(N =>
                    new NoteModel
                    {
                        UserID = (long)N.UserId,
                        NoteID = N.NoteId,
                        Title = N.Title,
                        Text = N.Text,
                        ReminderOn = N.ReminderOn,
                        BackgroundColor = N.BackgroundColor,
                        BackgroundImage = N.BackgroundImage,
                        IsArchive = N.IsArchive,
                        IsPin = N.IsPin,
                        IsTrash = N.IsTrash,
                        Labels = N.NoteLabels.Select(N => N.Label.LabelName).ToList(),
                        Collaborators = N.Collaborators.Select(C => C.CollaboratorEmail).ToList()
                    }
                    ).ToList();
                return responseNoteModels;
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
                var note = NotesDB.Notes.FirstOrDefault(N => N.NoteId == noteID && N.UserId == userID);
                if (note.IsPin)
                {
                    note.IsPin = false;
                }
                else
                {
                    note.IsPin = true;
                }
                NotesDB.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NoteModel UpdateNote(NoteModel Note)
        {
            try
            {
                if (NotesDB.NoteLabels.Any(n => n.NoteId == Note.NoteID && n.UserId == Note.UserID))
                {
                    NotesDB.NoteLabels.RemoveRange(NotesDB.NoteLabels.Where(N => N.NoteId == Note.NoteID));
                }
                NotesDB.SaveChanges();
                var UpdateNote = NotesDB.Notes.FirstOrDefault(N => N.NoteId == Note.NoteID && N.UserId == Note.UserID);
                if (UpdateNote != null)
                {
                    UpdateNote.Title = Note.Title;
                    UpdateNote.Text = Note.Text;
                    UpdateNote.ReminderOn = Note.ReminderOn;
                    UpdateNote.BackgroundColor = Note.BackgroundColor;
                    UpdateNote.BackgroundImage = Note.BackgroundImage;
                    UpdateNote.IsArchive = Note.IsArchive;
                    UpdateNote.IsPin = Note.IsPin;
                    UpdateNote.IsTrash = Note.IsTrash;
                }
                NotesDB.SaveChanges();
                if (Note.Labels != null)
                {
                    Note.Labels.ToList().ForEach(L =>
                    NotesDB.Set<Label>().AddIfNotExists(new Label { UserId = Note.UserID, LabelName = L }, x => x.LabelName.Equals(L)));
                    NotesDB.SaveChanges();
                    Note.Labels.ToList().ForEach(L =>
                    NotesDB.NoteLabels.Add(
                        new NoteLabel
                        {
                            UserId = Note.UserID,
                            NoteId = Note.NoteID,
                            LabelId = NotesDB.Labels.FirstOrDefault(a => a.LabelName == L).LabelId
                        })); ;
                }
                NotesDB.Collaborators.RemoveRange(NotesDB.Collaborators.Where(L => L.NoteId == Note.NoteID).ToList());
                NotesDB.SaveChanges();
                if (Note.Collaborators != null)
                {
                   Note.Collaborators.ToList().ForEach(C =>
                   NotesDB.Collaborators.Add(
                        new Collaborator { UserId = Note.UserID, NoteId = Note.NoteID, CollaboratorEmail = C }));
                    NotesDB.SaveChanges();
                }
                var NewResponseNote = NotesDB.Notes.Where(N => N.NoteId == Note.NoteID).Select(N =>
                    new NoteModel
                    {
                        UserID = (long)N.UserId,
                        NoteID = N.NoteId,
                        Title = N.Title,
                        Text = N.Text,
                        CreatedOn = N.CreatedOn,
                        ReminderOn = N.ReminderOn,
                        BackgroundColor = N.BackgroundColor,
                        BackgroundImage = N.BackgroundImage,
                        IsArchive = N.IsArchive,
                        IsPin = N.IsPin,
                        IsTrash = N.IsTrash,
                        Labels = N.NoteLabels.Select(N => N.Label.LabelName).ToList(),
                        Collaborators = N.Collaborators.Select(C => C.CollaboratorEmail).ToList()
                    }
                    ).ToList().First();
                return NewResponseNote;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
