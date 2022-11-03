namespace GROUP4PROJECT.Models
{
    public class Order:Core.Model
    {
        public string CustomerName { get; set; }
        public OrderProduct[] OrderProducts { get; set; }
    }
}