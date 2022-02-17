using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Data.Interfaces;

namespace UserManagement.Domain.Core.Interfaces
{
    public interface IGenericContextFactory
    {
        IGenericContext GetDbContext();
    }
}
