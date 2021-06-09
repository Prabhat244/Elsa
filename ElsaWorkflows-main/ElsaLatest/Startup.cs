using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.SqlServer;
using Elsa.Samples.UserRegistration.Web.Handlers;
using Elsa.Samples.UserRegistration.Web.Persistence;
using Elsa.Samples.UserRegistration.Web.Services;
using ElsaDashboard.Backend.Extensions;
using Elsa.Activities.UserTask.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Elsa;
using ElsaLatest.Activities;
using Elsa.Samples.UserRegistration.Web.Extensions;

namespace Elsa.Samples.UserRegistration.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddRazorPages();
            services.AddServerSideBlazor();

            Uri elsaServerUrl = Configuration.GetValue<Uri>("Elsa:HostUrl");

            services
                // Add Elsa services. 
                .AddElsa(elsa =>
                {
                    elsa
                    .UseEntityFrameworkPersistence<SampleDbContext>((builder) =>
                    {
                        builder.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
                    }, true)
                    .AddEmailActivities(Configuration.GetSection("Elsa:Smtp").Bind)
                    .AddHttpActivities(Configuration.GetSection("Elsa:Http").Bind)
                    .AddQuartzTemporalActivities()
                    .AddJavaScriptActivities()
                    .AddConsoleActivities()
                    .AddUserTaskActivities()
                    .AddUserActivities()
                    .AddWorkflowsFrom<Startup>()
                    .AddActivity<SqlQuery>()
                    .AddActivity<GetLevel1Services>()
                    .AddActivity<GetLevel2Services>()
                    .AddActivity<GetTicketDetails>()
                    .AddActivity<UserInputTask>();
                });

            services
              .AddElsaApiEndpoints()
              .AddElsaSwagger();

            // Add Elsa Dashboard services.
            services.AddElsaDashboardUI(options => options.ElsaServerUrl = elsaServerUrl)
                .AddElsaDashboardBackend(options => options.ServerUrl = elsaServerUrl)

                // Add our PasswordHasher service.
                .AddSingleton<IPasswordHasher, PasswordHasher>();

            services
               // Add activity Options provider
               //.AddOptionsProviders()
               // Add MongoCollections services
               .AddDbContext<SampleDbContext>()
               // Add our liquid handler.
               .AddNotificationHandlers(typeof(LiquidConfigurationHandler));

            services.AddCors(cors => cors.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("Content-Disposition")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Elsa"));

                //if (Program.RuntimeModel == BlazorRuntimeModel.WebAssembly)
                //app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors();
            // Add Elsa's middleware to handle HTTP requests to workflows.  
            app.UseHttpActivities();
            app.UseRouting();

            app.UseEndpoints(
                endpoints =>
                {
                    // Attribute-based routing stuff.
                    endpoints.MapControllers();
                    // Blazor stuff.
                    endpoints.MapBlazorHub();
                    endpoints.MapFallbackToPage("/_Host");

                });
        }
    }
}