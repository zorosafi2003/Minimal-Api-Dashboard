using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniApp.Application.Features.TimeSheetFeatures.Filters;
using MiniApp.Models.Base;
using MiniApp.Persistence.Abstruct;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Application.Features.TimeSheetFeatures.Queries
{
    public class GetTotalHoursBarChartQuery : IRequest<CqrsResponse>
    {
        public GetTotalHoursBarChartQueryFilter   Filter { get; set; }
        public GetTotalHoursBarChartQuery(GetTotalHoursBarChartQueryFilter filter)
        {
            Filter = filter;
        }
        public class GetTotalHoursBarChartQueryHandler : IRequestHandler<GetTotalHoursBarChartQuery, CqrsResponse>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetTotalHoursBarChartQueryHandler(IUnitOfWork UnitOfWork)
            {
                _unitOfWork = UnitOfWork;
            }
            public async Task<CqrsResponse> Handle(GetTotalHoursBarChartQuery request, CancellationToken cancellationToken)
            {
                var query = _unitOfWork.TimeSheetRepository.Query()
                  .Where(x => x.TrackedDate.Date >= request.Filter.DateFrom.Value.Date && x.TrackedDate.Date <= request.Filter.DateTo.Value.Date);

                var data = await query.GroupBy(x => x.CreateDate.Month)
                    .Select(x => new DataChildOfGetTotalHoursBarChartQueryResult
                    {
                        MonthId = x.Key,
                        TotalTracked = x.Sum(c => c.TotalMinutes) / 60,
                        TotalWorked = x.Where(c => c.IsWorking == true).Sum(c => c.TotalMinutes) / 60,
                    }).ToListAsync();

                return new GetTotalHoursBarChartQueryResult
                {
                    Data = data
                };
            }
        }

        public class GetTotalHoursBarChartQueryResult : CqrsResponse
        {
            public List<DataChildOfGetTotalHoursBarChartQueryResult> Data { get; set; }
        }

        public class DataChildOfGetTotalHoursBarChartQueryResult
        {
            public int TotalWorked { get; set; }
            public int TotalTracked { get; set; }
            public int MonthId { get; set; }

            [NotMapped]
            public string MonthName { get {
                    return new DateTime(2024, MonthId, 1).ToString("MMM", new CultureInfo("en-GB"));
                } }
        }
    }
}
