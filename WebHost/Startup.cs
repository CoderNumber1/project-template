using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector.Lifestyles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using SimpleInjector.Integration.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using SimpleInjector;
using Microsoft.Extensions.Logging;
using InsertNamespace.Logging;
using IdentityServer4.Services;
using System.IdentityModel.Tokens.Jwt;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4.Test;

namespace InsertNamespace
{
    public class Startup
    {
        private Container container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";

                    options.Authority = "https://localhost:5000/identity";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "mvc";
                    options.SaveTokens = true;
                });

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5000/identity";
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "api1";
                });

            IntegrateSimpleInjector(services);

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Identity.InMemoryConfiguration.GetIdentityResources())
                .AddInMemoryApiResources(Identity.InMemoryConfiguration.GetApiResources())
                .AddInMemoryClients(Identity.InMemoryConfiguration.GetClients())
                .AddTestUsers(Identity.InMemoryConfiguration.GetUsers());
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(container));
            services.AddSingleton<IViewComponentActivator>(
                new SimpleInjectorViewComponentActivator(container));

            services.EnableSimpleInjectorCrossWiring(container);
            services.UseSimpleInjectorAspNetRequestScoping(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory factory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.Map(
                PathString.FromUriComponent("/identity"),
                (a) =>
                {
                    a.UseStaticFiles();
                    a.UseIdentityServer();
                    a.UseMvc(routes =>
                    {
                        routes.MapRoute(
                            name: "default",
                            template: "{controller=Home}/{action=Index}/{id?}");
                    });
                });

            app.UseStaticFiles();

            InitializeContainer(app);

            //container.Register<CustomMiddleware1>();
            //container.Register<CustomMiddleware2>();

            //container.Verify();

            //// Add custom middleware
            //app.Use((c, next) => container.GetInstance<CustomMiddleware1>().Invoke(c, next));
            //app.Use((c, next) => container.GetInstance<CustomMiddleware2>().Invoke(c, next));

            // ASP.NET default stuff here
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            // Add application presentation components:
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            // Add application services. For instance:
            //container.Register<IUserService, UserService>(Lifestyle.Scoped);
            container.RegisterSingleton<ILog, Log>();
            container.RegisterSingleton<ILogEventFactory, LogEventFactory>();
            container.RegisterSingleton<IContextProvider, WebContextProvider>();

            // Cross-wire ASP.NET services (if any). For instance:
            container.CrossWire<ILoggerFactory>(app);
            container.CrossWire<IHttpContextAccessor>(app);

            container.CrossWire<IIdentityServerInteractionService>(app);
            container.CrossWire<IClientStore>(app);
            container.CrossWire<IAuthenticationSchemeProvider>(app);
            container.CrossWire<IEventService>(app);
            container.CrossWire<TestUserStore>(app);
            container.CrossWire<IResourceStore>(app);

            // NOTE: Do prevent cross-wired instances as much as possible.
            // See: https://simpleinjector.org/blog/2016/07/
        }
    }
}
