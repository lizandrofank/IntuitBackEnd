using ApiCliente.Models.Request;
using FluentValidation;
using FluentValidation.Results;
using Unipluss.Sign.Common.Validation;

namespace ApiCliente.Models
{
    public class ClienteValidator : AbstractValidator<ClienteRequest>
    {

        public ClienteValidator()
        {
            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.");

            RuleFor(c => c.Apellido)
                .NotEmpty().WithMessage("El apellido es obligatorio.");

            RuleFor(c => c.FechaNacimiento)
                .NotEmpty().WithMessage("La fecha de nacimiento es obligatoria.");

            RuleFor(c => c.Cuit)
                .NotEmpty().WithMessage("El CUIT es obligatorio.")
                .Matches(@"^\d{2}-\d{8}-\d{1}$").WithMessage("El CUIT debe tener el formato XX-XXXXXXXX-X.");

            RuleFor(c => c.Domicilio)
                .NotEmpty().WithMessage("El domicilio es obligatorio.");

            RuleFor(c => c.Telefono)
                .NotEmpty().WithMessage("El teléfono es obligatorio.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El email no tiene un formato válido.");
        }
    }

}
