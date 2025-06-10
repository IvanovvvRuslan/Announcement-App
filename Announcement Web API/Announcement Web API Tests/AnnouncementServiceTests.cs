using Announcement_Web_API.DTO;
using Announcement_Web_API.Exceptions;
using Announcement_Web_API.Models;
using Announcement_Web_API.Repositories;
using Announcement_Web_API.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Announcement_Web_API_Tests;

public class AnnouncementServiceTests
{
    private readonly IAnnouncementRepository _repository;
    private readonly IAnnouncementService _service;

    public AnnouncementServiceTests()
    {
        _repository =  Substitute.For<IAnnouncementRepository>();
        var logger = Substitute.For<ILogger<AnnouncementService>>();
        _service = new AnnouncementService(_repository, logger);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnPagedResult()
    {
        // Arrange
        var request = new PaginationRequest
        {
            PageNumber = 1,
            PageSize = 10
        };

        var repositoryResult = new PagedResult<Announcement>
        {
            Items = new List<Announcement>
            {
                new Announcement { Id = 1, Title = "Title 1" },
                new Announcement { Id = 2, Title = "Title 2" }
            },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        
        _repository.GetPagedAsync(request).Returns(repositoryResult);
        
        // Act
        var result = await _service.GetPagedAsync(request);
        
        // Assert
        Assert.Equal(repositoryResult.Items.Count, result.Items.Count);
        Assert.Equal(repositoryResult.TotalCount, result.TotalCount);
        Assert.Equal(repositoryResult.PageNumber, result.PageNumber);
        Assert.Equal(repositoryResult.PageSize, result.PageSize);
    }
    
    [Fact]
    public async Task GetPagedAsync_ShouldCallRepositoryWithCorrectRequest()
    {
        // Arrange
        var request = new PaginationRequest
        {
            PageNumber = 2, 
            PageSize = 5
        };
        
        var repositoryResult = new PagedResult<Announcement>
        {
            Items = new List<Announcement>()
        };
        
        _repository.GetPagedAsync(request).Returns(repositoryResult);
    
        // Act
        await _service.GetPagedAsync(request);
    
        // Assert
        await _repository.Received(1).GetPagedAsync(request);
    }
    
    [Fact]
    public async Task GetPagedAsync_WhenNoItems_ShouldReturnEmptyResult()
    {
        // Arrange
        var request = new PaginationRequest
        {
            PageNumber = 1, 
            PageSize = 10
        };
        var repositoryResult = new PagedResult<Announcement>
        {
            Items = new List<Announcement>(),
            TotalCount = 0,
            PageNumber = 1,
            PageSize = 10
        };
    
        _repository.GetPagedAsync(request).Returns(repositoryResult);
    
        // Act
        var result = await _service.GetPagedAsync(request);
    
        // Assert
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }
    
    [Fact]
    public async Task GetByIdDetailedAsync_ShouldCallRepositoryWithCorrectSearchText()
    {
        // Arrange
        var announcement = new Announcement
        {
            Id = 1, 
            Title = "Test", 
            Description = "Desc"
        };
        _repository.GetByIdAsync(announcement.Id).Returns(announcement);
    
        // Act
        await _service.GetByIdDetailedAsync(announcement.Id);
    
        // Assert
        await _repository.Received(1).GetSimilarAnnouncementAsync(announcement.Id, "Test Desc");
    }
    
    [Fact]
    public async Task GetByIdDetailedAsync_ShouldReturnWithSimilarAnnouncements()
    {
        // Arrange
        var announcement = new Announcement
        {
            Id = 1,
            Title = "Title",
            Description = "Description",
            DateAdded = DateTime.Now,
        };
        _repository.GetByIdAsync(announcement.Id).Returns(announcement);

        var similarAnnouncements = new List<Announcement>
        {
            new Announcement { Id = 2, Title = "Similar Title" },
            new Announcement { Id = 3, Description = "Similar Description" }
        };
        _repository.GetSimilarAnnouncementAsync(announcement.Id, "Title Description").Returns(similarAnnouncements);
        
        // Act
        var result = await _service.GetByIdDetailedAsync(announcement.Id);
        
        // Assert
        Assert.Equal("Title", result.Title);
        Assert.Equal("Description", result.Description);
        Assert.Equal("Similar Title", result.SimilarAnnouncements[0].Title);
        Assert.Equal("Similar Description", result.SimilarAnnouncements[1].Description);
    }

    [Fact]
    public async Task GetByIdDetailedAsync_ShouldThrowAnnouncementNotFoundException()
    {
        // Arrange
        var announcementId = 1;
        _repository.GetByIdAsync(announcementId).Returns((Announcement)null);
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdDetailedAsync(announcementId));
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateSuccessfully()
    {
        // Arrange
        var announcementCreateDto = new AnnouncementCreateDto
        {
            Title = "Test Title",
            Description = "Test Description",
        };
        
        // Act
        await _service.CreateAsync(announcementCreateDto);
        
        // Assert
        await _repository.Received(1).CreateAsync(Arg.Is<Announcement>(a => 
            a.Title == announcementCreateDto.Title &&
            a.Description ==  announcementCreateDto.Description));
        await _repository.Received(1).SaveChangesAsync();
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateSuccessfully()
    {
        // Arrange
        var existingAnnouncement = new Announcement
        {
            Id = 1,
            Title = "Test Title",
            Description = "Test Description",
            DateAdded = DateTime.Now,
        };
        
        _repository.GetByIdTrackedAsync(existingAnnouncement.Id).Returns(existingAnnouncement);

        var newAnnouncement = new AnnouncementCreateDto
        {
            Title = "New Announcement",
            Description = "New Description"
        };
        
        // Act
        await _service.UpdateAsync(existingAnnouncement.Id, newAnnouncement);
        
        // Assert
        await _repository.Received(1).SaveChangesAsync();
        
        Assert.Equal(existingAnnouncement.Title, newAnnouncement.Title);
        Assert.Equal(existingAnnouncement.Description, newAnnouncement.Description);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowAnnouncementNotFoundException()
    {
        // Arrange
        var announcementId = 1;
        _repository.GetByIdTrackedAsync(announcementId).Returns((Announcement)null);
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(announcementId, new AnnouncementCreateDto()));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteSuccessfully()
    {
        // Arrange
       var existingAnnouncement = new Announcement
        {
            Id = 1,
            Title = "Test Title",
            Description = "Test Description",
            DateAdded = DateTime.Now,
        };
        
        _repository.GetByIdAsync(existingAnnouncement.Id).Returns(existingAnnouncement);
        
        // Act
        await _service.DeleteAsync(existingAnnouncement.Id);
        
        // Assert
        _repository.Received(1).Delete(existingAnnouncement);
        await _repository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowAnnouncementNotFoundException()
    {
        // Arrange
        var announcementId = 1;
        _repository.GetByIdAsync(announcementId).Returns((Announcement)null);
        
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(announcementId));
    }
}