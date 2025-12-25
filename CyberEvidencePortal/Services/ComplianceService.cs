using CyberEvidencePortal.Data;
using CyberEvidencePortal.DTOs;
using CyberEvidencePortal.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberEvidencePortal.Services;

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

    private ComplianceStatus GetEvidenceStatus(Evidence e)
    {
        if (e.ValidUntil == null)
            return ComplianceStatus.Valid;

        var today = DateTime.UtcNow.Date;

        if (e.ValidUntil < today)
            return ComplianceStatus.Expired;

        if (e.ValidUntil < today.AddDays(30))
            return ComplianceStatus.ExpiringSoon;

        return ComplianceStatus.Valid;
    }


    private ComplianceStatus GetObligationStatus(IEnumerable<Evidence> evidence)
    {
        if (!evidence.Any())
            return ComplianceStatus.Missing;

        var statuses = evidence.Select(GetEvidenceStatus).ToList();

        if (statuses.Contains(ComplianceStatus.Valid))
            return ComplianceStatus.Valid;

        if (statuses.Contains(ComplianceStatus.ExpiringSoon))
            return ComplianceStatus.ExpiringSoon;

        if (statuses.Contains(ComplianceStatus.Expired))
            return ComplianceStatus.Expired;

        return ComplianceStatus.Missing;
    }

    double Percent(int part, int total)
    {
        if (total == 0) return 0;
        return Math.Round((double)part / total * 100, 0);
    }

    public async Task<List<CategoryComplianceDto>> GetComplianceByCategoryAsync(int organizationId)
    {
        var categories = await _db.Categories
            .Include(c => c.Obligations)
                .ThenInclude(o => o.EvidenceItems)
                    .ThenInclude(e => e.User)
            .ToListAsync();

        return categories.Select(category =>
        {
            var obligationStatuses = category.Obligations.Select(o =>
            {
                var evidence = o.EvidenceItems
                    .Where(e => e.User.OrganizationId == organizationId)
                    .ToList();

                return new ObligationStatusDto
                {
                    ObligationId = o.ObligationId,
                    Title = o.Title,
                    EvidenceCount = evidence.Count,
                    Status = GetObligationStatus(evidence)
                };
            }).ToList();

            var total = obligationStatuses.Count;
            var covered = obligationStatuses.Count(o => o.Status != ComplianceStatus.Missing);

            return new CategoryComplianceDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.Name,
                Obligations = obligationStatuses,
                Summary = new ComplianceSummaryDto
                {
                    TotalObligations = total,
                    CoveredObligations = covered,
                    CoveragePercent = Percent(covered, total),

                    Valid = obligationStatuses.Count(o => o.Status == ComplianceStatus.Valid),
                    ExpiringSoon = obligationStatuses.Count(o => o.Status == ComplianceStatus.ExpiringSoon),
                    Expired = obligationStatuses.Count(o => o.Status == ComplianceStatus.Expired),
                    Missing = obligationStatuses.Count(o => o.Status == ComplianceStatus.Missing)
                }
            };
        }).ToList();
    }

    public async Task<ComplianceSummaryDto> GetSummaryAsync(int organizationId)
    {
        var obligations = await _db.Obligations
            .Include(o => o.EvidenceItems)
                .ThenInclude(e => e.User)
            .ToListAsync();

        var statuses = obligations.Select(o =>
            GetObligationStatus(
                o.EvidenceItems.Where(e => e.User.OrganizationId == organizationId)
            )
        ).ToList();

        var total = statuses.Count;
        var covered = statuses.Count(s => s != ComplianceStatus.Missing);

        return new ComplianceSummaryDto
        {
            TotalObligations = total,
            CoveredObligations = covered,
            CoveragePercent = Percent(covered, total),

            Valid = statuses.Count(s => s == ComplianceStatus.Valid),
            ExpiringSoon = statuses.Count(s => s == ComplianceStatus.ExpiringSoon),
            Expired = statuses.Count(s => s == ComplianceStatus.Expired),
            Missing = statuses.Count(s => s == ComplianceStatus.Missing)
        };
    }
}
