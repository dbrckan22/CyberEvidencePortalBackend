using CyberEvidencePortal.Models;

namespace CyberEvidencePortal.DTOs
{
    public class CreateUserDto
    {
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
