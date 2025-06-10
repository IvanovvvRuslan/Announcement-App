namespace Announcement_Web_API.DTO;

public class AnnouncementDetailsDto
{
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public DateTime DateAdded { get; set; }
    
    public List<AnnouncementViewDto> SimilarAnnouncements { get; set; } =  new List<AnnouncementViewDto>();
}