// Copyright (c) Elias Frank. All rights reserved.

namespace DotNetPerformanceDelta.Console;

using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Logging;

internal static class Program
{
    internal static async Task<int> Main(string[] args)
    {
        return await new CommandLineBuilder(new AppRoot(CreateDefaultLogger()))
            .UseDefaults()
            .Build()
            .InvokeAsync(args);
    }

    private static ILogger CreateDefaultLogger()
    {
        using var factory = LoggerFactory.Create(
            builder =>
            {
                builder.AddSimpleConsole(
                    x =>
                    {
                        x.IncludeScopes = false;
                        x.SingleLine = true;
                    });

                builder.SetMinimumLevel(LogLevel.Information);
            });

        return factory.CreateLogger(string.Empty);
    }
}
