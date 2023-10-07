using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenSearch.Client;
using OpenSearch.Net;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Formatting.OpenSearch;
using Serilog.Sinks.File;
using Serilog.Sinks.OpenSearch;
using EmitEventFailureHandling = Serilog.Sinks.OpenSearch.EmitEventFailureHandling;

namespace LoggerMock;

public static class LoggerRegister
{
    private static readonly IConfigurationRoot Conf = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    public static Logger CreateLogger(IServiceProvider serviceProvider)
    {
        var staticConnectionPool = new StaticConnectionPool(new[] { new Uri("http://elastic:changeme@localhost:9200") });

        return new LoggerConfiguration()
            .WriteTo.Console()
            // .WriteTo.OpenSearch(new OpenSearchSinkOptions(staticConnectionPool)
            // {
            //     AutoRegisterTemplate = false,
            //     OverwriteTemplate = true,
            //     DetectOpenSearchVersion = true,
            //     IndexFormat = "LoggerMock",
            //     NumberOfReplicas = 1,
            //     NumberOfShards = 2,
            //     FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
            //     EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
            //                        EmitEventFailureHandling.WriteToFailureSink |
            //                        EmitEventFailureHandling.RaiseCallback,
            //     FailureSink = new FileSink("./fail-{Date}.txt", new JsonFormatter(), null, null),
            //     BufferCleanPayload = (failingEvent, statuscode, exception) =>
            //     {
            //         dynamic e = JObject.Parse(failingEvent);
            //         return JsonConvert.SerializeObject(new Dictionary<string, object>()
            //         {
            //             { "@timestamp", e["@timestamp"] },
            //             { "level", "Error" },
            //             { "message", "Error: " + e.message },
            //             { "messageTemplate", e.messageTemplate },
            //             { "failingStatusCode", statuscode },
            //             { "failingException", exception }
            //         });
            //     },
            // })
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithProperty("ApplicationName", "LoggerMock")
            .CreateLogger();
    }

    private class MordeKaiser : ILogEventSink
    {
        private static readonly Uri[] Uris = { new("http://localhost:9200") };
        private static readonly StaticConnectionPool StaticConnectionPool = new(Uris);
        private readonly IFormatProvider _formatProvider;
        private readonly OpenSearchClient _openSearchClient;

        public MordeKaiser(IFormatProvider formatProvider)
        {
            ConnectionSettings connectionSettings = new(StaticConnectionPool);
            connectionSettings.BasicAuthentication("admin", "admin");
            _openSearchClient = new OpenSearchClient(connectionSettings);
            _formatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            var renderMessage = logEvent.RenderMessage(_formatProvider);
            var log = $"{DateTimeOffset.Now} - {renderMessage}";
            _openSearchClient.Index(log, idx => idx.Index("TestIndex"));
            Console.WriteLine(log);
        }
    }
}
