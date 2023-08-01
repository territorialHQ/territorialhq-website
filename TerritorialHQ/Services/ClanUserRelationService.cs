using Newtonsoft.Json;
using System.Data;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Services;

public class ClanUserRelationService : ApisDtoService
{
    public ClanUserRelationService(IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(contextAccessor, configuration)
    {
    }

}
