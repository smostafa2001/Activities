using Activities.Application.Core;
using Activities.Domain;
using Activities.Persistence;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Activities.Application;

public class Edit
{
    public class Command : IRequest<Result<ActivityDTO>>
    {
        public required Activity Activity { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() 
            => RuleFor(a => a.Activity).SetValidator(new ActivityValidator());
    }

    public class Handler(
        DataContext context,
        IMapper mapper
    ) : IRequestHandler<Command, Result<ActivityDTO>>
    {
        public async Task<Result<ActivityDTO>> Handle(Command request, CancellationToken token)
        {
            var activity = await context.Activities.FindAsync(
                [request.Activity.Id],
                token
            );

            mapper.Map(request.Activity, activity);

            var result = await context.SaveChangesAsync(token) > 0;

            return result
                ? Result<ActivityDTO>.Success(
                    mapper.Map(activity, new ActivityDTO())
                )
                : Result<ActivityDTO>.Failure("Failed to update activity");
        }
    }
}