using System.ComponentModel.DataAnnotations;
using Masuit.Tools.Systems;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebAPIApplication.DbContext.Auth;
using WebAPIApplication.ErrorMessageDefine.Account;
using WebAPIApplication.ErrorMessageDefine.Auth;
using WebAPIApplication.ErrorMessageDefine.DatabaseError;
using WebAPIApplication.ErrorMessageDefine.UnKnowError;
using WebAPIApplication.Model.Auth;
using WebAPIApplication.Services.IServices.Auth.IUserService;

namespace WebAPIApplication.Services.Auth.UserService;

/// <summary>
///     用户账号Service
/// </summary>
/// <param name="authDb"></param>
public class UserService(AuthDbContext authDb) : IUserService
{
    private AuthDbContext AuthDb { get; } = authDb;

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<OneOf<True, (False, string)>> CreateUser(User user)
    {
        //验证数据模型
        var modelContext = new ValidationContext(user);
        var errors = new List<ValidationResult>();
        var modelValid = Validator.TryValidateObject(user, modelContext, errors);
        if (!modelValid) return (new False(), UserErrorMessages.UserModelValidError);
        //查找是否存在同一个UserName用户
        var userExisted = await AuthDb.Users.AnyAsync(x => x.UserName == user.UserName);
        if (userExisted)
            return (new False(), UserErrorMessages.UserExistError);
        //雪花算法生成ID
        user.UserId = SnowFlakeNew.NewId;
        await AuthDb.Users.AddAsync(user);
        //EF Core保存
        try
        {
            await AuthDb.SaveChangesAsync();
            return new True();
        }
        catch (Exception e)
        {
            return e switch
            {
                DbUpdateException => (new False(), BaseDatabaseErrorMessage.DatabaseSaveError),
                _ => (new False(), DatabaseUnKnowErrorMessages.DatabaseUnknownSaveError)
            };
        }
    }

    /// <summary>
    ///     根据给出的Id查找用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<OneOf<User, (False, string)>> FindUserById(string id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     数据库检索用户名以及sha256处理后的密码
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="passwordHash"></param>
    /// <returns></returns>
    public async Task<OneOf<(True, string), (False, string)>> CheckUserByUserNameAndPasswordHash(string userName,
        string passwordHash)
    {
        var user = await AuthDb.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user is null)
            return (new False(), UserErrorMessages.UserNotExistError);
        if (user.Password != passwordHash)
            return (new False(), AuthErrorMessages.PassWordError);
        return (new True(), user.UserId);
    }
}