using Garage.Data;
using System.ComponentModel.DataAnnotations;

namespace Garage.Annotations
{
    public class UniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var dbContext = validationContext.GetService<GarageContext>();

            if (dbContext == null)
                throw new InvalidOperationException("ApplicationDbContext not available");

            var personalId = value.ToString();

            var exists = dbContext.Users
                .Any(u => u.PersonalID == personalId);

            if (exists)
            {
                return new ValidationResult(ErrorMessage ?? "Personal ID already exists.");
            }

            return ValidationResult.Success;
        }
    }
}
