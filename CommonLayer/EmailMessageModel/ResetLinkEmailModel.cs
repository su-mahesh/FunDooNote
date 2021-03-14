using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.EmailMessageModel
{
    public class ResetLinkEmail
    {
        public string Email { get; set; }
        public string JwtToken { get; set; }
    }
}
