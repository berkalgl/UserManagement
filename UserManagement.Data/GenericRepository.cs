using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UserManagement.Data.Common;
using UserManagement.Data.Interfaces;

namespace UserManagement.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IGenericContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public DbSet<T> Set
        {
            get { return _dbSet; }
        }

        public virtual IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable<T>();
        }

        public GenericRepository(IGenericContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public virtual int Count
        {
            get { return _dbSet.Count(); }
        }

        public virtual IQueryable<T> All()
        {
            return _dbSet.AsQueryable();
        }

        public virtual T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual IQueryable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!String.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return orderBy != null ? orderBy(query).AsQueryable() : query.AsQueryable();
        }

        public virtual IQueryable<T> Filter(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsQueryable();
        }

        public virtual IQueryable<T> Filter(System.Linq.Expressions.Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var resetSet = filter != null ? _dbSet.Where(filter).AsQueryable() : _dbSet.AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }

        public virtual T First(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbSet.First(predicate);
        }

        public virtual T FirstOrDefault(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public virtual bool Contains(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Count(predicate) > 0;
        }

        public virtual T Find(params object[] keys)
        {
            return _dbSet.Find(keys);
        }

        public virtual T Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }
        public T CreateAndSave(T entity, bool isLog = false)
        {
            Create(entity);
            _dbContext.SaveChanges();
            _dbContext.Entry(entity).GetDatabaseValues();
            //if (isLog)
            //    HistoryTableInsertion.InsertHistory(entity, _dbContext, "CREATE", null);

            return entity;
        }
        public void CreateAndSave(IList<T> entityList, bool isLog = false)
        {
            foreach (var entity in entityList)
            {
                Create(entity);
            }
            //if (isLog)
            //    foreach (var entity in entityList)
            //    {
            //        HistoryTableInsertion.InsertHistory(entity, _dbContext, "CREATE", null);
            //    }
            _dbContext.SaveChanges();
        }
        public virtual T Create(T entity)
        {
            var newEntry = _dbSet.Add(entity);
            return newEntry;
        }
        public void CreateAndSaveTransactional(IList<T> entityList, bool isLog = false)
        {
            using (var transaction = new TransactionScope())
            {
                foreach (var entity in entityList)
                {
                    Create(entity);
                }
                _dbContext.SaveChanges();
                //if (isLog)
                //    foreach (var entity in entityList)
                //    {
                //        HistoryTableInsertion.InsertHistory(entity, _dbContext, "CREATE", secretKey);
                //    }
                transaction.Complete();
            }
        }
        public virtual void UpdateAndSave(T entity, bool isLog = false)
        {
            Update(entity, isLog);
            _dbContext.SaveChanges();
        }
        public virtual void Update(T entity, bool isLog = false, bool isUpdate = true)
        {
            var entry = _dbContext.Entry(entity);
            _dbSet.Attach(entity);
            entry.State = EntityState.Modified;
            var word = "UPDATE";
            //if (isLog)
            //{
            //    if (!isUpdate) word = "DELETE";
            //    HistoryTableInsertion.InsertHistory(entity, _dbContext, word, secretKey);
            //}
        }
        public virtual void UpdateAndSaveTransactional(IList<T> entityList, bool isLog = false)
        {
            using (var transaction = new TransactionScope())
            {
                foreach (var entity in entityList)
                {
                    Update(entity, isLog);
                }
                _dbContext.SaveChanges();
                transaction.Complete();
            }
        }
        public void DeleteAndSave(T entity, bool isLog = false)
        {
            {
                var isDeleted = entity.GetType().GetProperty("IS_DELETED", BindingFlags.Public | BindingFlags.Instance);
                if (null != isDeleted && isDeleted.CanWrite)
                {
                    isDeleted.SetValue(entity, 1, null);
                }
            }

            Update(entity, isLog, false);
            _dbContext.SaveChanges();
        }
        public void DeleteAndSave(IList<T> entityList, bool isLog = false)
        {
            foreach (var entity in entityList)
            {
                {
                    var isDeleted = entity.GetType().GetProperty("IS_DELETED", BindingFlags.Public | BindingFlags.Instance);
                    if (null != isDeleted && isDeleted.CanWrite)
                    {
                        isDeleted.SetValue(entity, 1, null);
                    }
                }

                {
                    var isDeleted = entity.GetType().GetProperty("IsDeleted", BindingFlags.Public | BindingFlags.Instance);
                    if (null != isDeleted && isDeleted.CanWrite)
                    {
                        isDeleted.SetValue(entity, 1, null);
                    }
                }
                Update(entity, isLog, false);
            }
            _dbContext.SaveChanges();
        }
        public virtual void DeleteAndSaveTransactional(IList<T> entityList)
        {
            using (var transaction = new TransactionScope())
            {
                foreach (var entity in entityList)
                {
                    {
                        var isDeleted = entity.GetType().GetProperty("IS_DELETED", BindingFlags.Public | BindingFlags.Instance);
                        if (null != isDeleted && isDeleted.CanWrite)
                        {
                            isDeleted.SetValue(entity, 1, null);
                        }
                    }

                    {
                        var isDeleted = entity.GetType().GetProperty("IsDeleted", BindingFlags.Public | BindingFlags.Instance);
                        if (null != isDeleted && isDeleted.CanWrite)
                        {
                            isDeleted.SetValue(entity, 1, null);
                        }
                    }

                    Update(entity, false, false);
                }
                _dbContext.SaveChanges();
                transaction.Complete();
            }
        }
        public virtual void Delete(T entity, bool isLog = false)
        {
            //if (isLog)
            //{
            //    HistoryTableInsertion.InsertHistory(entity, _dbContext, "DELETE", secretKey);
            //}
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }
        public virtual void DeleteHard(T entity, bool isLog = false)
        {
            Delete(entity, isLog);
            _dbContext.SaveChanges();
        }
        public virtual void Delete(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            var entitiesToDelete = Filter(predicate);
            foreach (var entity in entitiesToDelete)
            {
                if (_dbContext.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
                _dbSet.Remove(entity);
            }
        }
    }
}
