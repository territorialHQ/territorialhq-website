using Newtonsoft.Json;
using System.Net.Http;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Services;

public class AppUserRoleRelationService : ApisDtoService
{
    public AppUserRoleRelationService(IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(contextAccessor, configuration)
    {

    }
}
