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
        private ITenantInfo TenantInfo { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        //public FarmerDataContext(DbContextOptions options) : base(options)
        //{
        //}


        //Investigate how to have more than one construxtion with same number of parameters
        //I had to comment this constructor when I ran add-migration
        public FarmerDataContext(TenantInfo tenantInfo)
        {
            // DI will pass in the tenant info for the current request.
            // ITenantInfo is also injectable.
            TenantInfo = tenantInfo;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use the connection string to connect to the per-tenant database.
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(TenantInfo.ConnectionString);
        }

    }


    //public class ContextFactoryForMigration : IDesignTimeDbContextFactory<FarmerDataContext>
    //{

    //    public FarmerDataContext CreateDbContext(string[] args)
    //    {
    //        IConfigurationRoot configuration = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appSettings.json")
    //            .Build();

    //        var connectionString = configuration.GetConnectionString("DefaultConnection");
    //        var optionsBuilder = new DbContextOptionsBuilder<FarmerDataContext>();
    //        optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Farmer.Data"));

    //        return new FarmerDataContext(optionsBuilder.Options);
    //    }
    //}

}
