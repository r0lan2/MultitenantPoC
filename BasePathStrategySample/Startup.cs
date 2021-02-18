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

            services.AddMultiTenant<TenantInfo>()
                .WithEFCoreStore<MultiTenantStoreDbContext, TenantInfo>()
                .WithBasePathStrategy();

            services.AddScoped<FarmerDataContext>();



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

            if (await store.TryGetAsync("tenant-camanchaca") == null)
                await store.TryAddAsync(new TenantInfo
                {
                    Id = "tenant-camanchaca", Identifier = "camanchaca", Name = "Camanchaca",
                    ConnectionString = "camanchaca_conn_string"
                });

            if (await store.TryGetAsync("tenant-mowi") == null)
                await store.TryAddAsync(new TenantInfo
                    {Id = "tenant-mowi", Identifier = "mowi", Name = "Mowi", ConnectionString = "mowi_conn_string"});
        }
    }
}