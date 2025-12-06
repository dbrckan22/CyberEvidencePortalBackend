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
    }
}
