using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Farmer.Data.Model;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Farmer.Data
{
    public class FarmerDataContext : DbContext
    {
        public ITenantInfo TenantInfo { get; }
        public DbSet<Transaction> Transactions { get; set; }
        
        public FarmerDataContext(ITenantInfo tenantInfo)
        {
            TenantInfo = tenantInfo;
        }

        public FarmerDataContext(ITenantInfo tenantInfo, DbContextOptions<FarmerDataContext> options) : base( options)
        {
            TenantInfo = tenantInfo;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(TenantInfo.ConnectionString);
        }

    }


    public class ContextFactoryForMigration : IDesignTimeDbContextFactory<FarmerDataContext>
    {

        public FarmerDataContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<FarmerDataContext>();
            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Farmer.Data"));

            return new FarmerDataContext(null,optionsBuilder.Options);
        }
    }

}
