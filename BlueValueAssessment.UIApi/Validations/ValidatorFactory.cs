using BlueValueAssessment.Core.Exceptions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueValueAssessment.UIApi.Validations
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task ValidateAsync<TModel>(TModel model)
        {
            var validator = CreateValidator<TModel>();

            if (validator == null)
            {
                throw new InvalidOperationException($"No validator found for {typeof(TModel).Name}");
            }

            var result = await (validator as IValidator<TModel>).ValidateAsync(model);

            if (!result.IsValid)
            {
                IDictionary<string, string> errors = new Dictionary<string, string>();

                foreach (var item in result.Errors)
                {
                    errors.Add(item.PropertyName, item.ErrorMessage);
                }

                throw new Core.Exceptions.ValidationException(errors: errors);
            }
        }

        private IValidator CreateValidator<TModel>()
        {
            IValidator<TModel> validator;

            var result = _serviceProvider.GetRequiredService(typeof(IValidator<TModel>));

            if (result != null)
            {
                validator = result as IValidator<TModel>;
                return validator;
            }

            return null;
        }
    }

    public interface IValidatorFactory
    {
        Task ValidateAsync<TModel>(TModel model);
    }
}
