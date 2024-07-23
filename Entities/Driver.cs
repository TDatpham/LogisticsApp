using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LogisticsApp.Data;
namespace LogisticsApp.Entities;

public class Driver{
    [Key]
    public int DriverId { get; set; }
    [ForeignKey("ApplicationUser")]
    public string UserId { get; set; }
    public string LicenseNumber { get; set; }
    public string VehicleNumber { get; set; }
    public string VehicleType { get; set; }
    public string VehicleModel { get; set; }
    public string VehicleColor { get; set; }
    public bool IsActive { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; }
}