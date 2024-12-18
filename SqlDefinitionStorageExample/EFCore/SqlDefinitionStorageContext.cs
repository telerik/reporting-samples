using SqlDefinitionStorageExample.EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace SqlDefinitionStorageExample.EFCore
{
    public class SqlDefinitionStorageContext : DbContext
    {

        public DbSet<Resource> Resources { get; set; }

        public DbSet<ResourceFolder> ResourceFolders { get; set; }

        public DbSet<SharedDataSource> SharedDataSources { get; set; }

        public SqlDefinitionStorageContext(DbContextOptions<SqlDefinitionStorageContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = @"Server=.\SQLEXPRESS;Database=ResourcesStorage;Trusted_Connection=True;TrustServerCertificate=True;";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
