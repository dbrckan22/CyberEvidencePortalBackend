using CyberEvidencePortal.Data;
using CyberEvidencePortal.DTOs;
using CyberEvidencePortal.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CyberEvidencePortal.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _db;

        public UserService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<User> CreateUserAsync(CreateUserDto dto)
        {
            var orgExists = await _db.Organizations
                .AnyAsync(o => o.OrganizationId == dto.OrganizationId);

            if (!orgExists)
                throw new InvalidOperationException("Organization does not exist.");

            var userExists = await _db.Users
                .AnyAsync(u => u.Email == dto.Email);

            if (userExists)
                throw new InvalidOperationException("User with this email already exists.");

            var user = new User
            {
                OrganizationId = dto.OrganizationId,
                Name = dto.Name,
                Email = dto.Email,
                Password = HashPassword(dto.Password),
                Role = dto.Role.ToString(),
                CreatedAt = DateTime.Now
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return user;
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
