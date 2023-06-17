
using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static string? GetId(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims?.FirstOrDefault(c => c.Type == "Id")?.Value ?? string.Empty;
    }
}

