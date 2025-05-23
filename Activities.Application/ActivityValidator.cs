﻿using Activities.Domain;
using FluentValidation;

namespace Activities.Application;

public class ActivityValidator : AbstractValidator<Activity>
{
    public ActivityValidator()
    {
        RuleFor(a => a.Title).NotEmpty();
        RuleFor(a => a.Description).NotEmpty();
        RuleFor(a => a.Date).NotEmpty();
        RuleFor(a => a.Category).NotEmpty();
        RuleFor(a => a.City).NotEmpty();
        RuleFor(a => a.Venue).NotEmpty();
    }
}