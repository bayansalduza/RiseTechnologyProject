using MongoDB.Driver;
using RiseTechnologyProject.DataAccess.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RiseTechnologyProject.DataAccess.MongoDbRepository
{
    public class MongoDbRepository<T> : IDisposable, IMongoDbRepository<T> where T : class
    {
        private IMongoDatabase _database;
        private IMongoCollection<T> _collection;
        private MongoClient _client;

        /// <summary>
        /// İlk Database methodunu çalıştırıp, Collection methodunu çalıştırıyoruz
        /// </summary>
        public MongoDbRepository()
        {
            GetDatabase();
            GetCollection();
        }

        /// <summary>
        /// Collection nesnesi database de yoksa yeni oluşturup create ediyor ve collectiona referans ediyor
        /// Varsa sadece collectiona referans ediyor
        /// </summary>
        private void GetCollection()
        {
            if (_database.GetCollection<T>(typeof(T).Name) == null)
            {
                _database.CreateCollection(typeof(T).Name);
            }
            _collection = _database.GetCollection<T>(typeof(T).Name);
        }

        /// <summary>
        /// Databasemizi bağlantısı yoksa oluşturuyor varsa database i referans alıyor
        /// </summary>
        private void GetDatabase()
        {
            _client = new MongoClient(Resources.MONGO_URI);
            _database = _client.GetDatabase(Resources.MONGO_DATABASE);
        }

        /// <summary>
        /// Ekleme işlemi yapar
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            this._collection.InsertOne(entity);
        }

        /// <summary>
        /// Where koşuluna göre silme işlemi yapar
        /// </summary>
        /// <param name="condition"></param>
        public void Delete(Expression<Func<T, bool>> condition)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Where(condition);
            this._collection.DeleteMany(filter);
        }

        public void Dispose()//using bloğu biter bitmez çalışmasını istediğimiz kodlar 
        {

        }

        /// <summary>
        /// Where Koşuluna göre Veriyi çeker
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> condition)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Where(condition);
            return this._collection.Find(filter).FirstOrDefault();
        }

        /// <summary>
        /// Tüm verileri çeker
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            IQueryable<T> iQueryable = this._collection.AsQueryable();
            return iQueryable;
        }

        /// <summary>
        /// Where koşuluna göre verileri çekiyor 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IQueryable<T> GetAll(Expression<Func<T, bool>> condition)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Where(condition);
            return this._collection.Find(filter).ToEnumerable().AsQueryable();
        }

        /// <summary>
        /// Where komutuna göre entity i güncelleme yapıyor
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="entity"></param>
        public void Update(Expression<Func<T, bool>> condition, T entity)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Where(condition);
            this._collection.ReplaceOne(filter, entity);
        }
    }
}
