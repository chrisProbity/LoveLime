using DatingApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.DTOs
{
    public class UserListDTO
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public int Age { get; set; }
        public string  Username { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string PhotoUrl { get; set; }
        public string City { get; set; }
        public  string Country { get; set; }
    }
}
