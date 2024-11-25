using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MiniApp.Dal.Context;
using MiniApp.Dal.Entities.Base;
using MiniApp.Models.Base;
using MiniApp.Persistence.Abstruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using System.Linq.Dynamic.Core;

namespace MiniApp.Persistence.Concrete
{
    public class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : EntityBase
    {
        private bool _disposed = false;
        private ApplicationContext _db;
        private readonly IMapper _mapper;
        private DbSet<T> entities;

        public GenericRepository(ApplicationContext db, IMapper Mapper)
        {
            _db = db;
            _mapper = Mapper;
            entities = _db.Set<T>();
        }

        public IQueryable<T> Query()
        {
            return entities.AsQueryable().AsNoTracking();
        }

        public async Task<int> CountAsync()
        {
            return await entities.AsQueryable().AsNoTracking().CountAsync();
        }

        public async Task<T?> FindAsync(Guid id)
        {
            return await entities.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync(); ;
        }

        public async Task<Dto?> GetByIdAsync<Dto>(IQueryable<T> mainQuery, Guid id) where Dto : DtoBase
        {
            return await mainQuery.Where(x => x.Id == id).ProjectTo<Dto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(); ;
        }

        public async Task<Dto?> GetByIdAsync<Dto>(Guid id) where Dto : DtoBase
        {
            return await entities.AsNoTracking().Where(x => x.Id == id).ProjectTo<Dto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(); ;
        }

        public async Task<List<Dto>> GetAll<Dto>(IQueryable<T> mainQuery) where Dto : DtoBase
        {
            return await mainQuery.ProjectTo<Dto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<DataAndCountDto<Dto>> GetDataAndCountAsync<Dto>(IQueryable<T> mainQuery, FilterBase filter) where Dto : DtoBase
        {
            DataAndCountDto<Dto> model = new DataAndCountDto<Dto>();

            model.Count = await mainQuery.CountAsync();

            IQueryable<Dto> query = mainQuery.ProjectTo<Dto>(_mapper.ConfigurationProvider);

            if (string.IsNullOrEmpty(filter.OrderBy))
            {
                query = query.OrderByDescending(x => x.CreateDate);
            }
            else
            {
                var propertyName = filter.OrderBy.Split("-")[0];
                var isReverse = filter.OrderBy.Split("-")[1] == "desc" ? true : false;

                query = query.OrderBy(propertyName + (isReverse ? " descending" : "")); ;
            }

            if (filter.Skip == null)
            {
                filter.Skip = 0;
            }

            if (filter.Take == null)
            {
                filter.Take = int.MaxValue;
            }

            model.Data = await query.Skip(filter.Skip.Value).Take(filter.Take.Value).ToListAsync();

            return model;
        }

        public async Task<Guid> Add(T sender)
        {
            await entities.AddAsync(sender);
            return sender.Id;
        }

        public async Task AddRange(List<T> sender)
        {
            await entities.AddRangeAsync(sender);
        }

        public async Task Update(Guid id, Expression<Func<T, T>> updateFactory)
        {
            await entities.Where(x => x.Id == id).UpdateAsync(updateFactory);
        }

        public async Task Remove(Guid id)
        {
            var obj = await entities.FindAsync(id);
            obj.DeletedOn = DateTime.Now;
        }

        public async Task Remove(List<Guid> idArr)
        {
            var list = await entities.Where(x => idArr.Contains(x.Id)).ToListAsync();
            list.ForEach(x => x.DeletedOn = DateTime.Now);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}