using System.ComponentModel.DataAnnotations;

namespace ProductManager.Api.Models.Auth;

public class RegisterRequest
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(4, ErrorMessage = "La contraseña debe tener al menos 4 caracteres.")]
    public string Password { get; set; } = string.Empty;
}