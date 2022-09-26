using Microsoft.EntityFrameworkCore;
using RiseTechnologyProject.DataAccess.PostreSqlRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseTechnologyProject.DataAccess.PostreSqlUnitOfWork
{
    public class PostreSqlUnitOfWork : IPostreSqlUnitOfWork
    {
        private DbContext Context;
        public PostreSqlUnitOfWork(DbContext _context)
        {
            Context = _context;
        }

        public void Dispose()
        {
        }

        public IPostreSqlRepository<T> GetRepository<T>() where T : class
        {
            return new PostreSqlRepository<T>(Context);
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public async void SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
