using MiniApp.Dal.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Dal.Entities
{
    public class Employee : EntityBase
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public Guid? ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual Project? Project { get; set; }
    }
}
