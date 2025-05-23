// See https://aka.ms/new-console-template for more information

using System.Text;
using Autofac;
using JsonQL.Demos;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Startup.DependencyInjection;
using JsonQL.Diagnostics;
using Microsoft.Extensions.Configuration;
using OROptimizer.Diagnostics.Log;
using OROptimizer.Log4Net;

RegisterLogger();

LogHelper.Context.Log.Info("---BETTER LOGS CAN BE FOUND IN [c:/LogFiles/JsonQL.Demos.log]. SOME UNICODE CHARACTERS DO NOT SHOW IN CONSOLE LOGS!---");

var configuration = LoadConfiguration();

var container = RegisterServices(configuration);

CompilationResultSerializerAmbientContext.Context = container.Resolve<ICompilationResultSerializer>();

var exampleManagers = container.Resolve<IEnumerable<IExampleManager>>().ToList();

while (true)
{
    var exampleSelectionPrompt = new StringBuilder();

    exampleSelectionPrompt.AppendLine("Enter a number to select an example to execute, or to execute all examples:");

    exampleSelectionPrompt.AppendLine("\t0 To run all examples");
    for (var i = 0; i < exampleManagers.Count; ++i)
    {
        exampleSelectionPrompt.AppendLine($"\t{i + 1} {exampleManagers[i].GetType()}");
    }

    Console.Write(exampleSelectionPrompt);

    var userEntry = Console.ReadLine();

    if (!int.TryParse(userEntry, out var selectedNumber) || selectedNumber < 0 || selectedNumber > exampleManagers.Count)
    {
        Console.WriteLine("Invalid entry. Try again.");
        continue;
    }

    if (selectedNumber == 0)
    {
        foreach (var exampleManager in exampleManagers)
        {
            await ExecuteExample(exampleManager);
        }
           
    }
    else
    {
        await ExecuteExample(exampleManagers[selectedNumber - 1]);
    }
}

async Task ExecuteExample(IExampleManager exampleManager)
{
    try
    {
        await exampleManager.ExecuteAsync();
    }
    catch (Exception e)
    {
        LogHelper.Context.Log.Error($"Example [{exampleManager.GetType()}] failed", e);
    }
}



static IContainer RegisterServices(IConfigurationRoot configurationRoot)
{
    ContainerBuilder containerBuilder = new ContainerBuilder();
    RegisterModules(containerBuilder, configurationRoot);

    return containerBuilder.Build();
}

static void RegisterModules(ContainerBuilder containerBuilder, IConfigurationRoot configurationRoot)
{
    containerBuilder.RegisterModule(new ConfigurationModule(configurationRoot));
    containerBuilder.RegisterModule(new JsonQLExtensionsClassesRegistrationsModule());
    containerBuilder.RegisterModule(new JsonQLClassRegistrationsModule(LogHelper.Context.Log));
    
    containerBuilder.RegisterModule(new ExampleManagersModule());
}

static IConfigurationRoot LoadConfiguration()
{
    return new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
        .Build();
}

static void RegisterLogger()
{
    var loggerFileName = "JosnQL.log4net.config";

    try
    {
        LogHelper.RegisterContext(new Log4NetHelperContext(loggerFileName));        
    }
    catch (Exception e)
    {
        Console.Out.WriteLine($"Failed to initialize the logger: Error: {e.Message}.{Environment.NewLine}{e.StackTrace}.");
        throw new ApplicationException($"Failed to initialize the logger from [{loggerFileName}] configuration.");
    }
}