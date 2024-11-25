using Microsoft.EntityFrameworkCore;
using MiniApp.Dal.Context;
using MiniApp.Dal.Entities;
using MiniApp.Dal.Enums;
using MiniApp.Persistence.IProviders;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Persistence.Providers
{
    public class SeedProvider : ISeedProvider
    {
        private readonly ApplicationContext _db;

        public SeedProvider(ApplicationContext db)
        {
            _db = db;
        }

        public async Task InitDevelopment()
        {
            await SeedProjects();
            await SeedUsers();
            await SeedTimeSheet();
        }

        public async Task InitProduction()
        {
            await SeedProjects();
            await SeedUsers();
            await SeedTimeSheet();
        }

        public async Task SeedProjects()
        {
            if (!_db.Project.Any())
            {
                _db.Project.AddRange(
                  new List<Project>
                  {
                  new Project{Id = Guid.NewGuid(),Name="Apple" } ,
                   new Project{Id = Guid.NewGuid(),Name="Walmart" } ,
                      new Project{Id = Guid.NewGuid(),Name="Microsoft" } ,
                        new Project{Id = Guid.NewGuid(),Name="Project01" } ,
                         new Project{Id = Guid.NewGuid(),Name="Project02" } ,
                           new Project{Id = Guid.NewGuid(),Name="Project03" } ,
                             new Project{Id = Guid.NewGuid(),Name="Project04" } ,
                               new Project{Id = Guid.NewGuid(),Name="Project05" } ,
                                 new Project{Id = Guid.NewGuid(),Name="Project06" } ,
                                   new Project{Id = Guid.NewGuid(),Name="Project07" } ,
                  }
                );

                await _db.SaveChangesAsync();
            }
        }

        public async Task SeedUsers()
        {
            if (!_db.Employee.Any())
            {
                var projectIdArr = await _db.Project.Select(x => x.Id).ToListAsync();

                _db.Employee.AddRange(
                new List<Employee>
                {
                  new Employee{Id = Guid.NewGuid(),Name="Theresa Webb" , ProjectId =projectIdArr[0]} ,
                   new Employee{Id = Guid.NewGuid(),Name="Darrell Steward" , ProjectId =projectIdArr[1]} ,
                      new Employee{Id = Guid.NewGuid(),Name="Marvin McKinney", ProjectId =projectIdArr[2] } ,
                        new Employee{Id = Guid.NewGuid(),Name="Brooklyn Simmons" , ProjectId =projectIdArr[3]} ,
                         new Employee{Id = Guid.NewGuid(),Name="Wade Warren", ProjectId =projectIdArr[4] } ,
                }
              );

                await _db.SaveChangesAsync();
            }
        }

        public async Task SeedTimeSheet()
        {
            if (!_db.TimeSheet.Any())
            {
                var emplyeeList = await _db.Employee.Select(x => new { x.Id, x.ProjectId }).ToListAsync();

                foreach (var item in emplyeeList)
                {
                    var list = new List<TimeSheet>();

                    for (var i = 0; i < 500 ; i++)
                    {
                        var randomDate = RandomizerFactory.GetRandomizer(new FieldOptionsDateTime { From = new DateTime(2024, 1, 1, 0, 0, 0), To = new DateTime(2025, 12, 1, 0, 0, 0) });
                        var randombool = RandomizerFactory.GetRandomizer(new FieldOptionsBoolean ());

                        Random rnd = new Random();
                        var randomNumber = rnd.Next(0, 3);

                        var trackedDate = randomDate.Generate().Value;

                        list.Add(new TimeSheet { 
                            Id = Guid.NewGuid(),
                            EmployeeId = item.Id ,
                            ProjectId = item.ProjectId ,
                            IsWorking = randombool.Generate().Value, 
                            TotalMinutes = 10 ,
                            TrackedDate = trackedDate,
                            CreateDate = trackedDate,
                            ActionType = (ActionTypeEnum) randomNumber
                        });
                    }

                    _db.TimeSheet.AddRange(list);
                    await _db.SaveChangesAsync();
                }
            }
        }
    }
}
