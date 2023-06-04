using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace TerritorialHQ.Pages.ExternalLogin
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string Client { get; set; }
        public string Token { get; set; }

        public IActionResult OnGet(string client)
        {
            if (client == null)
                return NotFound();

            Client = client;

            var issuer = ConfigurationBinder.GetValue<string>(_configuration, "JWT_ISSUER");
            var audience = issuer;
            var key = ConfigurationBinder.GetValue<string>(_configuration, "JWT_ENCRYPTION_KEY");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();
            claims.Add(new Claim("DiscordId", User.Identity.Name));

            var token = new JwtSecurityToken(
                    issuer,
                    audience,
                    claims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: credentials
            );

            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);

            Token = jwt_token;

            return Page();
        }
    }
}
