using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace TravelInspiration.API.Shared.Behaviours;

public class ModelValidationBehaviour<TRequest, TResult>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResult> where TRequest : IRequest<TResult>
{
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        List<ValidationResult> validationResults = validators.Select(v => v.Validate(context)).ToList();
        var groupedValidationFailures = validationResults.SelectMany(validationResult => validationResult.Errors)
                                                                                       .GroupBy(validationFailure => validationFailure.PropertyName)
                                                                                       .Select(grouping => new { PropertyName = grouping.Key, ValidationFailures = grouping.Select(validationFailure => new { validationFailure.ErrorMessage })
                                                                                       }).ToList();

        if (groupedValidationFailures.Count == 0) return await next(cancellationToken);
        {
            var validationProblemsDictionary = new Dictionary<string, string[]>();
            foreach (var group in groupedValidationFailures)
            {
                IEnumerable<string> errorMessages = group.ValidationFailures.Select(v => v.ErrorMessage);
                validationProblemsDictionary.Add(group.PropertyName, errorMessages.ToArray());
            }

            return (TResult)Results.ValidationProblem(validationProblemsDictionary);
        }
    }
}
