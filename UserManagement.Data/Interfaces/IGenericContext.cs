using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Data.Interfaces
{
    public interface IGenericContext : IDisposable
    {
        IEnumerable<DbEntityValidationResult> GetValidationErrors();
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet Set(Type entityType);
        Database Database { get; }
        DbContextConfiguration Configuration { get; }
    }
}
