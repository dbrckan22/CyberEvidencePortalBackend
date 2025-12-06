using CyberEvidencePortal.Data;
using CyberEvidencePortal.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CyberEvidencePortal.Services
{
    public class ComplianceService
    {
        private readonly ApplicationDbContext _db;
        public ComplianceService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            return await _db.Categories
                .OrderBy(c => c.CategoryId)
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<List<ObligationDto>> GetObligationsByCategoryAsync(int categoryId)
        {
            return await _db.Obligations
                .Where(o => o.CategoryId == categoryId)
                .Select(o => new ObligationDto
                {
                    ObligationId = o.ObligationId,
                    Code = o.Code,
                    Title = o.Title,
                    LegalReference = o.LegalReference,
                    ReviewIntervalMonths = o.ReviewIntervalMonths,
                    IsApplicable = o.IsApplicable,
                    Justification = o.Justification
                })
                .ToListAsync();
        }

        public async Task<ObligationDto?> GetObligationByIdAsync(int id)
        {
            return await _db.Obligations
                .Where(o => o.ObligationId == id)
                .Select(o => new ObligationDto
                {
                    ObligationId = o.ObligationId,
                    Code = o.Code,
                    Title = o.Title,
                    LegalReference = o.LegalReference,
                    ReviewIntervalMonths = o.ReviewIntervalMonths,
                    IsApplicable = o.IsApplicable,
                    Justification = o.Justification
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<CategoryWithObligationsDto>> GetCategoriesWithObligationsAsync()
        {
            return await _db.Categories
                .Include(c => c.Obligations)
                .OrderBy(c => c.CategoryId)
                .Select(c => new CategoryWithObligationsDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    Obligations = c.Obligations
                        .OrderBy(o => o.ObligationId)
                        .Select(o => new ObligationShortDto
                        {
                            ObligationId = o.ObligationId,
                            Title = o.Title
                        })
                        .ToList()
                })
                .ToListAsync();
        }
    }
}
