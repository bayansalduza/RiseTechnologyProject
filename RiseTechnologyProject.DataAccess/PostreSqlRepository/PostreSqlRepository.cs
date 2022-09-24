using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RiseTechnologyProject.DataAccess.PostreSqlRepository
{
    public class PostreSqlRepository<T> : IPostreSqlRepository<T> where T : class
    {
        private readonly DbContext DbContext;
        private readonly DbSet<T> DbSet;

        public PostreSqlRepository(DbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public T Get(int id)
        {
            return DbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> condition)
        {
            return DbSet.Where(condition).FirstOrDefault();
        }

        public IQueryable<T> GetAll()
        {
            return DbContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> condition)
        {
            IQueryable<T> iQueryable = DbSet
               .Where(condition);
            return iQueryable;
        }

        public void Update(T entity)
        {
            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
