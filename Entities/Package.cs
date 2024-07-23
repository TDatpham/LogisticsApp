using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace LogisticsApp.Entities;

public class Package
{
    [Key]
    public int PackageId { get; set; }
    [Required]
    public string PackageName { get; set;  }
    [Required]
    public string Amount { get; set; }
    [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
    [MaxLength(200, ErrorMessage = "Description cannot be more than 200 characters long.")]
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual ICollection<OrderPackage> OrderPackages  { get; set; }
    public Package()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        OrderPackages = new HashSet<OrderPackage>();
    }
}