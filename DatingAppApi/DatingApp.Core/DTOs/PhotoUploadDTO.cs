using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.DTOs
{
   public class PhotoUploadDTO
   {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string PublicId { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now; 
   }
}
