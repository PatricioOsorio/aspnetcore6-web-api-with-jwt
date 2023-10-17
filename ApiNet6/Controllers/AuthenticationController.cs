using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ApiNet6.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ApiNet6.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthenticationController : ControllerBase
  {
    private readonly string _secretKey;

    public AuthenticationController(IConfiguration config)
    {
      _secretKey = config.GetSection("Settings").GetSection("SecretKey").ToString();
    }

    [HttpPost]
    [Route("Validate")]
    public IActionResult Validate([FromBody] User user)
    {
      if (user.Email == "patriciomiguel_12@hotmail.com" && user.Pass == "123")
      {
        var keyBytes = Encoding.ASCII.GetBytes(_secretKey);
        var claims = new ClaimsIdentity();

        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Email));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
          Subject = claims,
          Expires = DateTime.UtcNow.AddMinutes(5), // Expira en 5 min
          SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHadler = new JwtSecurityTokenHandler();
        var tokenConfig = tokenHadler.CreateToken(tokenDescriptor);

        string createdToken = tokenHadler.WriteToken(tokenConfig);

        return StatusCode(StatusCodes.Status200OK, new { token = createdToken });
      }
      else
      {
        return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });

      }
    }
  }
}
