namespace BlockChainTest.ConsoleApp;

public class Transaction
{
    public string From { get; set; }
    public string To { get; set; }
    public decimal Amount { get; set; }
    public DateTime TimeStamp { get; set; }

    public Transaction(string from, string to, decimal amount)
    {
        From = from;
        To = to;
        Amount = amount;
        TimeStamp = DateTime.Now;
    }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => $"[{TimeStamp}] {From}\t=>\t{To}\tBTC{Amount:F}";
}