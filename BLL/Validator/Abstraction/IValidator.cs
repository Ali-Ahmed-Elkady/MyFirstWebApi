using System.ComponentModel.DataAnnotations;

namespace BLL.Validator.Abstraction
{
    public interface IValidator<T>
    {
        ValidationResult Validate(T model);
    }
}
