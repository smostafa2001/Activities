using Activities.Application.Core;
using Activities.Persistence;
using MediatR;

namespace Activities.Application;
public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken token)
        {
            var activity = await context.Activities.FindAsync([request.Id], token);

            if (activity is null) 
                return Result<Unit>.Failure("Activity not found");

            context.Remove(activity);

            bool result = await context.SaveChangesAsync(token) > 0;

            return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Failed to delete the activity");
        }
    }
}