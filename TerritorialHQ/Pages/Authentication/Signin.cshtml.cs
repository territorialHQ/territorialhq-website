using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TerritorialHQ.Pages.Authentication
{
    public class SigninModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public SigninModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGet(string? token)
        {
            if (token == null)
                throw new Exception("Missing token.");

            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = ConfigurationBinder.GetValue<string>(_configuration, "JWT_ISSUER"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationBinder.GetValue<string>(_configuration, "JWT_ENCRYPTION_KEY")))
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                var jwtClaims = (JwtSecurityToken)validatedToken;

                var claims = new List<Claim>
                {
                    new Claim("Id", jwtClaims.Claims.FirstOrDefault(c => c.Type == "Id")?.Value ?? string.Empty),
                    new Claim(ClaimTypes.Name, jwtClaims.Claims.FirstOrDefault(c => c.Type == "DiscordId")?.Value ?? string.Empty),
                    new Claim(ClaimTypes.Role, jwtClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Store the token in a cookie  
                CookieOptions cookieOptions = new()
                {
                    Expires = DateTime.Now.AddHours(8),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true
                };
                Response.Cookies.Append("BearerToken", token, cookieOptions);

            }
            catch (SecurityTokenValidationException)
            {
                throw new SecurityTokenValidationException("The provided token in invalid");
            }

            var jwtToken = tokenHandler.ReadJwtToken(token);

            return RedirectToPage("/Index");
        }
    }
}
