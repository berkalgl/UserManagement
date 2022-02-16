using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Data.Interfaces;
using UserManagement.Data.Models;

namespace UserManagement.Data
{
    public class UserManagementDbContextFactory : CoreDbContextFactory<UserManagementDbContext> 
    {
        public UserManagementDbContextFactory() : base("name=UserManagementDbContext", "UserManagement")
        {

        }

        public override List<Type> IgnoredTypeList()
        {
            return new List<Type>()
            {
                //Add If you want to ignore a type
                //typeof(User)
            };
        }
        public override List<Type> RemoveFromIgnoredTypeList()
        {
            return new List<Type>()
            {
                //Add If you want to remove from ignore typelist
                typeof(User)
            };
        }
    }
    public class UserManagementDbContext : CoreDbContext, IContext
    {
        public UserManagementDbContext(string nameOrConnectionstring = "name=UserManagementDbContext")
            : base(nameOrConnectionstring, "UserManagement")
        {
            Database.CommandTimeout = 180;
            Database.SetInitializer<UserManagementDbContext>(null);
        }
        public IDbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
