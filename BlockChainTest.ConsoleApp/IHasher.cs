namespace BlockChainTest.ConsoleApp;

public interface IHasher
{
    string ComputeHash(string rawData);
}