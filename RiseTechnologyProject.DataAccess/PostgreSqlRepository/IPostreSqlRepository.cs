using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RiseTechnologyProject.DataAccess.PostgreSqlRepository
{
    public interface IPostgreSqlRepository<T> where T : class
    {
        /// <summary>
        /// Add method
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// Delete method
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// It performs deletion according to the given condition.
        /// </summary>
        /// <param name="entity"></param>
        void Delete(Expression<Func<T, bool>> condition);

        /// <summary>
        /// Gets id and returns an entity
        /// </summary>
        /// <param name="id">Geriye dönülmesini istediğimiz id paramatresi</param>
        /// <returns></returns>
        T Get(int id);

        /// <summary>
        /// A method that returns a single data according to the Linq condition.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> condition);

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Returns list and takes a linq condition
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IQueryable<T> GetAll(Expression<Func<T, bool>> condition);

        /// <summary>
        /// Returns all the data in the table unconditionally
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll();
    }
}
