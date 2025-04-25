using Activities.Application;
using Activities.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Activities.WebAPI.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetActivitiesAsync(
        [FromQuery] ActivityParams @params,
        CancellationToken token
    ) => HandlePagedResult(
        await Mediator.Send(
            new List.Query { Params = @params },
            token
        )
    );

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivityAsync(
        Guid id,
        CancellationToken token
    ) => HandleResult(
        await Mediator.Send(
            new Details.Query { Id = id },
            token
        )
    );

    [HttpPost]
    public async Task<IActionResult> CreateActivityAsync(
        Activity activity,
        CancellationToken token
    ) => HandleResult(
        await Mediator.Send(
            new Create.Command { Activity = activity },
            token
        )
    );

    [HttpPut("{id}")]
    public async Task<IActionResult> EditActivityAsync(
        Guid id,
        Activity activity,
        CancellationToken token
    )
    {
        activity.Id = id;
        return HandleResult(
            await Mediator.Send(
                new Edit.Command { Activity = activity },
                token
            )
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivityAsync(
        Guid id,
        CancellationToken token
    ) => HandleResult(
        await Mediator.Send(
            new Delete.Command { Id = id },
            token
        )
    );
}