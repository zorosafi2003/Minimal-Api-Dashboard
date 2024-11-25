using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using MiniApp.Application.Features.EmployeeFeatures.Queries;
using MiniApp.Application.Features.TimeSheetFeatures.Filters;
using MiniApp.Application.Features.TimeSheetFeatures.Queries;

namespace MiniApp.Api.Modules
{
    public class DashboardModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var endPoints = app.MapGroup("/Dashboard").WithTags("Dashboard");

            endPoints.MapPost("/TotalHoursBarChart", async (IMediator mediator, [FromBody] GetTotalHoursBarChartQueryFilter filter) =>
            {
                var result = await mediator.Send(new GetTotalHoursBarChartQuery(filter));
                return TypedResults.Ok(result);
            }).WithOpenApi() ;


            endPoints.MapPost("/TotalHoursDonutChart", async (IMediator mediator, [FromBody] GetTotalHoursDonutChartQueryFilter filter) =>
            {
                var result = await mediator.Send(new GetTotalHoursDonutChartQuery(filter));
                return TypedResults.Ok(result);
            }).WithOpenApi();


            endPoints.MapPost("/TimeSheetSummary", async (IMediator mediator, [FromBody] GetTimeSheetSummaryQueryFilter filter) =>
            {
                var result = await mediator.Send(new GetTimeSheetSummaryQuery(filter));
                return TypedResults.Ok(result);
            }).WithOpenApi();
        }
    }
}
