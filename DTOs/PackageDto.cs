namespace LogisticsApp.DTOs;

public class PackageDto
{
    public int PackageId { get; set; }
    public string PackageName { get; set; }
    public string Amount { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}