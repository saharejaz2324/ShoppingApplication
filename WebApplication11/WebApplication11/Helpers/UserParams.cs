using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApplication.API.Helpers
{
    public class UserParams
    {

        // setting the default values of the user parameters 

        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
            // If a user sends a pageSize 1000, it will simply send back the pageSize 50
            // because we define the page Size
        }

        public int UserId { get; set; }
        public string OrderBy { get; set; } = "lastActive";
        public bool Likees { get; set; } = false;
        public bool Likers { get; set; } = false;



    }
}
