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
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<AmstramgramQuery>();
            services.AddScoped<AmstramgramMutation>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPictureRepository, PictureRepository>();
            services.AddTransient<ILikeRepository, LikeRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
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
            services.AddTransient<TagType>();
            services.AddTransient<TagInputType>();
            services.AddTransient<CommentType>();
            services.AddTransient<CommentInputType>();
            services.AddTransient<LikeType>();
            services.AddTransient<LikeInputType>();
            services.AddTransient<UserFollowerType>();
            services.AddTransient<UserFollowerInputType>();

            var sp = services.BuildServiceProvider();
            services.AddScoped<ISchema>(_ => new AmstramgramSchema(type => (GraphType)sp.GetService(type)) { Query = sp.GetService<AmstramgramQuery>(), Mutation = sp.GetService<AmstramgramMutation>() });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
                              ILoggerFactory loggerFactory, AmstramgramContext db)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(
                options => options.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            );

            app.UseStaticFiles();
            app.UseMvc();
            // db.EnsureSeedData();
        }
    }
}
