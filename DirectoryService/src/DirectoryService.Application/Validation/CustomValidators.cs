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

    public static IRuleBuilderOptions<T, IEnumerable<TElement>> AllUnique<T, TElement>(
        this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder)
    {
        return ruleBuilder
            .Must(collection =>
            {
                var list = collection?.ToList();
                if (list is null || list.Count == 0) return true;
                return list.ToHashSet().Count == list.Count;
            });
    }
    
    public static IRuleBuilderOptions<T, IEnumerable<TElement>> AllUniqueByItem<T, TElement, TKey>(
        this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder,  Func<TElement, TKey> itemSelector)
    {
        return ruleBuilder
            .Must(colection =>
            {
                var list = colection?.ToList();
                if (list is null || list.Count == 0) return true;

                return list.Select(itemSelector).ToHashSet().Count == list.Count;
            });
    }
    
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, Failure error)
    {
        return rule.WithMessage(error.Serialize());
    }
    
}