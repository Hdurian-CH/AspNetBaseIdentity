using System.ComponentModel.DataAnnotations;

namespace WebAPIApplication.Model.Auth;

public class User(string userName, string password, string email)
{
    [Key] public string UserId { get; set; }
    [MinLength(5)] [MaxLength(50)] public string UserName { get; set; } = userName;
    [MinLength(8)] [MaxLength(50)] public string Password { get; set; } = password;
    [EmailAddress] public string Email { get; set; } = email;
    public List<UserPermissions> UserPermissionsList { get; } = new();
}