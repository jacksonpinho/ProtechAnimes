
using System.ComponentModel.DataAnnotations;

public class Register
{
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "As senhas não conferem.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}
