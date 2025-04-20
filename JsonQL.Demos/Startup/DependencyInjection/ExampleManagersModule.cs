using Autofac;
using System.Reflection;
using JsonQL.Demos.Examples;
using JsonQL.Diagnostics;
using JsonQL.Utilities;

namespace JsonQL.Demos.Startup.DependencyInjection;

public class ExampleManagersModule: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(t => typeof(IExampleManager).IsAssignableFrom(t) && t is {IsAbstract: false, IsInterface: false})
            .As<IExampleManager>();

        builder.RegisterType<ClassSerializer>().As<IClassSerializer>().SingleInstance();
        builder.RegisterType<JsonSerializer>().As<IJsonSerializer>().SingleInstance();
        builder.RegisterType<CompilationResultSerializer>().As<ICompilationResultSerializer>().SingleInstance();
    }
}