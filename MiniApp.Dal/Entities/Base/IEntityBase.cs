using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Dal.Entities.Base
{
    public interface IEntityBase
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
