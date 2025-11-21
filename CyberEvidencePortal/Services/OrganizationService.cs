using CyberEvidencePortal.Data;
using CyberEvidencePortal.DTOs;
using CyberEvidencePortal.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberEvidencePortal.Services
{
    public class OrganizationService
    {
        private readonly ApplicationDbContext _db;

        public OrganizationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Organization> CreateOrganizationAsync(CreateOrganizationDto dto)
        {
            var exists = await _db.Organizations
                .AnyAsync(o => o.Email == dto.Email || o.Oib == dto.Oib);

            if (exists)
                throw new InvalidOperationException("Organization with same OIB or email already exists.");

            var org = new Organization
            {
                Name = dto.Name,
                Oib = dto.Oib,
                Email = dto.Email,
                CreatedAt = DateTime.Now
            };

            _db.Organizations.Add(org);
            await _db.SaveChangesAsync();

            return org;
        }
    }
}
