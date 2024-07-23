using LogisticsApp.Entities;
using System.Collections.Generic;

namespace LogisticsApp.Repository
{
    public interface IPackageRepository
    {
        IEnumerable<Package> GetAllPackages();
        Package GetPackageById(Guid packageId);

        // Other methods like Add, Update, Delete can be added here
        void CreatePackage(Package package);
        void UpdatePackage(Package package);

    }
}