using LogisticsApp.DTOs;

namespace LogisticsApp.Services
{
    public interface IPackageService
    {
        IEnumerable<PackageDto> GetAllPackages();
        PackageDto GetPackageById(Guid packageId);
        PackageDto CreatePackage(CreatePackageDto createPackageDto);
        PackageDto UpdatePackage(Guid packageId, UpdatePackageDto updatePackageDto);
    }
}