using System.ComponentModel.DataAnnotations;

namespace Garage.Annotations
{
    public class NotEqualAttribute : ValidationAttribute
    {
        private readonly string _otherProperty;

        public NotEqualAttribute(string otherProperty)
        {
            _otherProperty = otherProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherProperty);

            if (otherPropertyInfo == null)
                return new ValidationResult($"Unknown property: {_otherProperty}");

            var otherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

            if (value != null && value.Equals(otherValue))
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be different from {_otherProperty}");

            return ValidationResult.Success;
        }
    }
}
