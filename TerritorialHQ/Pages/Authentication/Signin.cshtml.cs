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
        SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;

        public SigninModel(SignInManager<IdentityUser> signInManager, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _contextAccessor = contextAccessor;
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
                    new Claim(ClaimTypes.Name, jwtClaims.Claims.FirstOrDefault(c => c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value),
                    new Claim(ClaimTypes.Role, jwtClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Store the token in a cookie  
                CookieOptions cookieOptions = new CookieOptions()
                {
                    Expires = DateTime.Now.AddHours(8),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true
                };
                Response.Cookies.Append("BearerToken", token, cookieOptions);

            }
            catch (SecurityTokenValidationException ex)
            {
                throw new SecurityTokenValidationException("The provided token in invalid");
            }

            var jwtToken = tokenHandler.ReadJwtToken(token);


            //var c = _contextAccessor.HttpContext;
            //await c.SignInAsync(principal);

            return RedirectToPage("/Index");
        }
    }
}
