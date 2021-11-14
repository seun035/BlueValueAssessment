using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueValueAssessment.UIApi.Validations
{
    public class DDMMYYYValidator: AbstractValidator<string>
    {
        public DDMMYYYValidator()
        {
            RuleFor(x => x).NotEmpty();
            RuleFor(x => x).Custom((dateString, context) => {
                try
                {
                    var result = DateTime.ParseExact(dateString, "dd-MM-yyyy", null);
                }
                catch (FormatException ex)
                {
                    context.AddFailure($"Invalid '{context.PropertyName}' format. Valid date format: dd-MM-YYYY ");
                }
            });
        }
    }
}
