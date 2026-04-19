using System.Text.Json;
using CSharpFunctionalExtensions;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder, Func<TElement, Result<TValueObject, Failure>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            Result<TValueObject, Failure> result = factoryMethod.Invoke(value);
            if (result.IsSuccess)
                return;

            context.AddFailure(result.Error.Serialize());

        });
    }

    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, Failure error)
    {
        return rule.WithMessage(error.Serialize());
    }
}