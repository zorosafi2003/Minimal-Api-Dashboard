using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniApp.Application.Features.TimeSheetFeatures.Filters;
using MiniApp.Models.Base;
using MiniApp.Persistence.Abstruct;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Application.Features.TimeSheetFeatures.Queries
{
    public class GetTotalHoursDonutChartQuery : IRequest<CqrsResponse>
    {
        public GetTotalHoursDonutChartQueryFilter  Filter { get; set; }
        public GetTotalHoursDonutChartQuery(GetTotalHoursDonutChartQueryFilter filter)
        {
            Filter = filter;
        }
        public class GetTotalHoursDonutChartQueryHandler : IRequestHandler<GetTotalHoursDonutChartQuery, CqrsResponse>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetTotalHoursDonutChartQueryHandler(IUnitOfWork UnitOfWork)
            {
                _unitOfWork = UnitOfWork;
            }
            public async Task<CqrsResponse> Handle(GetTotalHoursDonutChartQuery request, CancellationToken cancellationToken)
            {

                var query = _unitOfWork.TimeSheetRepository.Query()
                    .Where(x=>x.TrackedDate.Date >= request.Filter.DateFrom.Value.Date && x.TrackedDate.Date <= request.Filter.DateTo.Value.Date);


                var totalTrackedTask = query.SumAsync(x => x.TotalMinutes);
                var totalWorkedTask = query.Where(x => x.IsWorking == true).SumAsync(x => x.TotalMinutes);

                Task.WaitAll([totalTrackedTask, totalWorkedTask]);

                var totalTracked = await totalTrackedTask;
                var totalWorked = await totalWorkedTask;

                var result = new GetTotalHoursDonutChartQueryResult
                {
                    TotalTracked = int.Parse((totalTracked / 60).ToString()),
                    TotalWorked = int.Parse((totalWorked / 60).ToString())
                };

                return result;
            }
        }

        public class GetTotalHoursDonutChartQueryResult : CqrsResponse
        {
            public int TotalWorked { get; set; }
            public int TotalTracked { get; set; }
        }
    }
}
