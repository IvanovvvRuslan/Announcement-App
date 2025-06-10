using Announcement_Web_API.Data;
using Announcement_Web_API.DTO;
using Announcement_Web_API.Models;
using Announcement_Web_API.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Announcement_Web_API.Repositories;

public interface IAnnouncementRepository
{
    Task<PagedResult<Announcement>> GetPagedAsync(PaginationRequest request);
    Task<Announcement?> GetByIdAsync(int? id);
    Task<Announcement?> GetByIdTrackedAsync(int? id);
    Task CreateAsync(Announcement announcement);
    Task Delete(Announcement announcement);
    Task SaveChangesAsync();
    Task<IEnumerable<Announcement>> GetSimilarAnnouncementAsync(int excludeId, string searchText);
}

public class AnnouncementRepository:  IAnnouncementRepository
{
    private readonly ApplicationDbContext _context;

    public AnnouncementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Announcement>> GetPagedAsync(PaginationRequest request)
    {
        var totalCount = await _context.Announcements.CountAsync();

        var items = await _context.Announcements
            .AsNoTracking()
            .OrderByDescending(a => a.DateAdded)
            .Skip(request.Skip)
            .Take(request.PageSize)
            .ToListAsync();

        return new PagedResult<Announcement>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<Announcement?> GetByIdAsync(int? id)
    {
        var announcement = await _context.Announcements
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);

        return announcement;
    }

    public async Task<Announcement?> GetByIdTrackedAsync(int? id)
    {
        var announcement = await _context.Announcements
            .FirstOrDefaultAsync(a => a.Id == id);

        return announcement;
    }

    public async Task CreateAsync(Announcement announcement)
    {
        await _context.Announcements.AddAsync(announcement);
    }

    public Task Delete(Announcement announcement)
    {
        _context.Announcements.Remove(announcement);
        
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Announcement>> GetSimilarAnnouncementAsync(int excludeId, string searchText)
    {
        var searchTerms = TextSearchHelper.PrepareSearchTerms(searchText);
        
        if (!searchTerms.Any())
            return new List<Announcement>();

        var containsQuery = string.Join(" OR ", searchTerms.Select(term => $"\"{term}\""));
        
        var candidates = await _context.Announcements
            .FromSqlRaw(@"
                SELECT * FROM Announcements 
                WHERE CONTAINS((Title, Description), {0})
                AND Id <> {1}
                ORDER BY DateAdded DESC", 
                containsQuery, excludeId)
            .AsNoTracking()
            .ToListAsync();

        var rankedResults = candidates
            .Select(announcement => new
            {
                Announcement = announcement,
                MatchCount = TextSearchHelper.CountMatches(searchTerms, announcement.Title + " " + announcement.Description)
            })
            .OrderByDescending(x => x.MatchCount)
            .Take(3)
            .OrderByDescending(x => x.Announcement.DateAdded)
            .Select(x => x.Announcement);

        return rankedResults;
    }
}