using CyberEvidencePortal.DTOs;
using CyberEvidencePortal.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CyberEvidencePortal.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class OrganizationsController : ControllerBase
    {
        private readonly OrganizationService _orgService;

        public OrganizationsController(OrganizationService orgService)
        {
            _orgService = orgService;
        }

        [HttpPost("register/organization")]
        public async Task<IActionResult> CreateOrganization(CreateOrganizationDto dto)
        {
            try
            {
                var org = await _orgService.CreateOrganizationAsync(dto);
                return Ok(org);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

      
    }
}
