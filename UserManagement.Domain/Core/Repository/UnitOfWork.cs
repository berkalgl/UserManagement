
using System;
using System.Linq;
using UserManagement.Data;
using UserManagement.Data.Interfaces;
using UserManagement.Data.Models;
using UserManagement.Domain.Core.Interfaces;

namespace UserManagement.Domain.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IContext _dbContext;
        private bool _disposed;

        IGenericRepository<User> _userRepository { get; set; }

        public IContext Context
        {
            get
            {
                return _dbContext;
            }
        }
        public IGenericRepository<User> UserRepository
        {
            get { return _userRepository ?? (_userRepository = new GenericRepository<User>(_dbContext)); }
        }
        public UnitOfWork(IGenericContextFactory dbContextFactory)
        {
            _dbContext = (IContext)dbContextFactory.GetDbContext();
        }
        public void Save()
        {
            if (_dbContext.GetValidationErrors().Any())
            {
                //todo
            }
            _dbContext.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
