using System;
using System.Linq;
using ContractControlCentre.DB.Interfaces;
using ContractControlCentre.Validation.Messages;
using ContractControlCentre.Validation.Models;
using ContractControlCentre.Validation.Models.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace ContractControlCentre.Validation.Services
{
    internal sealed class ValidationService : FluentValidationService<ValidationModel>, IVValidationService
    {
        private readonly IPersonRepository _repository;

        public ValidationService(IPersonRepository repository)
        {
            this._repository = repository;


            ///check first name on valid
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Имя не должно быть пустым")
                .WithErrorCode("EXP-1.1");

            RuleFor(x => x.FirstName)
                .MinimumLength(3)
                .MaximumLength(20)
                .WithMessage("Имя должно быть в рамках допустимой длинны")
                .WithErrorCode("EXP-1.1.2");

            RuleFor(x => x.FirstName).Custom((s, context) =>
            {
                if (s.Contains("1234567890-'^*~@/!#$%&") == true)
                {
                    context.AddFailure(new ValidationFailure(nameof(ValidationModel.FirstName), "Имя не должно включать числа или спец.символы")
                    {
                        ErrorCode = "EXP-1.1.3"
                    });
                }
            });


            ///check last name on valid
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Фамилия не должна быть пустой")
                .WithErrorCode("EXP-1.2");

            RuleFor(x => x.LastName)
                .MinimumLength(3)
                .MaximumLength(25)
                .WithMessage("Фамилия должна быть в рамках допустимой длинны")
                .WithErrorCode("EXP-1.2.2");

            RuleFor(x => x.LastName).Custom((s, context) =>
            {
                if (s.Contains("1234567890-'^*~@/!#$%&") == true )
                {
                    context.AddFailure(new ValidationFailure(nameof(ValidationModel.LastName), "Фамилия не должна включать числа или спец.символы")
                    {
                        ErrorCode = "EXP-1.2.3"
                    });
                }
            });


            ///check age on valid
            RuleFor(x => x.Age)
                .NotEmpty()
                .NotNull()
                .WithMessage("Возраст не должен быть пустым")
                .WithErrorCode("EXP-1.3");

            RuleFor(x => x.Age)
                .LessThan(150)
                .GreaterThan(12)
                .WithMessage("Возраст должен быть в рамках допустимых значений")
                .WithErrorCode("EXP-1.3.2");


            ///check email on valid
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Почта не должна быть пустой")
                .WithErrorCode("EXP-1.4");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Почта должна соответствовать стандартам")
                .WithErrorCode("EXP-1.4.2");

            RuleFor(x => x.Email).Custom((s, context) =>
            {
                if (_repository.EmailCheck(s))
                {
                    context.AddFailure(new ValidationFailure(nameof(ValidationModel.Email), "Почтовый адрес уже зарегистрирован")
                    {
                        ErrorCode = "EXP-1.4.3"
                    });
                }
            });

        }

    }
}

