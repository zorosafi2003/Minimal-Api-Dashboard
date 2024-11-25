using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Models.Base
{
    public class FilterBase
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string OrderBy { get; set; }
    }
}
