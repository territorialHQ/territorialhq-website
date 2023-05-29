using Microsoft.AspNetCore.Mvc;
using TerritorialHQ.Services;

namespace TerritorialHQ.Models.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly NavigationEntryService _navigationEntryService;

        public NavigationViewComponent(NavigationEntryService navigationEntryService) 
        { 
            _navigationEntryService = navigationEntryService;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _navigationEntryService.GetTopLevelAsync());
        }

    }
}
