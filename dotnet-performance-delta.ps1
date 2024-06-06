# Copyright (c) Elias Frank. All rights reserved.
param (
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$gitRepoUri,

    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$previousCommitId,

    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$currentCommitId,

    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$benchmarkProjectPath
)

function Test-CommandExists($command)
{
    $cmd = Get-Command $command -ErrorAction SilentlyContinue
    if ($cmd -eq $null)
    {
        Write-Host "Error: Required command '$command' not found."
        Exit
    }
}

# Validate inputs
if ([string]::IsNullOrEmpty($previousCommitId) -or
    [string]::IsNullOrEmpty($previousCommitId) -or
    [string]::IsNullOrEmpty($currentCommitId) -or
    [string]::IsNullOrEmpty($benchmarkProjectPath))
{
    Write-Host "Error: All inputs must have a value."
    return
}

# Validate commands
Test-CommandExists "git"
Test-CommandExists "dotnet"

New-Item -ItemType Directory -Path "a"
New-Item -ItemType Directory -Path "b"
git clone $gitRepoUri a
Set-Location a
git checkout $previousCommitId
Set-Location ..
git clone $gitRepoUri b
Set-Location b
git checkout $currentCommitId
Set-Location ..
$currDir = Get-Location
Set-Location "./a/$benchmarkProjectPath"
dotnet run --configuration release
Set-Location $currDir
Set-Location "./b/$benchmarkProjectPath"
dotnet run --configuration release
Set-Location $currDir

Write-Host "--------------------"
$allResultsFilesA = Get-ChildItem -Path "./a/$benchmarkProjectPath/BenchmarkDotNet.Artifacts/results" -Filter "*.csv" -File
$allResultsA = @()
foreach ($file in $allResultsFilesA)
{
    $allResultsA += Import-Csv $file.FullName
}

$allResultsFilesB = Get-ChildItem -Path "./b/$benchmarkProjectPath/BenchmarkDotNet.Artifacts/results" -Filter "*.csv" -File
$allResultsB = @()
foreach ($file in $allResultsFilesB)
{
    $allResultsB += Import-Csv $file.FullName
}

Out-File -Append -FilePath "./delta.csv" -InputObject "Method,PreviousMean,CurrentMean,Delta"
$deltaResult = @()
foreach($result in $allResultsA)
{
    $resultB = $allResultsB | Where-Object { $_.Method -eq $result.Method }
    if ($resultB -eq $null)
    {
        continue
    }

    # There is a bug here if the results are not the same time unit
    $delta = [double]($resultB.Mean -split ' ')[0] / [double]($result.Mean -split ' ')[0]
    Write-Host "Method:" $result.Method "Prev:" $result.Mean "Curr:" $resultB.Mean "Delta:" $delta
    Out-file -Append -FilePath "./delta.csv" -InputObject "$($result.Method),$($result.Mean),$($resultB.Mean),$delta"
}

Remove-Item -Path "a" -Recurse -Force
Remove-Item -Path "b" -Recurse -Force
