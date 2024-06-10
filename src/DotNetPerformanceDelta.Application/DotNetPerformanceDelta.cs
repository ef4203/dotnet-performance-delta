// Copyright (c) Elias Frank. All rights reserved.

namespace DotNetPerformanceDelta.Application;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

public partial class DotNetPerformanceDelta
{
    private readonly DotNetPerformanceDeltaConfiguration configuration = new();

    private ILogger? logger = NullLogger.Instance;

    public DotNetPerformanceDelta(Action<DotNetPerformanceDeltaConfiguration> configureAction)
    {
        ArgumentNullException.ThrowIfNull(configureAction);

        configureAction(this.configuration);
    }

    public DotNetPerformanceDelta AddLogger(ILogger loggerInstance)
    {
        this.logger = loggerInstance ?? throw new ArgumentNullException(nameof(loggerInstance));
        return this;
    }

    public void Invoke()
    {
        this.LogConfiguration(
            this.configuration.GitUrl!,
            this.configuration.BaseBranch,
            this.configuration.NextBranch,
            this.configuration.BenchmarkPath);

        // 1. Check if git is installed
        // 2. Check if dotnet is installed
        // 3. Create a temporary directory
        // 4. Clone the repository to the temporary directory/a
        // 5. Clone the repository to the temporary directory/b
        // 6. Check if the benchmark path exists for a
        // 7. Check if the benchmark path exists for b
        // 8. Run the benchmark for a
        // 9. Run the benchmark for b
        // 10. Check if the result file exists for a
        // 11. Check if the result file exists for b
        // 12. Compare the result files
        // 13. Output the delta
        // 14. Cleanup the temporary directory
    }

    [LoggerMessage(LogLevel.Debug, "GitUrl: {GitUrl}, BaseBranch: {BaseBranch}, NextBranch: {NextBranch}, BenchmarkPath: {BenchmarkPath}")]
    private partial void LogConfiguration(Uri gitUrl, string baseBranch, string nextBranch, string benchmarkPath);
}
