// Copyright (c) Elias Frank. All rights reserved.

namespace DotNetPerformanceDelta.Application;

using System.Diagnostics;

internal static class GitShell
{
    internal static async Task<bool> IsInstalledAsync()
    {
        using var cmd = new Process();
        cmd.StartInfo.FileName = "git";
        cmd.StartInfo.RedirectStandardInput = true;
        cmd.StartInfo.RedirectStandardOutput = true;
        cmd.StartInfo.CreateNoWindow = true;
        cmd.StartInfo.UseShellExecute = false;

        cmd.Start();
        await cmd.WaitForExitAsync();

        return cmd.ExitCode == 0;
    }
}
