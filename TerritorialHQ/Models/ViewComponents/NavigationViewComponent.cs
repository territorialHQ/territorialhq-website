using Microsoft.AspNetCore.Mvc;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Models.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly ApisService _service;

        public NavigationViewComponent(ApisService service) 
        { 
            _service = service;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View((await _service.GetAllAsync<NavigationEntry>("NavigationEntry")).Where(e => e.ParentId == null).ToList());
        }

    }
}
