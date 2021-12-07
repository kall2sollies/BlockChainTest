using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace BlockChainTest.ConsoleApp;

public class Block
{
    public List<Transaction> Transactions { get; set; }
    public string Hash { get; set; }
    public string PreviousHash { get; set; }
    
    private readonly DateTime _timeStamp;
    private long _nonce;
    
    private readonly IHasher _hasher;

    public bool IsGenesis => PreviousHash == "0";

    public Block(DateTime timeStamp, List<Transaction> transactions, string previousHash = "")
    {
        _timeStamp = timeStamp;
        _nonce = 0;
        
        _hasher = new Hasher();

        Transactions = transactions;
        PreviousHash = previousHash;

        Hash = CreateHash();
    }

    public void Mine(int challenge)
    {
        var hashPrefixChallenge = string.Empty.PadLeft(challenge, '0');
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        while (true)
        {
            if (Hash.Substring(0, challenge) == hashPrefixChallenge)
            {
                stopWatch.Stop();
                Console.WriteLine($"Block with Hash={Hash} succesfully mined after {_nonce} iterations in {stopWatch.Elapsed.TotalMilliseconds} ms.");
                return;
            }
            _nonce++;
            Hash = CreateHash();
        }
    }

    public string CreateHash()
    {
        var transactionsString = "";
        Transactions.ForEach(t => transactionsString += t.ToString());
        return _hasher.ComputeHash(
            $"{PreviousHash}{_timeStamp.Ticks.ToString()}{transactionsString}{_nonce}");
    }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"---------------- BLOCK ----------------");
        sb.AppendLine($"Timestamp:      {_timeStamp}");
        sb.AppendLine($"Hash:           {Hash}");
        sb.AppendLine($"Previous hash:  {PreviousHash}");
        sb.AppendLine($"Nonce:          {_nonce}");
        sb.AppendLine($"Transactions:   {Transactions.Count}");
        foreach (var transaction in Transactions) sb.AppendLine($"                =>  {transaction}");
        sb.AppendLine($"---------------------------------------");
        return sb.ToString();
    }
}