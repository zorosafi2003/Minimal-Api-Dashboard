using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Models.Base
{
    public class DataAndCountDto<T> where T : class
    {
        public List<T> Data { get; set; }

        public int Count { get; set; }

        public DataAndCountDto()
        {
            Data = new List<T>();
            Count = 0;
        }
    }
}
