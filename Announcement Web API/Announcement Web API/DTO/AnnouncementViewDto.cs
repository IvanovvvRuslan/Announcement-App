namespace Announcement_Web_API.DTO;

public class AnnouncementViewDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    public string Description { get; set; }

    public DateTime DateAdded { get; set; }
}