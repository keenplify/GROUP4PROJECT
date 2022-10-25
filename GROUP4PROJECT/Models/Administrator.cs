using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROUP4PROJECT.Models
{
    public class Administrator
    {
        public string id { get; set; }
        public string created_at { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string full_name { get; set; }
    }
}