using SqlDefinitionStorageExample.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace SqlDefinitionStorageExample.EFCore
{
    public class SqlDefinitionStorageContext(DbContextOptions<SqlDefinitionStorageContext> options) : DbContext(options)
    {

        public DbSet<Resource> Resources { get; set; }

        public DbSet<ResourceFolder> ResourceFolders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = @"Server=.\SQLEXPRESS;Database=WebDesignerStorage;Trusted_Connection=True;TrustServerCertificate=True;";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public bool IsDisposed { get; set; }

        public override void Dispose()
        {
            IsDisposed = true;
            base.Dispose();
            GC.SuppressFinalize(this);
        }

        public override ValueTask DisposeAsync()
        {
            IsDisposed = true;
            GC.SuppressFinalize(this);
            return base.DisposeAsync();
        }
    }
}
