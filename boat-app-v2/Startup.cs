using boat_app_v2.BusinessLogic;
using boat_app_v2.BusinessLogic.Repository;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using JavaScriptEngineSwitcher.V8;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using React.AspNet;

namespace boat_app_v2
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            //Configuration = configuration;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddReact();
            
            services.AddAutoMapper(typeof(Program));
            
            var myDatabaseName = "mydatabase_"+DateTime.Now.ToFileTimeUtc();

            services.AddDbContext<BoatContext>(o => o.UseInMemoryDatabase(databaseName: myDatabaseName).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)); //;
        
            services.AddScoped<IRepositoryController, RepositoryController>();
            
            services.AddJsEngineSwitcher(options => options.DefaultEngineName = V8JsEngine.EngineName)
                .AddV8();
            services.AddControllersWithViews();
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

            //   app.UseHttpsRedirection();
            //   app.UseHttpsRedirection();
            
            // Initialise ReactJS.NET. Must be before static files.
            app.UseReact(config =>
            {
                config
                 .AddScript("~/js/tutorial.jsx").SetJsonSerializerSettings(new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            });
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

