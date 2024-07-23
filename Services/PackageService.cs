using LogisticsApp.DTOs;
using LogisticsApp.Entities;
using LogisticsApp.Repository;

namespace LogisticsApp.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;

        public PackageService(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        public IEnumerable<PackageDto> GetAllPackages()
        {
            var packages = _packageRepository.GetAllPackages();
            return packages.Select(p => new PackageDto
            {
                PackageName = p.PackageName,
                Amount = p.Amount,
                ImageUrl = p.ImageUrl,
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            });
        }

        public PackageDto GetPackageById(Guid packageId)
        {
            var package = _packageRepository.GetPackageById(packageId);
            return new PackageDto
            {
                PackageId = package.PackageId,
                PackageName = package.PackageName,
                Amount = package.Amount,
                ImageUrl = package.ImageUrl,
                Description = package.Description,
                CreatedAt = package.CreatedAt,
                UpdatedAt = package.UpdatedAt
            };
        }

        public PackageDto CreatePackage(CreatePackageDto createPackageDto)
        {
            var package = new Package
            {
                PackageName = createPackageDto.PackageName,
                Amount = createPackageDto.Amount,
                ImageUrl = createPackageDto.ImageUrl,
                Description = createPackageDto.Description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _packageRepository.CreatePackage(package);

            return new PackageDto
            {
                PackageId = package.PackageId,
                PackageName = package.PackageName,
                Amount = package.Amount,
                ImageUrl = package.ImageUrl,
                Description = package.Description,
                CreatedAt = package.CreatedAt,
                UpdatedAt = package.UpdatedAt
            };
        }

        public PackageDto UpdatePackage(Guid packageId, UpdatePackageDto updatePackageDto)
        {
            var package = _packageRepository.GetPackageById(packageId);
            package.PackageName = updatePackageDto.PackageName;
            package.Amount = updatePackageDto.Amount;
            package.ImageUrl = updatePackageDto.ImageUrl;
            package.Description = updatePackageDto.Description;
            package.UpdatedAt = DateTime.Now;

            _packageRepository.UpdatePackage(package);

            return new PackageDto
            {
                PackageId = package.PackageId,
                PackageName = package.PackageName,
                Amount = package.Amount,
                ImageUrl = package.ImageUrl,
                Description = package.Description,
                CreatedAt = package.CreatedAt,
                UpdatedAt = package.UpdatedAt
            };
        }
    }
}
