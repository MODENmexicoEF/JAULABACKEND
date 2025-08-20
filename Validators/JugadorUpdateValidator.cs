using FluentValidation;
using JAULABACKEND.DTOs;

namespace JAULABACKEND.Validators
{
    public class JugadorUpdateValidator : AbstractValidator<JugadorUpdateDto>
    {
        public JugadorUpdateValidator() 
        {
            RuleFor(x => x.JugadorId).NotEmpty().WithMessage("El id es obligatorio");

            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es obligatorio");
            RuleFor(x => x.Nombre).Length(3, 20).WithMessage("El nombre debe tener 3 < x < 20 caracteres");

            RuleFor(x => x.ApellidoPaterno).NotEmpty().WithMessage("El Apellido paterno es obligatorio");
            RuleFor(x => x.ApellidoPaterno).Length(3, 20).WithMessage("El Apellido paterno debe tener 3 < x < 20 caracteres");

            RuleFor(x => x.ApellidoMaterno).NotEmpty().WithMessage("El Apellido Materno es obligatorio");
            RuleFor(x => x.ApellidoMaterno).Length(3, 20).WithMessage("El Apellido materno debe tener 3 < x < 20 caracteres");

            RuleFor(x => x.Edad)
                .GreaterThan(3)
                .LessThanOrEqualTo(80)
                .WithMessage("La edad debe estar entre 3 años y 80 años");

            RuleFor(x => x.Estatura)
                .GreaterThan(1.00m)
                .LessThanOrEqualTo(2.50m)
                .WithMessage("La estatura debe ser mayor a 1 < x < 2.50 metros");

            RuleFor(x => x.Peso)
                .GreaterThan(20.0m)
                .LessThanOrEqualTo(200.0m)
                .WithMessage("El peso debe ser mayor a 20 < x < 200 kilos");

            RuleFor(x => x.Habilidad)
                .GreaterThan(0)
                .LessThanOrEqualTo(100)
                .WithMessage("La habilidad debe ser 1 < x < 100");
        }
    }
}
