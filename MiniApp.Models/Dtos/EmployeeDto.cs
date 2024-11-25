using MiniApp.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Models.Dtos
{
    public class EmployeeDto : DtoBase
    {
        public string Name { get; set; }
        public Guid? ProjectId { get; set; }
    }
}
