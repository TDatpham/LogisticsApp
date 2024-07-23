using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsApp.Entities;

public class Payment
{
    [Key] 
    public int PaymentId { get; set; }
    [Required] 
    public int Amount { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime Timestamp { get; set; }
    public string TransactionId { get; set; }
    [ForeignKey("Order")] 
    public int OrderId; 
    public virtual Order Order { get; set; }

}