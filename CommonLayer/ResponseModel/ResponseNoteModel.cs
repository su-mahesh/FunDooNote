using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.RequestModel
{
    public class ResponseNoteModel
    {
        public long UserID { get; set; }
        public long NoteID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool IsPin { get; set; }
        public bool IsTrash { get; set; }
        public bool IsArchive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ReminderOn { get; set; }
        public string BackgroundColor { get; set; }
        public byte[] BackgroundImage { get; set; }
        public ICollection<string> Labels { get; set; }
        public ICollection<string> Collaborators { get; set; }
    }
}
