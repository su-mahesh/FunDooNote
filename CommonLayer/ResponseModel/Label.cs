using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.ResponseModel
{
    public class Label
    {
        public long? UserId { get; set; }
        public long LabelId { get; set; }
        public string LabelName { get; set; }
    }
}
