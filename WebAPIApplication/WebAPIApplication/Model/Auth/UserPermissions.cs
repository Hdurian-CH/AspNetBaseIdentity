namespace WebAPIApplication.Model.Auth;

public class UserPermissions
{
    public string UserId { get; set; }

    public string PermissionsId { get; set; }

    public User User { get; set; }

    public Permissions Permissions { get; set; }
}