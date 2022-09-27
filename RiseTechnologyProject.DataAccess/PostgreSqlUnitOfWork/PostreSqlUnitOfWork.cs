using Microsoft.EntityFrameworkCore;
using RiseTechnologyProject.DataAccess.PostgreSqlRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseTechnologyProject.DataAccess.PostgreSqlUnitOfWork
{
    public class PostgreSqlUnitOfWork : IPostgreSqlUnitOfWork
    {
        private DbContext Context;
        public PostgreSqlUnitOfWork(DbContext _context)
        {
            Context = _context;
        }

        public void Dispose()
        {
        }

        public IPostgreSqlRepository<T> GetRepository<T>() where T : class
        {
            return new PostgreSqlRepository<T>(Context);
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public async void SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        ~PostgreSqlUnitOfWork()
        {
            Context.Dispose();
        }
    }
}
