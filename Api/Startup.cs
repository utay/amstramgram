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
            services.AddMvc();
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

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

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Facebook:AppSecret"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
                              ILoggerFactory loggerFactory, AmstramgramContext db)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(
                options => options.WithOrigins("http://localhost:8080", "http://localhost:5000").AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            );

            app.UseStaticFiles();
            app.UseMvc();

            var optionsRewrite = new RewriteOptions()
                              .AddRedirectToHttps(StatusCodes.Status301MovedPermanently, 63423);
            app.UseRewriter(optionsRewrite);


            // db.EnsureSeedData();
        }
    }
}
