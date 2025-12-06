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

        [HttpGet("obligation/{obligationId}")]
        public async Task<IActionResult> GetEvidenceForObligation(int obligationId, int organizationId)
        {
            var items = await _evidenceService.GetEvidenceForObligationAsync(obligationId, organizationId);
            return Ok(items);
        }

        [HttpGet("download/{evidenceId}")]
        public async Task<IActionResult> DownloadEvidence(int evidenceId, int organizationId)
        {
            try
            {
                var (fileData, fileName, contentType) =
                    await _evidenceService.DownloadEvidenceAsync(evidenceId, organizationId);

                return File(fileData, contentType, fileName);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (FileNotFoundException)
            {
                return NotFound(new { message = "File not found" });
            }
        }

        [HttpDelete("{evidenceId}")]
        public async Task<IActionResult> DeleteEvidence(int evidenceId, int organizationId)
        {
            try
            {
                var deleted = await _evidenceService.DeleteEvidenceAsync(evidenceId, organizationId);

                if (!deleted)
                    return NotFound(new { message = "Evidence not found" });

                return Ok(new { message = "Evidence deleted successfully" });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

    }
}
