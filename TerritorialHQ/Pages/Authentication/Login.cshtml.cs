using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TerritorialHQ.Pages.Authentication
{
    public class LoginModel : PageModel
    {
        private IConfiguration _configuration;
        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;

            LoginUrl = ConfigurationBinder.GetValue<string>(configuration, "APIS_URI");
            if (LoginUrl == null)
                throw new Exception("APIS URL not set!");
        }

        public string? LoginUrl { get; set; }

        public void OnGet()
        {

        }
    }
}
