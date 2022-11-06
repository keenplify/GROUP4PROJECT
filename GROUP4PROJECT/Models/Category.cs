using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GROUP4PROJECT.Models
{
    public class Category:Core.Model
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<Models.Product> Products { get; set; }
    }
}