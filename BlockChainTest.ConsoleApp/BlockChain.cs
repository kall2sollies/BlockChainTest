using System.Text;

namespace BlockChainTest.ConsoleApp;

public class BlockChain
{
    private readonly int _challenge;
    private readonly decimal _miningReward;

    public const string SystemUser = "SYSTEM";

    private List<Transaction> _pendingTransactions = new();

    public List<Block> Chain { get; set; }

    public BlockChain(int challenge, decimal miningReward)
    {
        _challenge = challenge;
        _miningReward = miningReward;
        var genesisBlock = new Block(
            height: 1,
            timeStamp:DateTime.Now,
            transactions: new() { new Transaction(SystemUser, SystemUser, 0m) },
            previousHash:"0");
        Chain = new List<Block> { genesisBlock };
    }

    public void AddTransaction(Transaction transaction)
    {
        var senderBalance = GetBalance(transaction.From);
        if (transaction.From != SystemUser && senderBalance < transaction.Amount)
        {
            Console.WriteLine($"Balance of sender {transaction.From} is BTC {senderBalance:F}. Transaction to {transaction.To} for BTC {transaction.Amount:F} is not valid.");
            return;
        }
        _pendingTransactions.Add(transaction);
    }

    public void MinePendingTransactions(string minerAddress)
    {
        // Transaction de récompense
        var miningRewardTransaction = new Transaction(
            from: SystemUser,
            to: minerAddress,
            amount: _miningReward);
        // On l'ajoute au bloc à miner
        _pendingTransactions.Add(miningRewardTransaction);

        // Tentative de minage
        var blockToMine = new Block(
            Chain.Last().Height + 1,
            timeStamp: DateTime.Now, 
            transactions: _pendingTransactions,
            previousHash: Chain.Last().Hash);
        blockToMine.Mine(_challenge);
        
        // Mining ok => ajouter le block à la blockChain
        Chain.Add(blockToMine);
        // Et reset les transactions en attente (nouveau bloc)
        _pendingTransactions = new();
    }

    public bool IsValid()
    {
        // map des hash => transaction
        var chainMap = Chain.ToDictionary(
            b => b.Hash,
            b => b);
        foreach (var block in Chain)
        {
            // Si le hash du bloc est falsifié, la chaine est invalide
            if (block.Hash != block.CreateHash()) return false;
            
            // Si c'est le premier vrai bloc, il est forcément valide par rapport au bloc genesis
            if (block.IsGenesis) return true;
            // Trouver le block précédent via son hash
            var previousBlock = chainMap[block.PreviousHash];
            // Si le hash du bloc précédent ne matche pas, la chaine est invalide
            if (block.PreviousHash != previousBlock.Hash) return false;
        }
        // Si on arrive jusque là, la chaine est valide
        return true;
    }

    public decimal GetBalance(string user)
    {
        var balance = 0m;
        foreach (var block in Chain)
        {
            foreach (var transaction in block.Transactions)
            {
                if (transaction.From == user) balance -= transaction.Amount;
                if (transaction.To == user) balance += transaction.Amount;
            }
        }

        return balance;
    }

    public List<string> GetUsers()
    {
        var users = new List<string>();
        foreach (var block in Chain)
        {
            foreach (var transaction in block.Transactions)
            {
                users.Add(transaction.From);
                users.Add(transaction.To);
            }
        }

        return users.Distinct().ToList();
    }

    public Dictionary<string, decimal> GetBalances()
    {
        var users = GetUsers();
        return users.ToDictionary(u => u, GetBalance);
    }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var block in Chain)
        {
            sb.Append(block);
            sb.AppendLine();
        }
        sb.AppendLine();
        var balances = GetBalances();
        sb.AppendLine("Current balances: ");
        foreach (var b in balances)
        {
            sb.AppendLine($"{b.Key}:\tBTC {b.Value:F}");    
        }
        return sb.ToString();
    }
}