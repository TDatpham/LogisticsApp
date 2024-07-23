using LogisticsApp.DTOs;
using LogisticsApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsApp.Controllers
{
    [ApiController]
    [Route("api/package")]
    [AllowAnonymous]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PackageDto>> GetPackages()
        {
            try
            {
                var packages = _packageService.GetAllPackages();
                return Ok(packages);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpGet("{idPackage}")]
        public ActionResult<PackageDto> GetPackageById(Guid idPackage)
        {
            try
            {
                var package = _packageService.GetPackageById(idPackage);
                if (package == null)
                {
                    return NotFound();
                }

                return Ok(package);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpPost]
        public ActionResult<PackageDto> CreatePackage([FromBody] CreatePackageDto createPackageDto)
        {
            try
            {
                var createdPackage = _packageService.CreatePackage(createPackageDto);
                return CreatedAtAction(nameof(GetPackages), new { id = createdPackage.PackageId }, createdPackage);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpPut("{idPackage}")]
        public ActionResult<PackageDto> UpdatePackage(Guid idPackage, [FromBody] UpdatePackageDto updatePackageDto)
        {
            try
            {
                var updatedPackage = _packageService.UpdatePackage(idPackage, updatePackageDto);
                if (updatedPackage == null)
                {
                    return NotFound();
                }

                return Ok(updatedPackage);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}