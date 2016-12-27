using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApplication.Data;
using WebApplication.Models;
using WebApplication.Services;
using WebApplication.Middlewares;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
             

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, Role>(options => {
                options.User.AllowedUserNameCharacters = null;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext, int>()
                .AddUserStore<UserStore<User, Role, ApplicationDbContext, int>>()
                  .AddRoleStore<RoleStore<Role, ApplicationDbContext, int>>()
                //.AddRoleManager<RoleManager<Role>>()
                .AddDefaultTokenProviders();

            services.AddDistributedSqlServerCache(o =>
            {
                o.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                o.SchemaName = "dbo";
                o.TableName = "Sessions";
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(500000);
            });

            services.AddCloudscribePagination();

            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("HasPermission", policy => policy.Requirements.Add(new AuthorizationPolicies.HasPermissionRequirement()));
            });


            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                             .Database.Migrate();
                    }
                }
                catch { }

            }

     

            app.UseStaticFiles();

            //app.UseIdentity();

            app.UseCookieAuthentication();

            app.UseIdentity()
                //.UseFacebookAuthentication(new FacebookOptions
                //{
                //    AppId = "901611409868059",
                //    AppSecret = "4aa3c530297b1dcebc8860334b39668b"


                //    //AppId = "1448839281810879",
                //    //AppSecret = "7733ae8f75e2d3d18cf6fec6c791ad5d"
                //})
                .UseGoogleAuthentication(new GoogleOptions
                {
                    ClientId = "514485782433-fr3ml6sq0imvhi8a7qir0nb46oumtgn9.apps.googleusercontent.com",
                    ClientSecret = "V2nDD9SkFbvLTqAUBWBBxYAL"

                    //ClientId = "602092482899-l41fviq79kr7hd7lpju2qfp0skkvfdlv.apps.googleusercontent.com",
                    //ClientSecret = "K39xJM_AOOhVTpahCu6rxU6-"
                });
                //.UseTwitterAuthentication(new TwitterOptions
                //{
                //    ConsumerKey = "BSdJJ0CrDuvEhpkchnukXZBUv",
                //    ConsumerSecret = "xKUNuKhsRdHD03eLn67xhPAyE1wFFEndFo1X2UJaK2m1jdAxf4"

                //    //ConsumerKey = "OMSp7qx2V1Vv71RO6BNfLlb6T",
                //    //ConsumerSecret = "pWe7V0znSDUoL86puPOSPE3fq3J9rYsZLOw3evwjHIQfXPqUZy"
                //});


            app.UseInstaller();
            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
