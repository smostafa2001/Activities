using Activities.Domain;
using Activities.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Activities.Application.Tests;

public class CreateHandlerTests
{
    private readonly IMapper _mapper;
    private readonly DbContextOptions<DataContext> _dbOptions;

    public CreateHandlerTests()
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
    public async Task Handle_ReturnsSuccess_WhenActivityIsSaved()
    {
        using var context = new DataContext(_dbOptions);
        var handler = new Create.Handler(context, _mapper);

        var activity = new Activity
        {
            Id = Guid.NewGuid(),
            Title = "Test Activity",
            Description = "Test Description",
            Category = "Test Category",
            City = "Test City",
            Venue = "Test Venue"
        };

        var command = new Create.Command { Activity = activity };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(activity.Title, result.Value.Title);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenSaveChangesFails()
    {
        using var context = new DataContext(_dbOptions);
        var handler = new Create.Handler(context, _mapper);

        var activity = new Activity
        {
            Id = Guid.NewGuid(),
            Title = null!,
            Description = null!,
            Category = null!,
            City = null!,
            Venue = null!
        };

        var command = new Create.Command { Activity = activity };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to create activity", result.Error);
    }
}