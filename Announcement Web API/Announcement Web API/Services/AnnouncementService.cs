using Announcement_Web_API.DTO;
using Announcement_Web_API.Exceptions;
using Announcement_Web_API.Models;
using Announcement_Web_API.Repositories;
using Mapster;
using MapsterMapper;

namespace Announcement_Web_API.Services;

public interface IAnnouncementService
{
    Task <PagedResult<AnnouncementViewDto>> GetPagedAsync(PaginationRequest request);
    Task<AnnouncementDetailsDto> GetByIdDetailedAsync(int id);
    Task CreateAsync(AnnouncementCreateDto announcementCreateDto );
    Task UpdateAsync (int id, AnnouncementCreateDto announcementCreateDto);
    Task DeleteAsync(int id);

}

public class AnnouncementService: IAnnouncementService
{
    private readonly IAnnouncementRepository _repository;
    private readonly ILogger<AnnouncementService> _logger;

    public AnnouncementService(IAnnouncementRepository  repository, ILogger<AnnouncementService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PagedResult<AnnouncementViewDto>> GetPagedAsync(PaginationRequest request)
    {
        _logger.LogDebug("Starting GetPagedAsync: Page {PageNumber}, Size {PageSize}", 
            request.PageNumber, request.PageSize);
        
        var pagedResult = await _repository.GetPagedAsync(request);
        
        _logger.LogDebug("Repository returned {ItemCount} items out of {TotalCount} total", 
            pagedResult.Items.Count(), pagedResult.TotalCount);


        var result = new  PagedResult<AnnouncementViewDto>
        {
            Items = pagedResult.Items.Adapt<List<AnnouncementViewDto>>(),
            TotalCount = pagedResult.TotalCount,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize
        };
        
        _logger.LogInformation("Successfully retrieved page {PageNumber} with {ItemCount} announcements", 
            result.PageNumber, result.Items.Count);
        
        return result;
    }

    public async Task<AnnouncementDetailsDto> GetByIdDetailedAsync(int id)
    {
        _logger.LogDebug("Starting GetByIdDetailedAsync for announcement {AnnouncementId}", id);
                
        var announcement = await _repository.GetByIdAsync(id);
        
        if (announcement == null)
            throw new NotFoundException("Announcement Not Found");

        _logger.LogDebug("Found announcement {AnnouncementId} with title: '{Title}'", 
            id, announcement.Title);
        
        var searchText = $"{announcement.Title} {announcement.Description}";
        
        _logger.LogTrace("Searching for similar announcements using text: '{SearchText}'", searchText);

        
        var similarAnnouncements = await _repository.GetSimilarAnnouncementAsync(id, searchText);
        
        _logger.LogDebug("Found {SimilarCount} similar announcements for {AnnouncementId}", 
            similarAnnouncements.Count(), id);
        
        var announcementDetails = new AnnouncementDetailsDto
        {
            Title = announcement.Title,
            Description = announcement.Description,
            DateAdded = announcement.DateAdded,
            SimilarAnnouncements = similarAnnouncements
                .Select(a => a.Adapt<AnnouncementViewDto>())
                .ToList()
        };
        
        _logger.LogInformation("Successfully prepared detailed DTO for announcement {AnnouncementId} with {SimilarCount} similar items", 
            id, announcementDetails.SimilarAnnouncements.Count);
        
        return announcementDetails;
    }

    public async Task CreateAsync(AnnouncementCreateDto announcementCreateDto)
    {
        _logger.LogDebug("Starting CreateAsync for announcement with title: '{Title}'", 
            announcementCreateDto.Title);
        
        var newAnnouncement = new Announcement()
        {
            Title = announcementCreateDto.Title,
            Description = announcementCreateDto.Description
        };
        
        _logger.LogDebug("Created announcement entity, saving to repository");
        
        await _repository.CreateAsync(newAnnouncement);
        await _repository.SaveChangesAsync();
        
        _logger.LogInformation("Successfully created announcement with ID {AnnouncementId} and title: '{Title}'", 
            newAnnouncement.Id, newAnnouncement.Title);
    }
    
    public async Task UpdateAsync(int id, AnnouncementCreateDto announcementCreateDto)
    {
        _logger.LogDebug("Starting UpdateAsync for announcement {AnnouncementId}", id);
        
        var oldAnnouncement = await _repository.GetByIdTrackedAsync(id);
        
        if (oldAnnouncement == null)
            throw new NotFoundException("Announcement Not Found");
        
        _logger.LogDebug("Found announcement {AnnouncementId} for update. Old title: '{OldTitle}', New title: '{NewTitle}'", 
            id, oldAnnouncement.Title, announcementCreateDto.Title);
        
        oldAnnouncement.Title = announcementCreateDto.Title;
        oldAnnouncement.Description = announcementCreateDto.Description;
        
        _logger.LogDebug("Updated announcement properties, saving changes");
        
        await _repository.SaveChangesAsync();
        
        _logger.LogInformation("Successfully updated announcement {AnnouncementId} with new title: '{Title}'", 
            id, announcementCreateDto.Title);
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogDebug("Starting DeleteAsync for announcement {AnnouncementId}", id);
        
        var announcement = await _repository.GetByIdAsync(id);
        
        if (announcement == null)
            throw new NotFoundException("Announcement Not Found");
        
        _logger.LogDebug("Found announcement {AnnouncementId} with title: '{Title}', proceeding with deletion", 
            id, announcement.Title);
        
        _repository.Delete(announcement);
        
        await _repository.SaveChangesAsync();
        
        _logger.LogInformation("Successfully deleted announcement {AnnouncementId} with title: '{Title}'", 
            id, announcement.Title);
    }
}