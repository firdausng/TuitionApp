using FluentValidation;

namespace TuitionApp.Core.Features.Locations
{
    public class CreateLocationItemCommandValidator : AbstractValidator<CreateLocationItemCommand>
    {
        public CreateLocationItemCommandValidator()
        {
            RuleFor(command => command.Address).NotNull().NotEmpty();
            RuleFor(command => command.ClosingTime).NotNull().NotEmpty();
            RuleFor(command => command.OpeningTime).NotNull().NotEmpty();
            RuleFor(command => command.Description).NotNull().NotEmpty();
            RuleFor(command => command.InstructorLists).NotNull().NotEmpty();
            RuleFor(command => command.IsEnabled).NotNull().NotEmpty();
            RuleFor(command => command.Name).NotNull().NotEmpty();
        }
    }
}
