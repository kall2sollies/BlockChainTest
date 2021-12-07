using System.Security.Cryptography;
using System.Text;

namespace BlockChainTest.ConsoleApp;

public class Hasher : IHasher
{
    public string ComputeHash(string rawData)
    {
        using SHA256 hasher = SHA256.Create();
        var bytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return Encoding.UTF8.GetString(bytes);
    }
}