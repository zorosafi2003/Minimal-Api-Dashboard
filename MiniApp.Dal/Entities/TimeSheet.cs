using MiniApp.Dal.Entities.Base;
using MiniApp.Dal.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Dal.Entities
{
    public class TimeSheet : EntityBase
    {
        public DateTime TrackedDate { get; set; }
        public bool IsWorking { get; set; }
        public int TotalMinutes { get; set; }
        public ActionTypeEnum ActionType { get; set; }

        public Guid? ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual Project? Project { get; set; }

        public Guid? EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee? Employee { get; set; }
    }
}
