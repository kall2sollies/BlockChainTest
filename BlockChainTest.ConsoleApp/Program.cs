namespace BlockChainTest.ConsoleApp;

public static class Program
{
    public static void Main(string[] args)
    {
        // test encryption / authentication with https://stackoverflow.com/questions/18485715/how-to-use-public-and-private-key-encryption-technique-in-c-sharp
        // users de la blockchain
        var miner = "miner01@example.com";
        var user1 = "alice@example.com";
        var user2 = "bob@example.com";
        var user3 = "coco@example.com";
        var users = new List<string> { miner, user1, user2, user3 };
        
        // Initiliser une blockchain
        var blockChain = new BlockChain(
            challenge: 5,
            miningReward: 20);
        
        // Valide d'emblée ?
        Console.WriteLine($"Blockchain initialisée. Blocks: {blockChain.Chain.Count}. Valide: {blockChain.IsValid()}");

        // Mettre de l'argent en jeu
        blockChain.AddTransaction(new Transaction(BlockChain.SystemUser, user1, 100m));
        blockChain.AddTransaction(new Transaction(BlockChain.SystemUser, user2, 100m));
        blockChain.AddTransaction(new Transaction(BlockChain.SystemUser, user3, 100m));
        Console.WriteLine($"Argent initial.");
        blockChain.MinePendingTransactions(miner);
        Console.WriteLine(blockChain.Chain.Last());
        
        // Ajouter des transactions
        blockChain.AddTransaction(new Transaction(user1, user2, 200m));
        blockChain.AddTransaction(new Transaction(user2, user1, 10m));
        Console.WriteLine($"Ajout de 2 transactions.");
        
        // Miner le bloc des transactions en attente
        blockChain.MinePendingTransactions(miner);
        Console.WriteLine($"Blocks: {blockChain.Chain.Count}. Valide: {blockChain.IsValid()}");
        Console.WriteLine(blockChain.Chain.Last());
        
        // Ajouter des transactions
        blockChain.AddTransaction(new Transaction(user1, user3, 92m));
        blockChain.AddTransaction(new Transaction(user3, user2, 15.45m));
        blockChain.AddTransaction(new Transaction(user2, user1, 1.98m));
        Console.WriteLine($"Ajout de 3 transactions.");
        
        // Miner le bloc des transactions en attente
        blockChain.MinePendingTransactions(miner);
        Console.WriteLine($"Blocks: {blockChain.Chain.Count}. Valide: {blockChain.IsValid()}");
        Console.WriteLine(blockChain.Chain.Last());
        Console.WriteLine(blockChain);
        Console.Write("\r\n\r\n");
    }
}
