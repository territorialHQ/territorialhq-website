using TerritorialHQ.Services.Base;

namespace TerritorialHQ.Services;

public class ContentPageService : ApisDtoService
{
    public ContentPageService(IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(contextAccessor, configuration)
    {
    }
}
