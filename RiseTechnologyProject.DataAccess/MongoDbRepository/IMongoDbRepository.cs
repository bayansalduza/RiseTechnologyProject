using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RiseTechnologyProject.DataAccess.MongoDbRepository
{
    public interface IMongoDbRepository<T> where T : class
    {
        /// <summary>
        /// Fetch all data
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Fetch all data based on where command
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IQueryable<T> GetAll(Expression<Func<T, bool>> condition);

        /// <summary>
        /// Returns the desired data as a single
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> condition);

        /// <summary>
        /// Adds data
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// Data is updated according to the condition
        /// </summary>
        /// <param name="condition"></param>
        void Update(Expression<Func<T, bool>> condition, T entity);

        /// <summary>
        /// Deletes data according to condition
        /// </summary>
        /// <param name="condition"></param>
        void Delete(Expression<Func<T, bool>> condition);
    }
}
