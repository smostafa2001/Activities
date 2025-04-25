using Activities.Application.Core;
using Activities.Persistence;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Activities.Application;

public class Details
{
    public class Query : IRequest<Result<ActivityDTO>>
    {
        public Guid Id { get; set; }
    }

    public class Handler(
        DataContext context, 
        IMapper mapper
    ) : IRequestHandler<Query, Result<ActivityDTO>>
    {
        public async Task<Result<ActivityDTO>> Handle(
            Query request, 
            CancellationToken token
        )
        {
            var activity = await context.Activities
                .ProjectTo<ActivityDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(a => a.Id == request.Id, token);

            return activity is null 
                ? Result<ActivityDTO>.Failure("Activity not found") 
                : Result<ActivityDTO>.Success(activity);
        }
    }
}