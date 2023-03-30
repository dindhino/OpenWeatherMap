using FluentValidation;
using FluentValidation.Results;

namespace OpenWeatherMap.Services.Models
{
    public abstract class DataModel
    {
        public abstract class Validator<T> : AbstractValidator<T> where T : DataModel
        {
            protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
                => context.InstanceToValidate != null && base.PreValidate(context, result);
        }
    }
}
