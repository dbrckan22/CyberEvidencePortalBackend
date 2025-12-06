using CyberEvidencePortal.DTOs;
using CyberEvidencePortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace CyberEvidencePortal.Controllers
{
    [ApiController]
    [Route("api/v1/evidence")]
    public class EvidenceController : ControllerBase
    {
        private readonly EvidenceService _evidenceService;

        public EvidenceController(EvidenceService service)
        {
            _evidenceService = service;
        }

        [HttpPost("evidence/upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadEvidence([FromForm] EvidenceUploadDto dto)
        {
            try
            {
                var evidence = await _evidenceService.UploadEvidenceAsync(dto);
                return Ok(evidence);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
