using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniApp.Application.Features.ProjectFeatures.Filters;
using MiniApp.Application.Features.ProjectFeatures.Queries;
using MiniApp.Application.Features.TimeSheetFeatures.Filters;

namespace MiniApp.Api.Modules
{
    public class ProjectModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var endPoints = app.MapGroup("/Project").WithTags("Project");

            endPoints.MapPost("/ProjectHourBarChart", async (IMediator mediator, [FromBody] GetProjectHourBarChartQueryFilter filter) =>
            {
                var result = await mediator.Send(new GetProjectHourBarChartQuery(filter));
                return TypedResults.Ok(result);
            }).WithOpenApi();
        }
    }
}
