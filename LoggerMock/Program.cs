using LoggerMock;
using Serilog.Core;
using ILogger = Serilog.ILogger;

Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<ILogger, Logger>(LoggerRegister.CreateLogger);
        services.AddHostedService<Worker>();
    })
    .Build()
    .Run();