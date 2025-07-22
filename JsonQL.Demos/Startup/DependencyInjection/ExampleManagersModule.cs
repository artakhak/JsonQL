using System.Reflection;
using Autofac;
using JsonQL.Demos.Examples;
using JsonQL.Diagnostics;
using JsonQL.Utilities;
using Module = Autofac.Module;

namespace JsonQL.Demos.Startup.DependencyInjection;

public class ExampleManagersModule: Module
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
