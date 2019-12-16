namespace SD.BlockChainTest
{
    public class Transaction
    {
        public string Sender { get; private set; }
        public string Receiver { get; private set; }
        public decimal Amount { get; private set; }

        public Transaction(string sender, string receiver, decimal amount)
        {
            this.Sender = sender;
            this.Receiver = receiver;
            this.Amount = amount;
        }
    }
}