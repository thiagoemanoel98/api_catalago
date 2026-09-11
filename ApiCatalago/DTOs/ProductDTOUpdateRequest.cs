using System.ComponentModel.DataAnnotations;

namespace ApiCatalago.DTOs;

public class ProductDTOUpdateRequest : IValidatableObject
{
    [Range(1, 9999, ErrorMessage = "Estoque deve ta entre 1 e 9999")]
    public int Stock { get; set; }
    
    public DateTime RegistrationDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (RegistrationDate.Date <= DateTime.Now.Date)
        {
            yield return new  ("A data deve ser maior do que a data atual", 
                new[] {nameof(this.RegistrationDate)});
        }
    }
}