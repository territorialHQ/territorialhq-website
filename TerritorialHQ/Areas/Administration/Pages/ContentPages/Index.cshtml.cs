using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.ContentPages
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly LoggerService _logger;
        private readonly ContentPageService _service;

        public IndexModel(IMapper mapper, LoggerService logger, ContentPageService service)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
        }

        public IList<ContentPage>
            ContentPage
        { get; set; }

        public async Task OnGetAsync()
        {
            ContentPage = await _service.GetAllAsync();
        }
    }
}
