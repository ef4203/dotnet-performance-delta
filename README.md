# dotnet-performance-delta

dotnet-performance-delta is a simple CLI tool that compares performance
benchmarks from two individual commits and computes a delta between these
two commits.


## Example
```ps1
./dotnet-performance-delta.ps1 -gitRepoUri "https://github.com/ef4203/dotnet-performance-delta" -previousCommitId 364575a312757e0d598b9aea49143552f3908def -currentCommitId d5e1f9759afcaa7cdd82be2ecab06ab14855ac12 -benchmarkProjectPath "samples/SimpleBenchmark/"
```
