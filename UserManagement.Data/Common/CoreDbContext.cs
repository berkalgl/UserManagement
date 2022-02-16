using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Data.Interfaces;

namespace UserManagement.Data
{
    public class CoreDbContext : DbContext, IGenericContext
    {
        /// <summary>
        /// Gets the default schema of the model being migrated.
        /// This schema will be used for the migrations history table unless a different schema is configured in OnModelCreating.
        /// </summary>
        public string DefaultSchema { get; }
        public bool OnMigration { get; set; }
        public List<Type> IgnoredTypes { get; set; } = new List<Type>();

        public CoreDbContext(string nameOrConnectionString, string defaultSchema)
            : base(nameOrConnectionString)
        {
            DefaultSchema = defaultSchema;
        }
        protected virtual void IntegrateMigrationHistory(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HistoryRow>().ToTable("__MigrationHistory");
            modelBuilder.Entity<HistoryRow>().HasKey(h => new
            {
                h.MigrationId,
                h.ContextKey
            });
            modelBuilder.Entity<HistoryRow>().Property(h => h.MigrationId)
                .HasMaxLength(150).IsRequired();
            modelBuilder.Entity<HistoryRow>().Property(h => h.ContextKey)
                .HasMaxLength(300).IsRequired();
            modelBuilder.Entity<HistoryRow>().Property(h => h.Model)
                .IsRequired()
                .IsMaxLength();
            modelBuilder.Entity<HistoryRow>().Property(h => h.ProductVersion)
                .HasMaxLength(32).IsRequired();
            modelBuilder.Entity<HistoryRow>().Property(p => p.MigrationId).HasColumnName("Migration_ID");
            modelBuilder.Entity<HistoryRow>().ToTable(tableName: "MigrationHistory", schemaName: DefaultSchema);
        }

        public int SaveChanges(int userId)
        {
            ChangeTracker.DetectChanges();
            var changeSet = ChangeTracker.Entries();

            if (changeSet == null) return base.SaveChanges();
            var dbEntityEntries = changeSet.ToList();
            foreach (var entry in dbEntityEntries.Where(c => c.State == EntityState.Added))
            {
                var creationDateInfo = entry.Entity.GetType().GetProperty("CREATION_DATE", BindingFlags.Public | BindingFlags.Instance);
                var modifiedDateInfo = entry.Entity.GetType().GetProperty("LAST_UPDATED_DATE", BindingFlags.Public | BindingFlags.Instance);
                var modifiedByUserInfo = entry.Entity.GetType().GetProperty("LAST_UPDATED_USER_ID", BindingFlags.Public | BindingFlags.Instance);
                var createdByUserIdInfo = entry.Entity.GetType().GetProperty("CREATED_BY_USER_ID", BindingFlags.Public | BindingFlags.Instance);
                var isDeletedInfo = entry.Entity.GetType().GetProperty("IS_DELETED", BindingFlags.Public | BindingFlags.Instance);

                if (null != creationDateInfo && creationDateInfo.CanWrite)
                {
                    creationDateInfo.SetValue(entry.Entity, GetBaseNow(), null);
                }

                if (null != modifiedDateInfo && modifiedDateInfo.CanWrite)
                {
                    modifiedDateInfo.SetValue(entry.Entity, GetBaseNow(), null);
                }

                if (null != modifiedByUserInfo && modifiedByUserInfo.CanWrite)
                {
                    modifiedByUserInfo.SetValue(entry.Entity, userId, null);
                }

                if (null != createdByUserIdInfo && createdByUserIdInfo.CanWrite)
                {
                    createdByUserIdInfo.SetValue(entry.Entity, userId, null);
                }

                if (null != isDeletedInfo && isDeletedInfo.CanWrite)
                {
                    isDeletedInfo.SetValue(entry.Entity, 0, null);
                }

                creationDateInfo = entry.Entity.GetType().GetProperty("CreationDate", BindingFlags.Public | BindingFlags.Instance);
                modifiedDateInfo = entry.Entity.GetType().GetProperty("LastUpdatedDate", BindingFlags.Public | BindingFlags.Instance);
                modifiedByUserInfo = entry.Entity.GetType().GetProperty("LastUpdatedUserId", BindingFlags.Public | BindingFlags.Instance);
                createdByUserIdInfo = entry.Entity.GetType().GetProperty("CreatedByUserId", BindingFlags.Public | BindingFlags.Instance);
                isDeletedInfo = entry.Entity.GetType().GetProperty("IsDeleted", BindingFlags.Public | BindingFlags.Instance);

                if (null != creationDateInfo && creationDateInfo.CanWrite)
                {
                    creationDateInfo.SetValue(entry.Entity, GetBaseNow(), null);
                }

                if (null != modifiedDateInfo && modifiedDateInfo.CanWrite)
                {
                    modifiedDateInfo.SetValue(entry.Entity, GetBaseNow(), null);
                }

                if (null != modifiedByUserInfo && modifiedByUserInfo.CanWrite)
                {
                    modifiedByUserInfo.SetValue(entry.Entity, userId, null);
                }

                if (null != createdByUserIdInfo && createdByUserIdInfo.CanWrite)
                {
                    createdByUserIdInfo.SetValue(entry.Entity, userId, null);
                }

                if (null != isDeletedInfo && isDeletedInfo.CanWrite)
                {
                    isDeletedInfo.SetValue(entry.Entity, 0, null);
                }
            }

            foreach (var entry in dbEntityEntries.Where(c => c.State == EntityState.Modified))
            {
                var modifiedDate = entry.Entity.GetType().GetProperty("LAST_UPDATED_DATE", BindingFlags.Public | BindingFlags.Instance);
                var modifiedByUserInfo = entry.Entity.GetType().GetProperty("LAST_UPDATED_USER_ID", BindingFlags.Public | BindingFlags.Instance);

                if (null != modifiedDate && modifiedDate.CanWrite)
                {
                    modifiedDate.SetValue(entry.Entity, GetBaseNow(), null);
                }

                if (null != modifiedByUserInfo && modifiedByUserInfo.CanWrite)
                {
                    modifiedByUserInfo.SetValue(entry.Entity, userId, null);
                }
                modifiedDate = entry.Entity.GetType().GetProperty("LastUpdatedDate", BindingFlags.Public | BindingFlags.Instance);
                modifiedByUserInfo = entry.Entity.GetType().GetProperty("LastUpdatedUserId", BindingFlags.Public | BindingFlags.Instance);

                if (null != modifiedDate && modifiedDate.CanWrite)
                {
                    modifiedDate.SetValue(entry.Entity, GetBaseNow(), null);
                }

                if (null != modifiedByUserInfo && modifiedByUserInfo.CanWrite)
                {
                    modifiedByUserInfo.SetValue(entry.Entity, userId, null);
                }
            }

            return base.SaveChanges();
        }
        private static DateTime? GetBaseNow()
        {
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc,
                TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"));
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Default schema 
            modelBuilder.HasDefaultSchema(DefaultSchema);
            base.OnModelCreating(modelBuilder);
            if (OnMigration)
            {
                modelBuilder.Ignore(IgnoredTypes);
            }
            IntegrateMigrationHistory(modelBuilder);
        }
    }
}
