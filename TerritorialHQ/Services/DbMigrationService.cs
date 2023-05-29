using Microsoft.EntityFrameworkCore;
using TerritorialHQ.Data;

namespace TerritorialHQ.Services
{
    public static class DbMigrationService
    {
        public static void MigrationInitialization(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ApplicationDbContext>()?.Database.Migrate();
            }
        }
    }
}
