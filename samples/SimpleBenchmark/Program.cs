using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace SimpleBenchmark;

public class Md5VsSha256
{
    private const int N = 10000;
    private readonly byte[] data;
    private readonly MD5 md5 = MD5.Create();

    private readonly SHA256 sha256 = SHA256.Create();

    public Md5VsSha256()
    {
        data = new byte[N];
        new Random(42).NextBytes(data);
    }

    [Benchmark]
    public byte[] Sha256()
    {
        return sha256.ComputeHash(data);
    }

    [Benchmark]
    public byte[] Md5()
    {
        return md5.ComputeHash(data);
    }
}

public static class Program
{
    private static void Main()
    {
       _ = BenchmarkRunner.Run<Md5VsSha256>();
    }
}