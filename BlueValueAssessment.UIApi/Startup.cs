using BlueValueAssessment.Core.Configs;
using BlueValueAssessment.Core.Data;
using BlueValueAssessment.Core.Models;
using BlueValueAssessment.Core.Services;
using BlueValueAssessment.Data;
using BlueValueAssessment.Data.Repositories;
using BlueValueAssessment.DomainServices.Movies;
using BlueValueAssessment.DomainServices.Requests;
using BlueValueAssessment.DomainServices.Utilities;
using BlueValueAssessment.UIApi.Dtos;
using BlueValueAssessment.UIApi.Middlewares;
using BlueValueAssessment.UIApi.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueValueAssessment.UIApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(MongoDbContext));
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IUtilities, Utility>();
            services.AddScoped<IMoviesService, MoviesService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<Validations.IValidatorFactory, ValidatorFactory>();
            services.AddScoped<IValidator<SearchQueryDto>, MovieSearchValidator>();
            services.AddScoped<IValidator<string>, DDMMYYYValidator>();
            services.AddScoped<IValidator<DateRange>, DateRangeValidator>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(Log.Logger);
            services.AddScoped<IIPAccessor, IPAccessor>();
            services.Configure<OmdbConfig>(Configuration.GetSection("OmdbSettings"));
            services.AddHttpClient();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlueValueChallenge Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlueValueChallenge Api V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
