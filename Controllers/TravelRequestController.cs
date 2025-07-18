using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelRequests.Api.DTOs;
using TravelRequests.Api.Services;

namespace TravelRequests.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TravelRequestController : ControllerBase
    {
        private readonly ITravelRequestService _travelRequestService;
        
        public TravelRequestController(ITravelRequestService travelRequestService)
        {
            _travelRequestService = travelRequestService;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateTravelRequest([FromBody] CreateTravelRequestDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _travelRequestService.CreateTravelRequestAsync(dto, userId);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("my-requests")]
        public async Task<IActionResult> GetMyTravelRequests()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var requests = await _travelRequestService.GetMyTravelRequestsAsync(userId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("all")]
        [Authorize(Roles = "Aprobador")]
        public async Task<IActionResult> GetAllTravelRequests()
        {
            try
            {
                var requests = await _travelRequestService.GetAllTravelRequestsAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Aprobador")]
        public async Task<IActionResult> UpdateTravelRequestStatus(int id, [FromBody] UpdateTravelRequestStatusDto dto)
        {
            try
            {
                var result = await _travelRequestService.UpdateTravelRequestStatusAsync(id, dto);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}