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

    public async Task InvokeAsync()
    {
        this.LogConfiguration(
            this.configuration.GitUrl!,
            this.configuration.BaseBranch,
            this.configuration.NextBranch,
            this.configuration.BenchmarkPath);

        if (!await DotNetShell.IsInstalledAsync())
        {
            this.LogCommandNotFound("dotnet");
            return;
        }

        if (!await GitShell.IsInstalledAsync())
        {
            this.LogCommandNotFound("git");
            return;
        }

        var tempDir = Path.Join(Path.GetTempPath(), Guid.NewGuid().ToString());
        this.LogCreatingTempDir(tempDir);
        Directory.CreateDirectory(tempDir);

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
        this.LogDeletingTempDir(tempDir);
        Directory.Delete(tempDir, true);
    }

    [LoggerMessage(LogLevel.Debug, "GitUrl: {GitUrl}, BaseBranch: {BaseBranch}, NextBranch: {NextBranch}, BenchmarkPath: {BenchmarkPath}")]
    private partial void LogConfiguration(Uri gitUrl, string baseBranch, string nextBranch, string benchmarkPath);

    [LoggerMessage(LogLevel.Error, "The command '{command}' was not found.")]
    private partial void LogCommandNotFound(string command);

    [LoggerMessage(LogLevel.Debug, "Creating temp dir: {TempDir}")]
    private partial void LogCreatingTempDir(string tempDir);

    [LoggerMessage(LogLevel.Debug, "Deleting temp dir: {TempDir}")]
    private partial void LogDeletingTempDir(string tempDir);
}
