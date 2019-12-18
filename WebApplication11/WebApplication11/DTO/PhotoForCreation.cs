using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApplication.API.DTO
{
    public class PhotoForCreation
    {
        public string url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }


        public PhotoForCreation()
        {
            DateAdded = DateTime.Today;
        }

    }
}
