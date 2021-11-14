using BlueValueAssessment.Core.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueValueAssessment.UIApi.Validations
{
    public class DateRangeValidator: AbstractValidator<DateRange>
    {
        public DateRangeValidator()
        {
            RuleFor(x => x).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty();
            When(x => x.StartDate != default && x.EndDate != default, () => {
                RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate);
            });
            
        }
    }
}
