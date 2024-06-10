// Copyright (c) Elias Frank. All rights reserved.

namespace DotNetPerformanceDelta.Application;

public class DotNetPerformanceDeltaConfiguration
{
    public Uri? GitUrl { get; set; }

    public string BaseBranch { get; set; } = string.Empty;

    public string NextBranch { get; set; } = string.Empty;

    public string BenchmarkPath { get; set; } = string.Empty;
}
