using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Data.Models;

namespace UserManagement.Data.Interfaces
{
    public interface IContext : IGenericContext
    {
        IDbSet<User> User { get; set; }

    }
}
