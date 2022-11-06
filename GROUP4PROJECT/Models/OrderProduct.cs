using System;

namespace GROUP4PROJECT.Models
{
    public class OrderProduct:Core.Model
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
        public string Remarks { get; set; }
    }
}