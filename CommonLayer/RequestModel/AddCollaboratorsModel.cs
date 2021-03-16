using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.RequestModel
{
    public class AddCollaboratorsModel
    {
        public long UserID { get; set; }
        public long NoteID { get; set; }
        public ICollection<string> Collaborators { get; set; }
    }
}
