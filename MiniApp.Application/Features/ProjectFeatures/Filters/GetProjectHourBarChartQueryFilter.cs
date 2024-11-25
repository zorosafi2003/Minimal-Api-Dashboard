using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Application.Features.ProjectFeatures.Filters
{
    public class GetProjectHourBarChartQueryFilter
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
