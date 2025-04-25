using Activities.Domain;
using Activities.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Activities.Application.Tests;

public class EditHandlerTests
{
    private readonly IMapper _mapper;
    private readonly DbContextOptions<DataContext> _dbOptions;

    public EditHandlerTests()
    {
        var config = new MapperConfiguration(
            cfg => cfg.CreateMap<Activity, ActivityDTO>()
        );

        _mapper = config.CreateMapper();

        _dbOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenActivityIsUpdated()
    {
        var activityId = Guid.NewGuid();

        using var context = new DataContext(_dbOptions);

        var activity = new Activity
        {
            Id = activityId,
            Title = "Old Title",
            Description = "Old Description",
            Category = "Old Category",
            City = "Old City",
            Venue = "Old Venue",
            Date = DateTime.UtcNow,
        };

        context.Activities.Add(activity);
        await context.SaveChangesAsync();

        var handler = new Edit.Handler(context, _mapper);

        activity.Title = "New Title";
        activity.Description = "Updated Description";
        activity.Category = "Updated Category";
        activity.City = "Updated City";
        activity.Venue = "Updated Venue";
        activity.Date = DateTime.UtcNow.AddDays(1);

        var command = new Edit.Command { Activity = activity };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("New Title", result.Value.Title);
        Assert.Equal("Updated Description", result.Value.Description);

        var updatedActivity = await context.Activities.FindAsync(activityId);
        Assert.NotNull(updatedActivity);
        Assert.Equal("New Title", updatedActivity.Title);
        Assert.Equal("Updated Description", updatedActivity.Description);
        Assert.Equal("Updated Category", updatedActivity.Category);
        Assert.Equal("Updated City", updatedActivity.City);
        Assert.Equal("Updated Venue", updatedActivity.Venue);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenActivityNotFound()
    {
        using var context = new DataContext(_dbOptions);
        var handler = new Edit.Handler(context, _mapper);

        var command = new Edit.Command
        {
            Activity = new Activity
            {
                Id = Guid.NewGuid(),
                Title = "Ghost Activity",
                Description = "Should fail",
                Category = "Ghost Category",
                City = "Ghost City",
                Venue = "Ghost Venue"
            }
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to update activity", result.Error);
    }
}