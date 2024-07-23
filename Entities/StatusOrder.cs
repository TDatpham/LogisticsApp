using System.ComponentModel.DataAnnotations;

namespace LogisticsApp.Entities;

public class StatusOrder
{
    [Key]
    public int StatusOrderId { get; set; }
    [Required]
    public string StatusName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Order> Orders { get; set; }
    public StatusOrder()
    {
        Orders = new HashSet<Order>();
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
}