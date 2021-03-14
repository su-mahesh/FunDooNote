using System;
using System.Collections.Generic;

#nullable disable

namespace RepositoryLayer.Models
{
    public partial class UserAccount
    {
        public UserAccount()
        {
            Collaborators = new HashSet<Collaborator>();
            Labels = new HashSet<Label>();
            NoteLabels = new HashSet<NoteLabel>();
            Notes = new HashSet<Note>();
        }

        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Collaborator> Collaborators { get; set; }
        public virtual ICollection<Label> Labels { get; set; }
        public virtual ICollection<NoteLabel> NoteLabels { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}
