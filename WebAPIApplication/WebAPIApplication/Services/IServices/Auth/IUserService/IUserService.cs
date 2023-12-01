using OneOf;
using OneOf.Types;
using WebAPIApplication.Model.Auth;

namespace WebAPIApplication.Services.IServices.Auth.IUserService;

public interface IUserService
{
    public Task<OneOf<True, (False, string)>> CreateUser(User user);
    public Task<OneOf<User, (False, string)>> FindUserById(string id);

    public Task<OneOf<(True, string), (False, string)>> CheckUserByUserNameAndPasswordHash(string userName,
        string passwordHash);
}