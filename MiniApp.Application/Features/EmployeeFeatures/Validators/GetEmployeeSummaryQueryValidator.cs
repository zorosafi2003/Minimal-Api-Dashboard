using FluentValidation;
using MiniApp.Application.Features.EmployeeFeatures.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Application.Features.EmployeeFeatures.Validators
{
    public class GetEmployeeSummaryQueryValidator : AbstractValidator<GetEmployeeSummaryQuery>
    {
        public GetEmployeeSummaryQueryValidator()
        {
            RuleFor(x => x.Filter.DateFrom).NotNull();
            RuleFor(x => x.Filter.DateTo).NotNull();
        }
    }
}
