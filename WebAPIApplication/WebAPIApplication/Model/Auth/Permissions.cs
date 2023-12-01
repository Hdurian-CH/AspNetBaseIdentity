using System.ComponentModel.DataAnnotations;

namespace WebAPIApplication.Model.Auth;

public class Permissions(string controllerName, ICollection<string> routers)
{
    [Key] public string PermissionsId { get; set; }
    [MinLength(1)] [MaxLength(60)] public string ControllerName { get; set; } = controllerName;
    public ICollection<string> Routers { get; set; } = routers;
    public List<UserPermissions> UserPermissionsList { get; } = new();
}