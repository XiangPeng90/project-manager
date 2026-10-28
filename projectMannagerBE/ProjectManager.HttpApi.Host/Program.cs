using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Services;
using ProjectManager.EntityFrameworkCore;
using ProjectManager.HttpApi.Host.Services;
using Serilog;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>((context, cb) =>
{
    cb.RegisterModule(new ApplicationModule());
    cb.RegisterModule(new InfrastructureModule());
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
    options.InstanceName = "ProjectManager:";
});

// JWT 认证
var jwtSecret = builder.Configuration["Jwt:SecretKey"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjectManager API", Version = "v1" });

    // 添加 JWT Bearer 认证定义
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "输入登录接口返回的 JWT Token（不需要手动加 Bearer 前缀）"
    });

    // 全局要求认证（Swagger UI 里每个接口都会带这个安全要求）
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddAuthorization();
builder.Services.AddControllers().AddControllersAsServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Program.cs 里，app.Run() 之前
// Console.WriteLine(">>> Seeding start");
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<ProjectManagerDbContext>();

//     Console.WriteLine($">>> Users count: {db.Users.Count()}, Roles count: {db.Roles.Count()}");
//     // 1. 先插角色（如果不存在）
//     if (!db.Roles.Any())
//     {
//         db.Roles.AddRange(
//             new Role { Code = "Admin", Name = "系统管理员", Description = "拥有全部权限", IsActive = true },
//             new Role { Code = "ProjectManager", Name = "项目经理", Description = "管理项目与工作包", IsActive = true },
//             new Role { Code = "DepartmentManager", Name = "部门经理", Description = "查看报表与审批", IsActive = true },
//             new Role { Code = "User", Name = "普通用户", Description = "只读权限", IsActive = true }
//         );
//         db.SaveChanges();
//     }

//     // 2. 再插 admin 用户（如果不存在）
//     if (!db.Users.Any(u => u.Username == "admin"))
//     {
//         db.Users.Add(new User
//         {
//             Username = "admin",
//             PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
//             DisplayName = "管理员",
//             Status = true,
//             CreatedAt = DateTime.UtcNow
//         });
//         db.SaveChanges();
//     }

//     // 3. 绑定 admin 到 Admin 角色（如果还没绑定）
//     var adminUser = db.Users.First(u => u.Username == "admin");
//     var adminRole = db.Roles.First(r => r.Code == "Admin");
//     if (!db.UserRoles.Any(ur => ur.UserId == adminUser.Id && ur.RoleId == adminRole.Id))
//     {
//         db.UserRoles.Add(new UserRole { UserId = adminUser.Id, RoleId = adminRole.Id });
//         db.SaveChanges();
//     }
// }
// Console.WriteLine(">>> Seeding end");

app.Run();
