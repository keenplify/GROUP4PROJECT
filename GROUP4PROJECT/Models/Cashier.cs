using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROUP4PROJECT.Models
{
    public class Cashier: Core.Model
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}