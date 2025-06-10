using Announcement_Web_API.DTO;
using FluentValidation;

namespace Announcement_Web_API.Validators;

public class AnnouncementCreateDtoValidator:  AbstractValidator<AnnouncementCreateDto>
{
    public AnnouncementCreateDtoValidator()
    {
        RuleFor(a => a.Title).NotEmpty().WithMessage("Title cannot be empty")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");
        
        RuleFor(a => a.Description).NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
    }
}