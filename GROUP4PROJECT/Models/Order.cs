using System.Collections.Generic;

namespace GROUP4PROJECT.Models
{
    public class Order:Core.Model
    {
        public string CustomerName { get; set; }
        public IEnumerable<OrderProduct> OrderProducts { get; set; }
    }
}