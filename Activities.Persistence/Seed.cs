using Activities.Domain;

namespace Activities.Persistence;

public class Seed
{
    public static async Task SeedDataAsync(DataContext context)
    {
        if (!context.Activities.Any())
        {
            List<Activity> activities =
            [
                new()
                {
                    Title = "Past Activity 1",
                    Date = DateTime.UtcNow.AddMonths(-2),
                    Description = "Activity 2 months ago",
                    Category = "drinks",
                    City = "London",
                    Venue = "Pub",
                },
                new()
                {
                    Title = "Past Activity 2",
                    Date = DateTime.UtcNow.AddMonths(-1),
                    Description = "Activity 1 month ago",
                    Category = "culture",
                    City = "Paris",
                    Venue = "The Louvre",
                },
                new()
                {
                    Title = "Future Activity 1",
                    Date = DateTime.UtcNow.AddMonths(1),
                    Description = "Activity 1 month in future",
                    Category = "music",
                    City = "London",
                    Venue = "Wembly Stadium",
                },
                new()
                {
                    Title = "Future Activity 2",
                    Date = DateTime.UtcNow.AddMonths(2),
                    Description = "Activity 2 months in future",
                    Category = "food",
                    City = "London",
                    Venue = "Jamies Italian",
                },
                new()
                {
                    Title = "Future Activity 3",
                    Date = DateTime.UtcNow.AddMonths(3),
                    Description = "Activity 3 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Pub",
                },
                new()
                {
                    Title = "Future Activity 4",
                    Date = DateTime.UtcNow.AddMonths(4),
                    Description = "Activity 4 months in future",
                    Category = "culture",
                    City = "London",
                    Venue = "British Museum",
                },
                new()
                {
                    Title = "Future Activity 5",
                    Date = DateTime.UtcNow.AddMonths(5),
                    Description = "Activity 5 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Punch and Judy",
                },
                new()
                {
                    Title = "Future Activity 6",
                    Date = DateTime.UtcNow.AddMonths(6),
                    Description = "Activity 6 months in future",
                    Category = "music",
                    City = "London",
                    Venue = "O2 Arena",
                },
                new()
                {
                    Title = "Future Activity 7",
                    Date = DateTime.UtcNow.AddMonths(7),
                    Description = "Activity 7 months in future",
                    Category = "travel",
                    City = "Berlin",
                    Venue = "All",
                },
                new()
                {
                    Title = "Future Activity 8",
                    Date = DateTime.UtcNow.AddMonths(8),
                    Description = "Activity 8 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Pub",
                }
            ];

            await context.Activities.AddRangeAsync(activities);
            await context.SaveChangesAsync();
        }
    }
}