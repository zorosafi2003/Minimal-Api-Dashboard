using MiniApp.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Persistence.Abstruct
{
    public interface IUnitOfWork
    {
        IGenericRepository<Employee> EmployeeRepository { get; }
        IGenericRepository<TimeSheet> TimeSheetRepository { get; }
        IGenericRepository<Project> ProjectRepository { get; }
        Task Commit();
        Task Rollback();
    }
}
