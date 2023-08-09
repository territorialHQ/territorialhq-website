using Microsoft.AspNetCore.Authentication.Cookies;
using TerritorialHQ.Mapping;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;

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

            // Add services

            builder.Services.AddSingleton(typeof(DiscordBotService));
            builder.Services.AddSingleton(typeof(LoggerService));

            builder.Services.AddScoped(typeof(BotEndpointService));
            builder.Services.AddScoped(typeof(ChartDataService));
            builder.Services.AddScoped(typeof(ApisService));
            builder.Services.AddScoped(typeof(AppUserService));
            builder.Services.AddScoped(typeof(ClanService));
            builder.Services.AddScoped(typeof(ClanUserRelationService));
            builder.Services.AddScoped(typeof(ContentPageService));
            builder.Services.AddScoped(typeof(NavigationEntryService));
            builder.Services.AddScoped(typeof(JournalArticleService));
            builder.Services.AddScoped(typeof(ContentCreatorService));
            builder.Services.AddScoped(typeof(ClanRelationService));
            builder.Services.AddScoped(typeof(AppUserRoleRelationService));

            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add controller mapping functions
            var mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ContentPageProfile());
            });
            var mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);


            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(6);
                options.SlidingExpiration = false;
                options.AccessDeniedPath = "/Forbidden/";
                options.LoginPath = "/Authentication/Login";
                options.LogoutPath = "/Authentication/Logout";
            });

            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AddPageRoute("/Profiles/Index", "/Profiles/{id?}");
                options.Conventions.AddPageRoute("/ContentPages/Details", "{*url}");
            })
            .AddRazorRuntimeCompilation();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //app.UseHsts();
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