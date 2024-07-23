using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LogisticsApp.Data;
using Microsoft.Data.SqlClient.DataClassification;

namespace LogisticsApp.Entities;

public class Order
{
    [Key]
    public int OrderId { get; set; }
    [ForeignKey("ApplicationUser")]
    public string UserId { get; set; }
    [ForeignKey("StatusOrder")]
    public int StatusOrderId { get; set; }
    public string PackageId { get; set; }
    [Required]
    public DateTime CreateOrderDate { get; set; }
    [Required]
    public string PickupLocation { get; set; }
    [Required]
    public string DeliveryLocation { get; set; }
    [Required]
    public string RecipientName { get; set; }
        [Required]
    public string RecipientPhoneNumber { get; set; }
    [Required]
    public string RecipientEmail { get; set; }
    [Required]
    public string RecipientAddress { get; set; }
    [Required]
    public string RecipientCity { get; set; }
    [Required]
    public string RecipientCountry { get; set; }
    [Required]
    public string RecipientPostalCode { get; set; }
    [Required]
    public DateTime ExpectedDeliveryDate { get; set; }
    [Required]
    public DateTime DeliveryDateEstimated { get; set; }
    [Required]
    public DateTime ActualDeliveryDate { get; set; }
    public string Description { get; set; }
    [Required]
    public double TotalAmount { get; set; }
    [Required]
    public int Quantity { get; set; }
    
    public virtual StatusOrder StatusOrder { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
    public  virtual ICollection<OrderTracking> OrderTrackings { get; set; }
    public virtual ICollection<OrderPackage> OrderPackages { get; set; }

    public Order()
    {
        CreateOrderDate = DateTime.Now;
        OrderTrackings = new HashSet<OrderTracking>();
        OrderPackages = new HashSet<OrderPackage>();
    }
}