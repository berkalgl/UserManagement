using UserManagement.Data;
using UserManagement.Data.Interfaces;
using UserManagement.Domain.Core.Interfaces;

namespace UserManagement.Domain.Core.Repository
{
    public class DbContextFactory : IGenericContextFactory
    {
        private readonly IContext _context;

        public DbContextFactory()
        {
            _context = new UserManagementDbContext();
            _context.Configuration.LazyLoadingEnabled = false;
            _context.Configuration.ProxyCreationEnabled = false;
        }

        public IGenericContext GetDbContext()
        {
            return _context;
        }
    }
}
