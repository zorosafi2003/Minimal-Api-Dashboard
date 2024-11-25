using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniApp.Application.Features.ProjectFeatures.Filters;
using MiniApp.Dal.Entities;
using MiniApp.Models.Base;
using MiniApp.Persistence.Abstruct;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Application.Features.ProjectFeatures.Queries
{
    public class GetProjectHourBarChartQuery : IRequest<CqrsResponse>
    {
        public GetProjectHourBarChartQueryFilter   Filter { get; set; }
        public GetProjectHourBarChartQuery(GetProjectHourBarChartQueryFilter filter)
        {
            Filter = filter;
        }
        public class GetProjectHourBarChartQueryHandler : IRequestHandler<GetProjectHourBarChartQuery, CqrsResponse>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetProjectHourBarChartQueryHandler(IUnitOfWork UnitOfWork)
            {
                _unitOfWork = UnitOfWork;
            }
            public async Task<CqrsResponse> Handle(GetProjectHourBarChartQuery request, CancellationToken cancellationToken)
            {
                var query = _unitOfWork.TimeSheetRepository.Query().Include(x=>x.Project)
                 .Where(x => x.TrackedDate.Date >= request.Filter.DateFrom.Value.Date && x.TrackedDate.Date <= request.Filter.DateTo.Value.Date);

                var data = await query.GroupBy(x => new { x.ProjectId, ProjectName = x.Project.Name }).Select(x => new DataChildOfGetProjectHourBarChartQueryResult
                {
                    ProjectId = x.Key.ProjectId.Value,
                    ProjectName = x.Key.ProjectName,
                    TotalTracked = x.Sum(c => c.TotalMinutes) / 60
                }).ToListAsync();

                return new GetProjectHourBarChartQueryResult
                {
                    Data = data
                };
            }
        }

        public class GetProjectHourBarChartQueryResult : CqrsResponse
        {
            public List<DataChildOfGetProjectHourBarChartQueryResult> Data { get; set; }
        }

        public class DataChildOfGetProjectHourBarChartQueryResult
        {
            public int TotalTracked { get; set; }
            public Guid ProjectId { get; set; }
            public string ProjectName { get; set; }
        }
    }
}
