using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniApp.Application.Features.TimeSheetFeatures.Filters;
using MiniApp.Models.Base;
using MiniApp.Persistence.Abstruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Application.Features.TimeSheetFeatures.Queries
{
    public class GetTimeSheetSummaryQuery : IRequest<CqrsResponse>
    {
        public GetTimeSheetSummaryQueryFilter Filter { get; set; }
        public GetTimeSheetSummaryQuery(GetTimeSheetSummaryQueryFilter filter)
        {
            Filter = filter;
        }
        public class GetTimeSheetSummaryQueryHandler : IRequestHandler<GetTimeSheetSummaryQuery, CqrsResponse>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetTimeSheetSummaryQueryHandler(IUnitOfWork UnitOfWork)
            {
                _unitOfWork = UnitOfWork;
            }
            public async Task<CqrsResponse> Handle(GetTimeSheetSummaryQuery request, CancellationToken cancellationToken)
            {
                var query = _unitOfWork.TimeSheetRepository.Query()
                  .Where(x => x.TrackedDate.Date >= request.Filter.DateFrom.Value.Date && x.TrackedDate.Date <= request.Filter.DateTo.Value.Date);

                var totalHoursTask = query.SumAsync(x => x.TotalMinutes);
                var expectedHoursTask = query.Where(x => x.ActionType == Dal.Enums.ActionTypeEnum.Expected).SumAsync(x => x.TotalMinutes);
                var createdHoursTask = query.Where(x => x.ActionType == Dal.Enums.ActionTypeEnum.UnConfirmed).SumAsync(x => x.TotalMinutes);
                var acceptedHoursTask = query.Where(x => x.ActionType == Dal.Enums.ActionTypeEnum.Confirmed).SumAsync(x => x.TotalMinutes);
                var missingHoursTask = query.Where(x => x.ActionType == Dal.Enums.ActionTypeEnum.Missing).SumAsync(x => x.TotalMinutes);

                Task.WaitAll(totalHoursTask, expectedHoursTask, createdHoursTask, acceptedHoursTask, acceptedHoursTask);

                var totalHours = await totalHoursTask;
                var expectedHours = await expectedHoursTask;
                var createdHours = await createdHoursTask;
                var acceptedHours = await acceptedHoursTask;
                var missingHours = await missingHoursTask;

                return new GetTimeSheetSummaryQueryResult
                {
                    ExpectedHours = expectedHours / 60,
                    ExpectedPercent = expectedHours / totalHours,

                    AcceptedHours = createdHours / 60,
                    AcceptedPercent = createdHours / totalHours,

                    CreatedHours = acceptedHours / 60,
                    CreatedPercent = acceptedHours / totalHours,

                    MissingHours = missingHours / 60,
                    MissingPercent = missingHours / totalHours,
                };
            }
        }

        public class GetTimeSheetSummaryQueryResult : CqrsResponse
        {
            public int ExpectedHours { get; set; }
            public double ExpectedPercent { get; set; }
            public int CreatedHours { get; set; }
            public double CreatedPercent { get; set; }
            public int AcceptedHours { get; set; }
            public double AcceptedPercent { get; set; }
            public int MissingHours { get; set; }
            public double MissingPercent { get; set; }
        }
    }
}
