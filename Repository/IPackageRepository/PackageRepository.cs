using LogisticsApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using LogisticsApp.Data;

namespace LogisticsApp.Repository
{
    public class PackageRepository : IPackageRepository
    {
        private readonly LogisticsDbContext _context;

        public PackageRepository(LogisticsDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Package> GetAllPackages()
        {
            return _context.Packages.ToList();
        }

        public Package GetPackageById(Guid packageId)
        {
            return _context.Packages.Find(packageId);
        }

        public void CreatePackage(Package package)
        {
            _context.Packages.Add(package);
            _context.SaveChanges();
        }
        public void UpdatePackage(Package package)
        {
            _context.Packages.Update(package);
            _context.SaveChanges();
        }

        // Other methods like Add, Update, Delete can be implemented here
    }
}