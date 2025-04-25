using Activities.Domain;
using Activities.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Activities.Application.Tests;

public class DetailsHandlerTests
{
    private readonly IMapper _mapper;
    private readonly DbContextOptions<DataContext> _dbOptions;

    public DetailsHandlerTests()
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
    public async Task Handle_ReturnsActivity_WhenFound()
    {
        var activityId = Guid.NewGuid();

        using (var context = new DataContext(_dbOptions))
        {
            context.Activities.Add(new()
            {
                Id = activityId,
                Title = "Details Test",
                Category = "Details Category",
                City = "Details City",
                Description = "Details Description",
                Venue = "Details Venue"
            });
            await context.SaveChangesAsync();
        }

        using (var context = new DataContext(_dbOptions))
        {
            var handler = new Details.Handler(context, _mapper);
            var query = new Details.Query { Id = activityId };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal("Details Test", result?.Value?.Title);
        }
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenNotFound()
    {
        using var context = new DataContext(_dbOptions);
        var handler = new Details.Handler(context, _mapper);

        var query = new Details.Query { Id = Guid.NewGuid() };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Activity not found", result.Error);
    }
}