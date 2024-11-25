using MiniApp.Dal.Entities.Base;
using MiniApp.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Persistence.Abstruct
{
    public interface IGenericRepository<T> where T : EntityBase
    {
        IQueryable<T?> Query();
        Task<int> CountAsync();
        Task<T?> FindAsync(Guid id);
        Task<Dto?> GetByIdAsync<Dto>(IQueryable<T> mainQuery, Guid id) where Dto : DtoBase;

        Task<Dto?> GetByIdAsync<Dto>(Guid id) where Dto : DtoBase;
        Task<List<Dto>> GetAll<Dto>(IQueryable<T> query) where Dto : DtoBase;
        Task<DataAndCountDto<Dto>> GetDataAndCountAsync<Dto>(IQueryable<T> query, FilterBase filter) where Dto : DtoBase;
        Task<Guid> Add(T sender);
        Task AddRange(List<T> sender);
        Task Update(Guid id, Expression<Func<T, T>> updateFactory);
        Task Remove(Guid id);
        Task Remove(List<Guid> idArr);

    }
}