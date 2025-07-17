using BLL.Validator.Abstraction;
using System.ComponentModel.DataAnnotations;

namespace BLL.Validator.Implementation
{
    public class Validator<T> : IValidator<T>
    where T : class
    {
        private readonly IServiceProvider _provider;

        public Validator(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ValidationResult Validate(T model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, _provider, null);

            bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
                model, context, results, validateAllProperties: true
            );
            throw new Exception("Validation failed: " + string.Join(", ", results.Select(r => r.ErrorMessage)));
        }
    }

}
