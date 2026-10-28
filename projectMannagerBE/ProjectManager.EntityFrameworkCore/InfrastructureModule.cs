using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectManager.Domain.Services;

namespace ProjectManager.EntityFrameworkCore;

public class InfrastructureModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // 1. DbContext 直接用 Autofac 注册
        builder.Register(c =>
        {
            var configuration = c.Resolve<IConfiguration>();
            var connectionString = configuration.GetConnectionString("Default")!;

            var optionsBuilder = new DbContextOptionsBuilder<ProjectManagerDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            var interceptor = c.Resolve<AuditSaveChangesInterceptor>();
            optionsBuilder.AddInterceptors(interceptor);

            return new ProjectManagerDbContext(optionsBuilder.Options);
        }).AsSelf().InstancePerLifetimeScope();

        // 2. Repository 扫描
        var assembly = typeof(InfrastructureModule).Assembly;
        builder.RegisterAssemblyTypes(assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        // 3. 拦截器
        builder.RegisterType<AuditSaveChangesInterceptor>()
            .AsSelf()
            .InstancePerLifetimeScope();
        
        //4.缓存
        builder.RegisterType<RedisCacheService>()
            .As<ICacheService>()
            .SingleInstance();
    }
}