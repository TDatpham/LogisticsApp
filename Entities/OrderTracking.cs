using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsApp.Entities;

public class OrderTracking
{
    [Key]
    public int OrderTrackingId { get; set; }

    [ForeignKey("Order")]
    public int OrderId { get; set; }
    public string CurrentLocation { get; set; }
    public DateTime StatusUpdateTime { get; set; }
    
    public virtual Order Order { get; set; }
    public OrderTracking()
    {
        StatusUpdateTime = DateTime.Now; 
    }
}