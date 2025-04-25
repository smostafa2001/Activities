using Activities.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Activities.Application.Tests;

public class DeleteHandlerTests
{
    private readonly DbContextOptions<DataContext> _dbOptions;

    public DeleteHandlerTests() 
        => _dbOptions = new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenActivityIsDeleted()
    {
        var activityId = Guid.NewGuid();

        using (var context = new DataContext(_dbOptions))
        {
            context.Activities.Add(new()
            {
                Id = activityId,
                Title = "To Delete",
                Category = "To Delete Category",
                City = "To Delete City",
                Description = "To Delete Description",
                Venue = "To Delete Venue"
            });
            await context.SaveChangesAsync();
        }

        using (var context = new DataContext(_dbOptions))
        {
            var handler = new Delete.Handler(context);
            var command = new Delete.Command { Id = activityId };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenActivityNotFound()
    {
        using var context = new DataContext(_dbOptions);
        var handler = new Delete.Handler(context);

        var command = new Delete.Command { Id = Guid.NewGuid() };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Activity not found", result.Error);
    }
}