using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LacmusPlugin;
using LacmusPluginsRepository.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LacmusPluginsRepository
{
    public class Startup
    {
        private List<IObjectDetectionPlugin> _plugins;
        private readonly PluginManager _pluginManager;
        private string _packagesBaseDirectory;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _pluginManager = new PluginManager(Path.Join(configuration["PluginsBaseDirectory"], "src"));
            _packagesBaseDirectory = Path.Join(configuration["PluginsBaseDirectory"], "packages");
            _plugins = _pluginManager.FindPlugins();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEnumerable<IObjectDetectionPlugin>, List<IObjectDetectionPlugin>>(
                p => _plugins);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lacmus Plugin Repository", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "plugin-repository/docs/{documentname}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "API v1");
                c.RoutePrefix = "plugin-repository/docs";
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}