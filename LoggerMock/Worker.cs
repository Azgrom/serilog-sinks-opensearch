using ILogger = Serilog.ILogger;

namespace LoggerMock;

public class Worker : BackgroundService
{
    private const int TwoSecondsPeriod = 2;
    private readonly ILogger _logger;
    private readonly PeriodicTimer _periodicTimer = new(TimeSpan.FromSeconds(TwoSecondsPeriod));

    public Worker(ILogger logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            var exception = new Exception("Whatever");
            _logger.Information("Worker running at: {time}", DateTimeOffset.Now);
            _logger.Error("Somente uma String");
            _logger.Warning("Formato Qualquer {@Unknown}", new Test());
            _logger.Information($"Formato do bonde {Random.Shared.Next()}", DateTime.Now);
            _logger.Error(exception, "ExceptionLogging");
        }
    }

    private struct Test
    {
        public Test()
        {
        }

        public int Integer { get; set; } = Random.Shared.Next();
        public DateTime DateTime { get; set; } = DateTime.Now;
    }

}
