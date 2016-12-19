using Gilmond.WindowsService.Portal.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using React.AspNet;

namespace Gilmond.WindowsService.Portal
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add service to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add framework service.
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddReact();
			services.AddMvc();
			services.AddOptions();
			services.AddMemoryCache();

			// Add application configuration
			services.Configure<MyOptions>(Configuration.GetSection("MyOptions"));
			// Add services for own types
			services.AddTransient<INetwork, Network>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			app.UseReact(config =>
			{
				config
				.SetLoadBabel(false)
				.AddScriptWithoutTransform("~/wwwroot/app.js")
				.SetJsonSerializerSettings(new JsonSerializerSettings
				{
					StringEscapeHandling = StringEscapeHandling.EscapeHtml,
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				});
			});

			app.UseStaticFiles();

			app.UseMvc();
		}
	}
}
