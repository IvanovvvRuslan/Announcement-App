using System.ComponentModel.DataAnnotations;

namespace Announcement_Web_API.Models;

public class Announcement
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public DateTime DateAdded { get; set; } =  DateTime.UtcNow;
}