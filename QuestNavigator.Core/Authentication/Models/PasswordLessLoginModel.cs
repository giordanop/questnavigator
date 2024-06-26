using System.ComponentModel.DataAnnotations;

namespace QuestNavigator.Core.Authentication.Models;
public class PasswordLessLoginModel
{
    [Required]
    public string Email { get; set; } = null!;
    public string? RedirectUrl { get; set; }
}

