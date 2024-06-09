// Copyright (c) Elias Frank. All rights reserved.

namespace DotNetPerformanceDelta.Console;

using System.CommandLine;
using DotNetPerformanceDelta.Application;
using Microsoft.Extensions.Logging;

internal sealed class AppRoot : RootCommand
{
    internal AppRoot(ILogger logger)
        : base(".NET Performance Delta CLI Tool")
    {
        var uriOption = new Option<Uri>(
            "--git-url",
            "The URL of the git repository to analyze")
        {
            IsRequired = true,
        };

        this.AddOption(uriOption);

        var baseBranchOption = new Option<string>(
            "--base-branch",
            "The name of the branch to use as the base for comparison")
        {
            IsRequired = true,
        };

        this.AddOption(baseBranchOption);

        var nextBranchOption = new Option<string>(
            "--next-branch",
            "The name of the branch to use as the next value for comparison")
        {
            IsRequired = true,
        };

        this.AddOption(nextBranchOption);

        var benchmarkPathOption = new Option<string>(
            "--benchmark-path",
            "The folder path to the benchmark project")
        {
            IsRequired = true,
        };

        this.AddOption(benchmarkPathOption);

        this.SetHandler(
            (uri, baseBranch, nextBranch, benchmarkPath) =>
            {
                new DeltaCommand(logger)
                    .Invoke(
                        new DeltaConfiguration
                        {
                            GitUrl = uri,
                            BaseBranch = baseBranch,
                            NextBranch = nextBranch,
                            BenchmarkPath = benchmarkPath,
                        });
            },
            uriOption,
            baseBranchOption,
            nextBranchOption,
            benchmarkPathOption);
    }
}
