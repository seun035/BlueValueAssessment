using BlueValueAssessment.UIApi.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueValueAssessment.UIApi.Validations
{
    public class MovieSearchValidator: AbstractValidator<SearchQueryDto>
    {
        public MovieSearchValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
        }
    }
}
