using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

#region telemetry

// IMPORTANT:
AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);

using var otel = Sdk.CreateTracerProviderBuilder()
    .AddSource("demo", "Azure.Cosmos.Operation")
    .AddHttpClientInstrumentation()
    .AddZipkinExporter()
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("demo"))
    .Build();

#endregion

LinqTest.TestAsync().Wait();

Console.WriteLine("OK");