using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiniApp.Application.Features.EmployeeFeatures.Filters;
using MiniApp.Dal.Context;
using MiniApp.Models.Base;
using MiniApp.Models.Dtos;
using MiniApp.Persistence.Abstruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Application.Features.EmployeeFeatures.Queries
{
    public class GetEmployeeSummaryQuery : IRequest<CqrsResponse>
    {
        public GetEmployeeSummaryQueryFilter Filter { get; set; }
        public GetEmployeeSummaryQuery(GetEmployeeSummaryQueryFilter filter)
        {
            Filter = filter;
        }

        public class GetEmployeesQueryHnadler : IRequestHandler<GetEmployeeSummaryQuery, CqrsResponse>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetEmployeesQueryHnadler(IUnitOfWork UnitOfWork)
            {
                _unitOfWork = UnitOfWork;
            }
            public async Task<CqrsResponse> Handle(GetEmployeeSummaryQuery request, CancellationToken cancellationToken)
            {
                var query = _unitOfWork.TimeSheetRepository.Query()
                 .Where(x => x.TrackedDate.Date >= request.Filter.DateFrom.Value.Date && x.TrackedDate.Date <= request.Filter.DateTo.Value.Date);

                var data = await query.GroupBy(x => new { x.Employee.Name, x.Employee.Id, x.Employee.ImageUrl })
                    .Select(x => new DataChildOfGetEmployeeSummaryQuery { 
                        EmployeeId = x.Key.Id,
                        EmployeeName = x.Key.Name ,
                        EmployeeUrl = x.Key.ImageUrl,
                        ConfirmedHours = x.Where(c=>c.ActionType == Dal.Enums.ActionTypeEnum.Confirmed).Sum(c=>c.TotalMinutes)/60,
                        ExpectedHours = x.Where(c => c.ActionType == Dal.Enums.ActionTypeEnum.Expected).Sum(c => c.TotalMinutes) / 60,
                        MissingHours = x.Where(c => c.ActionType == Dal.Enums.ActionTypeEnum.Missing).Sum(c => c.TotalMinutes) / 60,
                        UnconfirmedHours = x.Where(c => c.ActionType == Dal.Enums.ActionTypeEnum.UnConfirmed).Sum(c => c.TotalMinutes) / 60
                    }).ToListAsync();

                return new GetEmployeeSummaryQueryResult
                {
                    Data = data
                };
            }
        }

        public class GetEmployeeSummaryQueryResult : CqrsResponse
        {
            public List<DataChildOfGetEmployeeSummaryQuery> Data { get; set; }
        }
        public class DataChildOfGetEmployeeSummaryQuery
        {
            public Guid EmployeeId { get; set; }
            public string EmployeeName { get; set; }
            public string EmployeeUrl { get; set; }
            public int ExpectedHours { get; set; }
            public int UnconfirmedHours { get; set; }
            public int ConfirmedHours { get; set; }
            public int MissingHours { get; set; }
        }
    }
}
