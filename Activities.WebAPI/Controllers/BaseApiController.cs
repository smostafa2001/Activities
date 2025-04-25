using Activities.Application.Core;
using Activities.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Activities.WebAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator Mediator 
        => _mediator ??= HttpContext
        .RequestServices
        .GetRequiredService<IMediator>();

    protected ActionResult HandleResult<T>(Result<T>? result)
        => result is null
            ? NotFound()
            : result.IsSuccess && result.Value is not null
                ? Ok(result.Value)
                : result.IsSuccess && result.Value is null
                    ? NotFound()
                    : BadRequest(result.Error);

    protected ActionResult HandlePagedResult<T>(Result<PagedList<T>>? result)
    {
        if (result is null) return NotFound();
        if (result.IsSuccess && result.Value is not null)
        {
            Response.AddPaginationHeader(
                result.Value.CurrentPage,
                result.Value.PageSize,
                result.Value.TotalCount,
                result.Value.TotalPages
            );

            return Ok(result.Value);
        }

        return result.IsSuccess && result.Value is null
            ? NotFound()
            : BadRequest(result.Error);
    }
}