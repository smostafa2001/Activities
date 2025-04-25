using Activities.Domain;
using Activities.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Activities.Application.Tests;

public class ListHandlerTests
{
    private readonly IMapper _mapper;
    private readonly DbContextOptions<DataContext> _dbOptions;

    public ListHandlerTests()
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
    public async Task Handle_ReturnsPagedListOfActivities()
    {
        using var context = new DataContext(_dbOptions);
        var startDate = DateTime.UtcNow;

        for (int i = 1; i <= 10; i++)
        {
            context.Activities.Add(new()
            {
                Id = Guid.NewGuid(),
                Title = $"Activity {i}",
                Category = $"Category Activity {i}",
                City = $"City Activity {i}",
                Description = $"Description Activity {i}",
                Venue = $"Venue Activity {i}",
                Date = startDate.AddDays(i)
            });
        }

        await context.SaveChangesAsync();

        var handler = new List.Handler(context, _mapper);

        var query = new List.Query
        {
            Params = new()
            {
                PageNumber = 1,
                PageSize = 5,
                StartDate = startDate
            }
        };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Value.Count);
        Assert.All(
            result.Value, 
            item => Assert.Contains("Activity", item.Title)
        );
    }
}