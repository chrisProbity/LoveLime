﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.DTOs
{
    public class ProfileUpdateDTO
    {
        public string Introduction { get; set; }
        public string Interest { get; set; }
        public string LookingFor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
