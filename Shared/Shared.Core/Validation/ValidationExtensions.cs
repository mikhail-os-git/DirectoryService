using CSharpFunctionalExtensions;
using FluentValidation.Results;
using Shared.Kernal.Failures;

namespace Shared.Core.Validation;

public static class ValidationExtensions
{
    public static FailList ToFailList(this ValidationResult result)
    {
        List<ValidationFailure> resultErrors = result.Errors;
        var errors = resultErrors.Select(e => Failure.Deserialize(e.ErrorMessage)).ToList();
        return new FailList(errors);
    }
}