using CyberEvidencePortal.Data;
using CyberEvidencePortal.DTOs;
using CyberEvidencePortal.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberEvidencePortal.Services
{
    public class EvidenceService
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public EvidenceService(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<Evidence> UploadEvidenceAsync(EvidenceUploadDto dto)
        {
            var obligation = await _db.Obligations
                .Include(o => o.Category)
                .FirstOrDefaultAsync(o => o.ObligationId == dto.ObligationId);

            var user = await _db.Users
                .Include(u => u.Organization)
                .FirstOrDefaultAsync(u => u.UserId == dto.UserId);

            if (obligation == null || user == null)
                throw new InvalidOperationException("Obligation or User not found.");

            string? filePath = null;

            if (dto.File != null)
            {
                string basePath = Path.Combine(_env.ContentRootPath, "Documentation");

                string orgFolder = Path.Combine(basePath,
                    CleanName(user.Organization.Name));

                string categoryFolder = Path.Combine(orgFolder,
                    CleanName(obligation.Category.Name));

                string obligationFolder = Path.Combine(categoryFolder,
                    CleanName(obligation.Title));

                Directory.CreateDirectory(obligationFolder);

                string fileName = $"{Guid.NewGuid()}_{CleanName(dto.File.FileName)}";
                filePath = Path.Combine(obligationFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.File.CopyToAsync(stream);
            }

            var evidence = new Evidence
            {
                ObligationId = dto.ObligationId,
                AddedBy = dto.UserId,
                EvidenceType = dto.EvidenceType,
                Title = dto.Title,
                FilePath = filePath,
                LinkUrl = dto.LinkUrl,
                Note = dto.Note,
                ValidFrom = dto.ValidFrom,
                ValidUntil = dto.ValidUntil,
                CreatedAt = DateTime.Now,
                Status = "Active"
            };

            _db.Evidence.Add(evidence);
            await _db.SaveChangesAsync();

            return evidence;
        }

        private string CleanName(string input)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                input = input.Replace(c, '_');

            return input;
        }

        public async Task<List<EvidenceViewDto>> GetEvidenceForObligationAsync(int obligationId, int organizationId)
        {
            return await _db.Evidence
                .Include(e => e.User)
                .Where(e => e.ObligationId == obligationId &&
                            e.User.OrganizationId == organizationId)
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new EvidenceViewDto
                {
                    EvidenceId = e.EvidenceId,
                    Title = e.Title,
                    EvidenceType = e.EvidenceType,
                    FileName = Path.GetFileName(e.FilePath),
                    ValidFrom = e.ValidFrom,
                    ValidUntil = e.ValidUntil,
                    CreatedAt = e.CreatedAt,
                    Status = e.Status,
                    AddedByUser = e.User.Name,
                    Note = e.Note
                })
                .ToListAsync();
        }

        public async Task<(byte[] fileData, string fileName, string contentType)> DownloadEvidenceAsync(int evidenceId, int organizationId)
        {
            var evidence = await _db.Evidence
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.EvidenceId == evidenceId);

            if (evidence == null || evidence.User.OrganizationId != organizationId)
                throw new UnauthorizedAccessException("Not allowed");

            if (string.IsNullOrEmpty(evidence.FilePath) || !File.Exists(evidence.FilePath))
                throw new FileNotFoundException("File not found");

            var bytes = await File.ReadAllBytesAsync(evidence.FilePath);
            var fileName = Path.GetFileName(evidence.FilePath);
            var contentType = "application/octet-stream";

            return (bytes, fileName, contentType);
        }

        public async Task<bool> DeleteEvidenceAsync(int evidenceId, int organizationId)
        {
            var evidence = await _db.Evidence
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.EvidenceId == evidenceId);

            if (evidence == null)
                return false;

            if (evidence.User.OrganizationId != organizationId)
                throw new UnauthorizedAccessException("Not allowed");

            if (!string.IsNullOrEmpty(evidence.FilePath) && File.Exists(evidence.FilePath))
                File.Delete(evidence.FilePath);

            _db.Evidence.Remove(evidence);
            await _db.SaveChangesAsync();

            return true;
        }


    }
}
