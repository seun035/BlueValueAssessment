using BlueValueAssessment.Core.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlueValueAssessment.UIApi.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleApplicationExceptionErrors(context, ex);
            }
        }

        private async Task HandleApplicationExceptionErrors(HttpContext context, Exception exception)
        {
            object error = null;

            switch (exception)
            {
                case ValidationException ex:
                    context.Response.StatusCode = ex.StatusCode;
                    if (_env.IsDevelopment())
                    {
                        error = new { ex.StatusCode, Message = ex.FriendlyMessage, errors = ex.Errors };
                    }
                    else
                    {
                        error = new { ex.StatusCode, Message = ex.FriendlyMessage, errors = ex.Errors, ex.StackTrace};
                    }

                    _logger.Error($"Validation Exception. Ex: {JsonConvert.SerializeObject(ex)} || Errors: {JsonConvert.SerializeObject(error)}");
                    break;
                case AppException ex:
                    context.Response.StatusCode = ex.StatusCode;

                    if (_env.IsDevelopment())
                    {
                        error = new { ex.StatusCode, Message = ex.FriendlyMessage };
                    }
                    else
                    {
                        error = new { ex.StatusCode, Message = ex.FriendlyMessage, ex.StackTrace };
                    }

                    _logger.Error($"Application Exception. Ex: {JsonConvert.SerializeObject(ex)} || Errors: {JsonConvert.SerializeObject(error)}");
                    break;
                case Exception ex:
                    context.Response.StatusCode = 500;

                    if (_env.IsDevelopment())
                    {
                        error = new { StatusCode = 500, ex.Message};
                    }
                    else
                    {
                        error = new { StatusCode = 500, ex.Message, ex.StackTrace };
                    }

                    break;
            }

            context.Response.ContentType = "application/json";

            if (error != null)
            {
                var namingPolicy = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                };

                var result = System.Text.Json.JsonSerializer.Serialize(error, namingPolicy);
                await context.Response.WriteAsync(result);
            }
        }
    }
}
