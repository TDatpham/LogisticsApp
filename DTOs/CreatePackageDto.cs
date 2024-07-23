namespace LogisticsApp.DTOs;

public class CreatePackageDto
{
    public string PackageName { get; set; }
    public string Amount { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
}