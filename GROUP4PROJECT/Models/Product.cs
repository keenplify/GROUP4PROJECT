using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROUP4PROJECT.Models
{
    public class Product:Core.Model
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
    }
}