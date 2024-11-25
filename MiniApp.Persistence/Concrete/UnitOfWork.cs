using AutoMapper;
using MiniApp.Dal.Context;
using MiniApp.Dal.Entities;
using MiniApp.Persistence.Abstruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Persistence.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _dbContext;
        private readonly IMapper _mapper;

        public IGenericRepository<Employee> EmployeeRepository { get; private set; }
        public IGenericRepository<TimeSheet> TimeSheetRepository { get; private set; }
        public IGenericRepository<Project> ProjectRepository { get; private set; }

        public UnitOfWork(ApplicationContext dbContext, IMapper Mapper)
        {
            _dbContext = dbContext;
            _mapper = Mapper;

            EmployeeRepository = new GenericRepository<Employee>(dbContext, _mapper);
            TimeSheetRepository = new GenericRepository<TimeSheet>(dbContext, _mapper);
            ProjectRepository = new GenericRepository<Project>(dbContext, _mapper);
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task Rollback()
        {
            var list = _dbContext.ChangeTracker.Entries().ToList();
            list.ForEach(x => x.Reload());
        }
    }
}