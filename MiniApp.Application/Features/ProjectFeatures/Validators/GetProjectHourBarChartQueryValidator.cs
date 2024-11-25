using FluentValidation;
using MiniApp.Application.Features.EmployeeFeatures.Queries;
using MiniApp.Application.Features.ProjectFeatures.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Application.Features.ProjectFeatures.Validators
{
    public class GetProjectHourBarChartQueryValidator : AbstractValidator<GetProjectHourBarChartQuery>
    {
        public GetProjectHourBarChartQueryValidator()
        {
            RuleFor(x => x.Filter.DateFrom).NotNull();
            RuleFor(x => x.Filter.DateTo).NotNull();
        }
    }
}
