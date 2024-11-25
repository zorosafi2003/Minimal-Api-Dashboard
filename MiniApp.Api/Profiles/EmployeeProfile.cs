using AutoMapper;
using MiniApp.Dal.Entities;
using MiniApp.Models.Dtos;

namespace MiniApp.Api.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>();
        }
    }
}
