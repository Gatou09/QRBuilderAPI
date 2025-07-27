namespace QRBuilderAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QRBuilderAPI.Models;
using QRBuilderAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IConfiguration _config;

    public AuthController(UserService userService, IConfiguration config)
    {
        _userService = userService;
        _config = config;
    }

    /// <summary>
    /// Inscription d'un utilisateur.
    /// </summary>
    [HttpPost("register")]
    public IActionResult Register([FromBody] LoginRequestDto dto)
    {
        var success = _userService.Register(dto.Email, dto.Password);
        if (!success)
            return BadRequest("Utilisateur déjà existant.");

        return Ok("Compte créé !");
    }

    /// <summary>
    /// Connexion et génération du token JWT.
    /// </summary>
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto dto)
    {
        var user = _userService.Authenticate(dto.Email, dto.Password);
        if (user == null)
            return Unauthorized("Email ou mot de passe incorrect.");

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
