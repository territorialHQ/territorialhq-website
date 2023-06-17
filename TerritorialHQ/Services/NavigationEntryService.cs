using TerritorialHQ.Services.Base;

namespace TerritorialHQ.Services
{
    public class NavigationEntryService : ApisDtoService
    {
        public NavigationEntryService(IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(contextAccessor, configuration)
        {
        }
    }
}
