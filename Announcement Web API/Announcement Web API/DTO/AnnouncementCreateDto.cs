using System.ComponentModel.DataAnnotations;

namespace Announcement_Web_API.DTO;

public class AnnouncementCreateDto
{
    public string Title { get; set; }
    
    public string Description { get; set; }
}