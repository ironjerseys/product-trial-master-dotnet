using System.ComponentModel.DataAnnotations;

namespace product_trial_master_dotnet.Models;

public class LoginRequest
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

