﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.DTOs
{
    public class PhotoForReturnDTO
    {

        public int ID { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }

    }
}
