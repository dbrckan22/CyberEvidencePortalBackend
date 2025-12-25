using CyberEvidencePortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace CyberEvidencePortal.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ComplianceController : ControllerBase
    {
        private readonly ComplianceService _service;

        public ComplianceController(ComplianceService service)
        {
            _service = service;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _service.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("categories/{id}/obligations")]
        public async Task<IActionResult> GetObligationsByCategory(int id)
        {
            var obligations = await _service.GetObligationsByCategoryAsync(id);

            if (!obligations.Any())
                return NotFound(new { message = "No obligations found for this category" });

            return Ok(obligations);
        }

        [HttpGet("obligations/{id}")]
        public async Task<IActionResult> GetObligation(int id)
        {
            var obligation = await _service.GetObligationByIdAsync(id);

            if (obligation == null)
                return NotFound(new { message = "Obligation not found" });

            return Ok(obligation);
        }

        [HttpGet("compliance/tree")]
        public async Task<IActionResult> GetComplianceTree()
        {
            var result = await _service.GetCategoriesWithObligationsAsync();
            return Ok(result);
        }

        [HttpGet("compliance/summary")]
        public async Task<IActionResult> GetComplianceSummary(int organizationId)
        {
            var summary = await _service.GetSummaryAsync(organizationId);
            return Ok(summary);
        }

        [HttpGet("compliance/categories")]
        public async Task<IActionResult> GetComplianceByCategory(int organizationId)
        {
            var data = await _service.GetComplianceByCategoryAsync(organizationId);
            return Ok(data);
        }

    }
}
