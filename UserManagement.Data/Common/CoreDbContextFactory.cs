using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Data
{
    public abstract class CoreDbContextFactory<TContext> : IDbContextFactory<TContext> where TContext : CoreDbContext
    {
        private readonly string _nameOrConnectionString;
        private readonly string _moduleName;

        protected CoreDbContextFactory(string nameOrConnectionString, string moduleName)
        {
            _nameOrConnectionString = nameOrConnectionString;
            _moduleName = moduleName;
        }

        public TContext CreateContextType()
        {
            return (TContext)Activator.CreateInstance(typeof(TContext), _nameOrConnectionString);
        }

        public virtual IEnumerable<Type> MigrationIgnoredTypes(Type contextType, string defaultSchema)
        {
            var ignoredTypes = new List<Type>();
            var contextTypes
                = contextType.GetProperties()
                    .Where(en =>
                        en.PropertyType.IsGenericType &&
                        en.PropertyType.GetGenericTypeDefinition() == typeof(IDbSet<>));
            foreach (var table in contextTypes)
            {
                var tableType = table.PropertyType.GenericTypeArguments.First();
                if (tableType == null) continue;
                if (tableType.Name.StartsWith("V_"))
                {
                    ignoredTypes.Add(tableType);
                }
                else
                {
                    var tableTypeAttribute = (tableType.GetCustomAttribute<TableAttribute>());
                    var schema = string.Empty;
                    if (!string.IsNullOrEmpty(tableTypeAttribute.Schema))
                    {
                        schema = tableTypeAttribute.Schema;
                    }
                    else if (tableTypeAttribute.Name.Contains("."))
                    {
                        schema = tableTypeAttribute.Name.Split('.').FirstOrDefault();
                    }

                    if (defaultSchema != schema)
                    {
                        ignoredTypes.Add(tableType);
                    }
                }
            }

            return ignoredTypes;
        }

        public TContext Create()
        {
            DeleteEntityNames(_moduleName);
            var contextTypes
                = typeof(TContext).GetProperties()
                    .Where(en =>
                        en.PropertyType.IsGenericType &&
                        en.PropertyType.GetGenericTypeDefinition() == typeof(IDbSet<>));
            foreach (var table in contextTypes)
            {
                var tableType = table.PropertyType.GenericTypeArguments.First();
                if (tableType == null) continue;
                var tableTypeAttribute = (tableType.GetCustomAttribute<TableAttribute>());
                var schema = string.Empty;
                var tableName = string.Empty;
                if (!string.IsNullOrEmpty(tableTypeAttribute.Schema))
                {
                    schema = tableTypeAttribute.Schema;
                    tableName = tableTypeAttribute.Schema;
                }
                else if (tableTypeAttribute.Name.Contains("."))
                {
                    schema = tableTypeAttribute.Name.Split('.').FirstOrDefault();
                    tableName = tableTypeAttribute.Name.Split('.').LastOrDefault();
                }
                var recordIsExists = IsEntityExists(_moduleName, tableName, schema);

                if (!recordIsExists)
                {
                    SetEntityNames(_moduleName, tableName, schema);
                }
            }

            var context = CreateContextType();// new DemandDbContext();
            context.OnMigration = true;
            context.IgnoredTypes = MigrationIgnoredTypes(typeof(TContext), context.DefaultSchema).ToList();
            context.IgnoredTypes.AddRange(IgnoredTypeList());
            RemoveFromIgnoredTypeList().ForEach(en => context.IgnoredTypes.Remove(en));
            return context;
        }

        public abstract List<Type> IgnoredTypeList();
        public abstract List<Type> RemoveFromIgnoredTypeList();


        private string CreateConnectionString()
        {
            //TO_DO connection string
            return "";
        }

        public bool IsEntityExists(string moduleName, string tableName, string schemaName)
        {

            bool recordIsExists;
            using (var con = new SqlConnection(CreateConnectionString()))
            {
                var checkSql = $"SELECT COUNT(*) from [DB].[CONFIGURATION].[MODULE_USAGE] where REFERRED_MODULE = '{moduleName}' AND REFERRING_MODULE_SCHEMA = '{schemaName}' AND REFERRING_MODULE_TABLE = '{tableName}'";
                using (var dataCommand = con.CreateCommand())
                {
                    con.Open();
                    dataCommand.CommandText = checkSql;
                    var userCount = (int)dataCommand.ExecuteScalar();
                    recordIsExists = userCount > 0;
                }
            }
            return recordIsExists;
        }

        public void SetEntityNames(string moduleName, string tableName, string schemaName)
        {
            using (var con = new SqlConnection(CreateConnectionString()))
            {
                var sql = $"INSERT INTO [DB].[CONFIGURATION].[MODULE_USAGE]([REFERRED_MODULE],[REFERRING_MODULE_SCHEMA] ,[REFERRING_MODULE_TABLE] ,[IS_USING]) VALUES('{moduleName}','{schemaName}','{tableName}',1) ";
                using (var dataCommand = con.CreateCommand())
                {
                    con.Open();
                    dataCommand.CommandText = sql;
                    dataCommand.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public void DeleteEntityNames(string moduleName)
        {
            using (var con = new SqlConnection(CreateConnectionString()))
            {
                var sql = $"DELETE FROM [DB].[CONFIGURATION].[MODULE_USAGE] WHERE [REFERRED_MODULE]='{moduleName}'";
                using (var dataCommand = con.CreateCommand())
                {
                    con.Open();
                    dataCommand.CommandText = sql;
                    dataCommand.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
}
