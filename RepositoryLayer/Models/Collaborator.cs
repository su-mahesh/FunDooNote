using System;
using System.Collections.Generic;

#nullable disable

namespace RepositoryLayer.Models
{
    public partial class Collaborator
    {
        public long CollaboratorId { get; set; }
        public long? UserId { get; set; }
        public long? NoteId { get; set; }
        public string CollaboratorEmail { get; set; }

        public virtual Note Note { get; set; }
        public virtual UserAccount User { get; set; }
    }
}
