using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniApp.Application.Features.EmployeeFeatures.Filters;
using MiniApp.Application.Features.EmployeeFeatures.Queries;
using MiniApp.Application.Features.ProjectFeatures.Filters;

namespace MiniApp.Api.Modules
{
    public class EmployeeModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var endPoints = app.MapGroup("/Employee").WithTags("Employee");

            endPoints.MapPost("/EmployeeSummary", async (IMediator mediator, [FromBody] GetEmployeeSummaryQueryFilter filter) =>
            {
                var result = await mediator.Send(new GetEmployeeSummaryQuery(filter));
                return TypedResults.Ok(result);
            }).WithOpenApi();
        }
    }
}
