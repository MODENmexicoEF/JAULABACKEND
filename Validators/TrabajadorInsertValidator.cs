using FluentValidation;
using JAULABACKEND.DTOs;

namespace JAULABACKEND.Validators
{
    public class TrabajadorInsertValidator : AbstractValidator<TrabajadorInsertDto>
    {
        public TrabajadorInsertValidator() 
        {

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

            RuleFor(x => x.Salario)
                .GreaterThan(1.00m)
                .LessThanOrEqualTo(10000000.00m)
                .WithMessage("El salario debe ser mayor a 1 < x < 10000000");
        }
    }
}
