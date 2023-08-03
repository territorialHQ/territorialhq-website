using TerritorialHQ.Services.Base;

namespace TerritorialHQ.Services;

public class ContentCreatorService : ApisDtoService
{
    public ContentCreatorService(IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(contextAccessor, configuration)
    {
    }
}
