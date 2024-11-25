using Carter;
using FluentValidation;
using MediatR;
using MiniApp.Api.Middlewares;
using MiniApp.Api.Pipelines;
using MiniApp.Api.Profiles;
using MiniApp.Application.Features.EmployeeFeatures.Queries;
using MiniApp.Application.Features.EmployeeFeatures.Validators;
using MiniApp.Persistence.Abstruct;
using MiniApp.Persistence.Concrete;
using MiniApp.Persistence.IProviders;
using MiniApp.Persistence.Providers;
using Newtonsoft.Json;

namespace MiniApp.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddCarter();
            services.AddMediatR(typeof(GetEmployeeSummaryQuery).Assembly);
            services.AddAutoMapper(typeof(EmployeeProfile).Assembly);

            services.AddScoped<ISeedProvider, SeedProvider>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            services.AddValidatorsFromAssembly(typeof(GetEmployeeSummaryQueryValidator).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddTransient<ExceptionHandlingMiddleware>();

            services.Configure<JsonSerializerSettings>(options =>
            {
                options.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddProblemDetails();

        }
    }
}
