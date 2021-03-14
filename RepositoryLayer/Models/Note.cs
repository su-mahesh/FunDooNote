using System;
using System.Collections.Generic;

#nullable disable

namespace RepositoryLayer.Models
{
    public partial class Note
    {
        public Note()
        {
            Collaborators = new HashSet<Collaborator>();
            NoteLabels = new HashSet<NoteLabel>();
        }

        public long NoteId { get; set; }
        public long? UserId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool IsPin { get; set; }
        public bool IsArchive { get; set; }
        public bool IsTrash { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ReminderOn { get; set; }
        public string BackgroundColor { get; set; }
        public byte[] BackgroundImage { get; set; }

        public virtual UserAccount User { get; set; }
        public virtual ICollection<Collaborator> Collaborators { get; set; }
        public virtual ICollection<NoteLabel> NoteLabels { get; set; }
    }
}
