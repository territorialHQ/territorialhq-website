using Microsoft.AspNetCore.Mvc;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Models.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly NavigationEntryService _service;

        public NavigationViewComponent(NavigationEntryService service) 
        { 
            _service = service;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = (await _service.GetAllAsync<DTONavigationEntry>("NavigationEntry")).Where(e => e.ParentId == null).ToList();
            return View(model);
        }

    }
}
