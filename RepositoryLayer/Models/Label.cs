using System;
using System.Collections.Generic;

#nullable disable

namespace RepositoryLayer.Models
{
    public partial class Label
    {
        public Label()
        {
            NoteLabels = new HashSet<NoteLabel>();
        }

        public long LabelId { get; set; }
        public long? UserId { get; set; }
        public string LabelName { get; set; }

        public virtual UserAccount User { get; set; }
        public virtual ICollection<NoteLabel> NoteLabels { get; set; }
    }
}
