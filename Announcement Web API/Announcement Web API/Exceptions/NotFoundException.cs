namespace Announcement_Web_API.Exceptions;

public class NotFoundException: Exception
{
    public NotFoundException(string message) : base(message) {}
}