using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TerritorialHQ.Areas.Administration.Pages.Home
{
    [Authorize(Roles ="Administrator, Staff, Journalist")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
