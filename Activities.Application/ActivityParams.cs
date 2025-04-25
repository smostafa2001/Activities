using Activities.Application.Core;

namespace Activities.Application;

public class ActivityParams : PagingParams
{
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
}
