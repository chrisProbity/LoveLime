using DatingApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.DTOs
{
    public class UserDetailDTO
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string UserName { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public int Age { get; set; }
        public string Interest { get; set; }
        public string LookingFor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotoDTO> Photos { get; set; } = new List<PhotoDTO>();


    }
}
