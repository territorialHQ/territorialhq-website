using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerritorialHQ.Data;
using TerritorialHQ.Mapping;
using TerritorialHQ.Services;

namespace TerritorialHQ
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
        
            // Create database connection string from environment variables
            var db_server = builder.Configuration.GetValue<string>("DB_SERVER");
            var db_port = builder.Configuration.GetValue<string>("DB_PORT") ?? "3306";
            var db_cat = builder.Configuration.GetValue<string>("DB_CATALOGUE");
            var db_user = builder.Configuration.GetValue<string>("DB_USER");
            var db_pw = builder.Configuration.GetValue<string>("DB_PASSWORD");

            var connectionString = $"Server={db_server};Port={db_port};Database={db_cat};Uid={db_user};Pwd={db_pw};";

            // Add database context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options
                    .UseLazyLoadingProxies()
                    .UseMySQL(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Add Identity context (user account system)
            builder.Services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Enable forced user signout for role management
            builder.Services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });

            // Add services
            builder.Services.AddSingleton(typeof(DiscordBotService));
            builder.Services.AddSingleton(typeof(LoggerService));
            
            builder.Services.AddScoped(typeof(ClanService));
            builder.Services.AddScoped(typeof(ClanUserRelationService));
            builder.Services.AddScoped(typeof(NavigationEntryService));
            builder.Services.AddScoped(typeof(ContentPageService));
            builder.Services.AddScoped(typeof(JournalArticleService));

            builder.Services.AddMemoryCache();

            // Add controller mapping functions
            var mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ContentPageProfile());
            });
            var mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);


            // Enable Discord OAth login
            builder.Services.AddAuthentication().AddDiscord(options =>
            {
                options.ClientId = builder.Configuration.GetValue<string>("DISCORD_CLIENTID") ?? string.Empty;
                options.ClientSecret = builder.Configuration.GetValue<string>("DISCORD_CLIENTSECRET") ?? string.Empty;
                options.CallbackPath = "/signin-discord";
            });

            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AddPageRoute("/ContentPages/Details", "{*url}");
            })
            .AddRazorRuntimeCompilation();

            var app = builder.Build();
            
            // Migrate the database if there are pending updates that have not yet been applied
            DbMigrationService.MigrationInitialization(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                MinimumSameSitePolicy = SameSiteMode.Lax
            });

            // Standard stuff

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();
            app.Run();
        }
    }
}