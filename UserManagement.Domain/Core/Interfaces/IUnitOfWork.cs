using UserManagement.Data;
using UserManagement.Data.Interfaces;
using UserManagement.Data.Models;

namespace UserManagement.Domain.Core.Interfaces
{
    public interface IUnitOfWork : IGenericUnitOfWork
    {
        IContext Context { get; }

        IGenericRepository<User> UserRepository { get; }
    }
}
