using SqlDefinitionStorageExample.EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace SqlDefinitionStorageExample.EFCore
{
    public class SqlDefinitionStorageContext : DbContext
    {

        public DbSet<Report> Reports { get; set; }

        public DbSet<ReportFolder> ReportFolders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = @"Server=.\SQLEXPRESS;Database=DefinitionStorage;Trusted_Connection=True;TrustServerCertificate=True;";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
