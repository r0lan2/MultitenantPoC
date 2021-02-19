using System;
using Farmer.Data;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasePathStrategySample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<FarmerDataContext>();

            services.AddDbContext<MultiTenantStoreDbContext>(options =>
                options.UseSqlServer(Configuration["DefaultConnection"]));

            

            services.AddMultiTenant<TenantInfo>()
                .WithEFCoreStore<MultiTenantStoreDbContext, TenantInfo>()
                .WithBasePathStrategy();

           



        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseMultiTenant();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{first_segment=}/{controller=Home}/{action=Index}");
            });
            
            SetupStore(app.ApplicationServices);

        }

        
        private async void SetupStore(IServiceProvider sp)
        {
            var scopeServices = sp.CreateScope().ServiceProvider;
            var store = scopeServices.GetRequiredService<IMultiTenantStore<TenantInfo>>();

            if (await store.TryGetAsync("tenant-customer1") == null)
                await store.TryAddAsync(new TenantInfo
                {
                    Id = "tenant-customer1", Identifier = "customer1", Name = "Customer 1",
                    ConnectionString = "customer1_conn_string"
                });

            if (await store.TryGetAsync("tenant-customer2") == null)
                await store.TryAddAsync(new TenantInfo
                    {Id = "tenant-customer2", Identifier = "customer2", Name = "Customer 2", ConnectionString = "customer2_conn_string" });
        }
    }
}