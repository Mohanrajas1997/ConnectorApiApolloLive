using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MysqlEfCoreDemo.Data;
using NPOI.SS.Formula.Functions;
using Serilog;
using System.IO;
using System.Reflection;

namespace MysqlEfCoreDemo
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>(options => options.UseMySQL(Configuration.GetConnectionString("Mysql")));
            //services.AddDbContext<MyDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("sqllite")));
            //services.AddDbContext<MyDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("Postgres")));

            services.AddControllersWithViews();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            services.AddScoped<MyDbContext>();
            // XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }


    }
}
