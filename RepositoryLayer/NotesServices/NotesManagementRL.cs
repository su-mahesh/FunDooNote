using System;
using System.Collections.Generic;
using System.Linq;
using CommonLayer.RequestModel;
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

        public ICollection<NoteModel> GetActiveUserNotes(long UserID)
        {
            try
            {
                responseNoteModels = NotesDB.Notes.Where(N => N.UserId.Equals(UserID)
                && N.IsTrash == false && N.IsArchive == false).Select(N =>
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
    }
}
