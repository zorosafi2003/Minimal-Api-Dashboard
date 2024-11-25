using MiniApp.Dal.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Dal.Entities
{
    public class Project : EntityBase
    {
        public string Name { get; set; }
        public virtual ICollection<TimeSheet> TimeSheets { get; set; }
    }
}
