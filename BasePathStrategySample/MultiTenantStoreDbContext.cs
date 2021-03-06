﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BasePathStrategySample
{
    public class MultiTenantStoreDbContext : EFCoreStoreDbContext<TenantInfo>
    {
        public MultiTenantStoreDbContext(DbContextOptions options) : base(options)
        {
        }
    }

    public class ContextFactoryForMigration : IDesignTimeDbContextFactory<MultiTenantStoreDbContext>
    {

        public MultiTenantStoreDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();

            var connectionString = configuration["DefaultConnection"];
            var optionsBuilder = new DbContextOptionsBuilder<MultiTenantStoreDbContext>();
            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("BasePathStrategySample"));

            return new MultiTenantStoreDbContext(optionsBuilder.Options);
        }
    }



}
