using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsApp.Entities;

public class OrderPackage
{
    [Key, Column(Order = 0)]
    [ForeignKey("Order")]
    public int OrderId { get; set; }
    public Order Order { get; set; }

    [Key, Column(Order = 1)]
    [ForeignKey("Package")]
    public int PackageId { get; set; }
    public Package Package { get; set; }
}