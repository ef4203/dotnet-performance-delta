// Copyright (c) Elias Frank. All rights reserved.

namespace DotNetPerformanceDelta.Application;

using Microsoft.Extensions.Logging;

public class DeltaCommand(ILogger logger)
{
    private readonly ILogger logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public void Invoke(DeltaConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        this.logger.LogInformation("Hello, World!");
    }
}
