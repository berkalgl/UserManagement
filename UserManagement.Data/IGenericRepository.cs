using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Data
{
    public interface IGenericRepository<T> where T : class
    {
        int Count { get; }
        DbSet<T> Set { get; }

        IQueryable<T> All();
        IQueryable<T> AsQueryable();
        bool Contains(Expression<Func<T, bool>> predicate);
        T Create(T entity);
        void CreateAndSave(IList<T> entityList, bool isLog = false);
        T CreateAndSave(T entity, bool isLog = false);
        void Delete(Expression<Func<T, bool>> predicate);
        void DeleteAndSave(T entity, bool isLog = false);
        void DeleteAndSave(IList<T> entityList, bool isLog = false);
        void DeleteAndSaveTransactional(IList<T> entityList);
        void DeleteHard(T entity, bool isLog = false);
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);
        IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50);
        T Find(params object[] keys);
        T Find(Expression<Func<T, bool>> predicate);
        T First(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        T GetById(object id);
        void UpdateAndSave(T entity, bool isLog = false);
        void UpdateAndSaveTransactional(IList<T> entityList, bool isLog = false);
    }
}
