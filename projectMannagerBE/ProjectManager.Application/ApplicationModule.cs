using Autofac;
using FluentValidation;
using MediatR;
using ProjectManager.Application.Contracts.Services;
using ProjectManager.Application.PipelineBehaviors;
using ProjectManager.Application.Services;

namespace ProjectManager.Application;

public class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var appAssembly = typeof(ApplicationModule).Assembly;
        var contractsAssembly = typeof(IJwtTokenService).Assembly;

        // 1. IMediator
        builder.RegisterType<Mediator>()
            .As<IMediator>()
            .InstancePerLifetimeScope();

        // 2. Handler
        builder.RegisterAssemblyTypes(appAssembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>))
            .InstancePerLifetimeScope();

        // 3. Validator（两个程序集都扫，确保 LoginCommandValidator 被注册）
        builder.RegisterAssemblyTypes(appAssembly, contractsAssembly)
            .AsClosedTypesOf(typeof(IValidator<>))
            .InstancePerLifetimeScope();

        // 4. 管道行为（先注册的外层先执行）
        builder.RegisterGeneric(typeof(ExceptionBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(ValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>));

        // 5. JWT 服务
        builder.RegisterType<JwtTokenService>()
            .As<IJwtTokenService>()
            .InstancePerLifetimeScope();
    }
}