using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Core.Data;
using Api.Models;
using Data;
using Data.Repositories;
using Data.Seed;
using Microsoft.EntityFrameworkCore;
using GraphQL.Types;
using GraphQL;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Facebook;
using Microsoft.AspNetCore.Authentication.Facebook;
using Api.Helper;
using System;
using Microsoft.AspNetCore.HttpOverrides;
using Api.Controllers;
//using Core.Logic;

namespace Api
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
            Env = env;
        }

        public IConfigurationRoot Configuration { get; }
        private IHostingEnvironment Env { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Add framework services.

            services.AddCors();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = Configuration["ConnectionStrings:AmstramgramDatabaseConnection"];
                options.SchemaName = "dbo";
                options.TableName = "Session";
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.Secure = CookieSecurePolicy.SameAsRequest;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromHours(5);
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                options.Cookie.Path = "/";
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ConnectionFilter));
            });
            services.AddSingleton<IConfiguration>(Configuration);


            /*services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });*/



            services.AddAutoMapper(typeof(Startup));
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };


            services.AddScoped<AmstramgramQuery>();
            services.AddScoped<AmstramgramMutation>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPictureRepository, PictureRepository>();
            services.AddTransient<ILikeRepository, LikeRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IUserFollowerRepository, UserFollowerRepository>();

            if (Env.IsEnvironment("Test"))
            {
                services.AddDbContext<AmstramgramContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "Amstramgram"));
            }
            else
            {
                services.AddDbContext<AmstramgramContext>(options =>
                    options.UseSqlServer(Configuration["ConnectionStrings:AmstramgramDatabaseConnection"]));
            }

            services.AddScoped<IDocumentExecuter, DocumentExecuter>();

            services.AddTransient<UserType>();
            services.AddTransient<UserInputType>();
            services.AddTransient<PictureType>();
            services.AddTransient<PictureInputType>();
            services.AddTransient<CommentType>();
            services.AddTransient<CommentInputType>();
            services.AddTransient<LikeType>();
            services.AddTransient<LikeInputType>();
            services.AddTransient<UserFollowerType>();
            services.AddTransient<UserFollowerInputType>();
            services.AddTransient<TagType>();
            services.AddTransient<TagInputType>();

            var sp = services.BuildServiceProvider();
            services.AddScoped<ISchema>(_ => new AmstramgramSchema(type => (GraphType)sp.GetService(type)) { Query = sp.GetService<AmstramgramQuery>(), Mutation = sp.GetService<AmstramgramMutation>() });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<AmstramgramContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthentication()
            .AddCookie("ConnectionCookie")
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Facebook:AppSecret"];
                facebookOptions.BackchannelHttpHandler = new FacebookBackChannelHandler();
                facebookOptions.Scope.Add("email");
                facebookOptions.SaveTokens = true;
                facebookOptions.TokenEndpoint = "https://graph.facebook.com/v3.0/oauth/access_token";
                facebookOptions.UserInformationEndpoint = "https://graph.facebook.com/v3.0/me?fields=id,name,email,first_name,last_name";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
                              ILoggerFactory loggerFactory, AmstramgramContext db)
        {
            var forwardedHeadersOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                RequireHeaderSymmetry = false
            };

            forwardedHeadersOptions.KnownNetworks.Clear();
            forwardedHeadersOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardedHeadersOptions);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(
                options => options.WithOrigins("http://localhost:8080", "http://localhost:5000", "https://app.amstramgram.insideapp.io").AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            );

            app.UseSession();

            app.Use((httpContext, nextMiddleware) =>
            {
                httpContext.Session.SetObject("_SessionKey", String.Empty);
                httpContext.Session.SetObject("_SessionDate", DateTime.Now);

                return nextMiddleware();
            });

            app.UseStaticFiles();

            app.UseMvc();

            app.UseCookiePolicy();

            /*var optionsRewrite = new RewriteOptions()
                              .AddRedirectToHttps(StatusCodes.Status301MovedPermanently, 63423);*/

            //app.UseRewriter(optionsRewrite);
            AppHttpContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            // db.EnsureSeedData();
        }
    }
}
