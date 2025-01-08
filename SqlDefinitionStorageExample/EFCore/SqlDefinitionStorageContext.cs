using SqlDefinitionStorageExample.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SqlDefinitionStorageExample.EFCore
{
    public class SqlDefinitionStorageContext : DbContext
    {

        public DbSet<Resource> Resources { get; set; }

        public DbSet<ResourceFolder> ResourceFolders { get; set; }

        public SqlDefinitionStorageContext(DbContextOptions<SqlDefinitionStorageContext> options) : base(options) { }

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
        }

        public override ValueTask DisposeAsync()
        {
            IsDisposed = true;
            return base.DisposeAsync();
        }
    }
}
