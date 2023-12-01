using System.ComponentModel.DataAnnotations;

namespace DTO.Request.AccountController;

public class CreateAccountRequest
{
    [Required(ErrorMessage = "用户名不能为空")]
    [MinLength(5, ErrorMessage = "用户名长度不能小于5")]
    [MaxLength(50, ErrorMessage = "用户名长度不能超过50")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "密码不能为空")] public string Password { get; set; }

    [EmailAddress] public string Email { get; set; }
}