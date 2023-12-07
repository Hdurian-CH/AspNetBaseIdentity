using Masuit.Tools.Strings;
using Masuit.Tools.Systems;
using WebAPIApplication.Middleware.Error;
using WebAPIApplication.ServiceCollection.AuthService;
using WebAPIApplication.ServiceCollection.Cache;
using WebAPIApplication.ServiceCollection.DbContext;
using WebAPIApplication.ServiceCollection.Swagger;

#region 说明

//一切catch错误统一http状态码为400
//ErrorMessage统一在ErrorMessageDefine中定义
//code 定义 0:success，500:error

#endregion

var builder = WebApplication.CreateBuilder(args);

#region 雪花ID生成器

SnowFlakeNew.SetMachienId(1); // 设置机器id
SnowFlakeNew.SetInitialOffset(4219864516915105792); // 设置起始偏移量
SnowFlakeNew.SetNumberFormater(new NumberFormater("0123456789abcdefghijklmnopqrstuvwxyz._-!")); // 设置数制格式化器
#endregion

builder.Services.AddControllers();
builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowOrigin", policy => 
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:7247");
    });    
});
builder.Services.AddAuthDbContext(builder.Configuration);
builder.Services.AddAuthService(builder.Configuration);
builder.Services.AddCacheService(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*app.UseHttpsRedirection();*/

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorMiddleware>();

app.Run();