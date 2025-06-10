using Announcement_Web_API.DTO;
using Announcement_Web_API.Models;
using Announcement_Web_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Announcement_Web_API.Controllers;

[ApiController]
[Route("announcement")]
public class AnnouncementController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;
    private readonly ILogger<AnnouncementController> _logger;

    public AnnouncementController(IAnnouncementService  announcementService,  ILogger<AnnouncementController> logger)
    {
        _announcementService = announcementService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<AnnouncementViewDto>>> GetAllPagedAsync([FromQuery] PaginationRequest request)
    {
        _logger.LogInformation("Retrieving paged announcements: Page {PageNumber}, Size {PageSize}", 
            request.PageNumber, request.PageSize);
        
        var announcements = await _announcementService.GetPagedAsync(request);

        _logger.LogInformation("Successfully retrieved {TotalCount} announcements", announcements.TotalCount);
        
        return Ok(announcements);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AnnouncementViewDto>> GetByIdDetailAsync(int id)
    {
        _logger.LogInformation("Retrieving detailed announcement with ID: {AnnouncementId}", id);

        var announcement = await _announcementService.GetByIdDetailedAsync(id);
        
        _logger.LogInformation("Successfully retrieved announcement {AnnouncementId}", id);

        return Ok(announcement);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAnnouncementAsync([FromBody] AnnouncementCreateDto announcementCreateDto)
    {
        _logger.LogInformation("Creating new announcement with title: '{Title}'", announcementCreateDto.Title);

        await _announcementService.CreateAsync(announcementCreateDto);
        
        _logger.LogInformation("Successfully created announcement");
        
        return Ok("Announcement created");
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] AnnouncementCreateDto announcementCreateDto)
    {
        _logger.LogInformation("Updating announcement with ID: {AnnouncementId}", id);
        
        await _announcementService.UpdateAsync(id, announcementCreateDto);
        
        _logger.LogInformation("Successfully updated announcement {AnnouncementId}", id);
        
        return Ok("Announcement updated");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        _logger.LogInformation("Deleting announcement with ID: {AnnouncementId}", id);
        
        await _announcementService.DeleteAsync(id);
        
        _logger.LogInformation("Successfully deleted announcement {AnnouncementId}", id);
        
        return Ok("Announcement deleted");
    }
}