using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.Core.Interfaces
{
    public interface IGenericUnitOfWork : IDisposable
    {
        void Save();
    }
}
