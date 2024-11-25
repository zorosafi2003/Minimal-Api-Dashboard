using FluentValidation;
using MiniApp.Application.Features.EmployeeFeatures.Queries;
using MiniApp.Application.Features.TimeSheetFeatures.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Application.Features.TimeSheetFeatures.Validators
{
    public class GetTotalHoursBarChartQueryValidator : AbstractValidator<GetTotalHoursBarChartQuery>
    {
        public GetTotalHoursBarChartQueryValidator()
        {
            RuleFor(x => x.Filter.DateFrom).NotNull();
            RuleFor(x => x.Filter.DateTo).NotNull();
        }
    }
}
