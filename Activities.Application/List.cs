using Activities.Application.Core;
using Activities.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;

namespace Activities.Application;

public class List
{
    public class Query : IRequest<Result<PagedList<ActivityDTO>>>
    {
        public required ActivityParams Params { get; set; }
    }

    public class Handler(
        DataContext context,
        IMapper mapper
    ) : IRequestHandler<Query, Result<PagedList<ActivityDTO>>>
    {
        public async Task<Result<PagedList<ActivityDTO>>> Handle(
            Query request,
            CancellationToken token
        )
        {
            var query = context.Activities
                .Where(a => a.Date >= request.Params.StartDate)
                .OrderBy(a => a.Date)
                .ProjectTo<ActivityDTO>(mapper.ConfigurationProvider)
                .AsQueryable();

            return Result<PagedList<ActivityDTO>>.Success(
                await PagedList<ActivityDTO>.CreateAsync(
                    query,
                    request.Params.PageNumber,
                    request.Params.PageSize,
                    token
                )
            );
        }
    }
}