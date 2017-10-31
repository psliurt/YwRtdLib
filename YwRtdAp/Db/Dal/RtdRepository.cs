using System;
using System.Data.Entity;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using YwRtdAp.Db.DbObject;

namespace YwRtdAp.Db.Dal
{
    public class RtdRepository
    {
        private string _requestId { get; set; }

        public RtdRepository()
        {
            _requestId = Guid.NewGuid().ToString().Replace("-", "");
        }

        public RtdRepository(string reqId)
        {
            _requestId = reqId;
        }

        private DbContext _context = new RtdBaseContext();
        public void Insert<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public IQueryable<T> FetchAll<T>() where T : class
        {
            return _context.Set<T>().AsQueryable();
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return _context.Set<T>().Where(predicate);//.AsQueryable();
        }

        public T DefaultOne<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return _context.Set<T>().Where(predicate).FirstOrDefault();
        }

        public T QueryByKey<T>(params object[] keys) where T : class
        {
            return _context.Set<T>().Find(keys);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteByKey<T>(params object[] keys) where T : class
        {
            T obj = _context.Set<T>().Find(keys);
            if (obj != null)
            {
                Delete<T>(obj);
            }
        }

        public int Commit()
        {
            int ret = 0;
            try
            { 
                ret = _context.SaveChanges();
            }
            catch(Exception e)
            {
                throw e;
            }

            return ret;
        }
        
        public void ExecuteSql(string sql, params DbParameter[] parameters)
        {            
            this._context.Database.ExecuteSqlCommand(sql, parameters);
        }
        
        public void Close()
        {
            this._context.Dispose();
        }
    }
}
