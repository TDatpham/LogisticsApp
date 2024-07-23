using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HostingEnvironmentExtensions = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions;

namespace LogisticsApp.Entities;

public class Notification
{
    [Key]
    public int NotificationId { get; set; }
    [ForeignKey("DriverId")]
    public int DriverId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public DateTime NotificationTime { get; set; }
    public bool IsRead { get; set; }
    public bool IsDeleted { get; set; }
    public virtual Driver Driver { get; set; }
    
}