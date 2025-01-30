using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using product_trial_master_dotnet.Data;
using product_trial_master_dotnet.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace product_trial_master_dotnet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly string _privateKey;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _privateKey = configuration["Keys:PrivateKey"];
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] Models.LoginRequest login)
    {
        if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
        {
            return BadRequest(new { message = "Aresse Email et mot de passe requis." });
        }

        var user = _context.Users.FirstOrDefault(u => u.Email == login.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
        {
            return Unauthorized(new { message = "Identifiants incorrects." });
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_privateKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new[] {
            new System.Security.Claims.Claim("id", user.Id.ToString())
        }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new { Token = tokenHandler.WriteToken(token) });
    }



    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User newUser)
    {
        // Vérifiez si un utilisateur avec le même username existe déjà
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
        if (existingUser != null)
        {
            return BadRequest(new { message = "Nom d'utilisateur déjà utilisé." });
        }

        // Hash du mot de passe avant de l'enregistrer (bonne pratique)
        newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Utilisateur enregistré avec succès." });
    }

}
