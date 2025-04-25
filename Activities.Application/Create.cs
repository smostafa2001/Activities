using Activities.Application.Core;
using Activities.Domain;
using Activities.Persistence;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Activities.Application;

public class Create
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
        public async Task<Result<ActivityDTO>> Handle(
            Command request,
            CancellationToken token
        )
        {
            context.Activities.Add(request.Activity);

            bool result;
            try
            {
                result = await context.SaveChangesAsync(token) > 0;
            }
            catch (Exception)
            {
                result = false;
            }

            return result
                ? Result<ActivityDTO>.Success(mapper.Map(request.Activity, new ActivityDTO()))
                : Result<ActivityDTO>.Failure("Failed to create activity");
        }
    }
}