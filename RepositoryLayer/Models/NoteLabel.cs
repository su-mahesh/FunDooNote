using System;
using System.Collections.Generic;

#nullable disable

namespace RepositoryLayer.Models
{
    public partial class NoteLabel
    {
        public long NoteLabelId { get; set; }
        public long? NoteId { get; set; }
        public long? LabelId { get; set; }
        public long? UserId { get; set; }

        public virtual Label Label { get; set; }
        public virtual Note Note { get; set; }
        public virtual UserAccount User { get; set; }
    }
}
