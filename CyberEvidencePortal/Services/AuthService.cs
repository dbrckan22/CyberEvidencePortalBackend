using CyberEvidencePortal.Data;
using CyberEvidencePortal.DTOs;
using CyberEvidencePortal.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CyberEvidencePortal.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _db;

        public AuthService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<User?> Login(LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return null;

            if (!VerifyPassword(dto.Password, user.Password))
                return null;

            return user;
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private bool VerifyPassword(string input, string hashed)
        {
            return HashPassword(input) == hashed;
        }
    }
}
