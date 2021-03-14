﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.RequestModel
{
    public class ForgetPasswordModel
    {
        [Required]
        public string Email { get; set; }

        public string JwtToken { get; set; }
    }
}
