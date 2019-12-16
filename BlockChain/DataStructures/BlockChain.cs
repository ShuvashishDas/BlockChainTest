using System.Collections.Generic;

namespace SD.BlockChainTest
{
    public class BlockChain
    {
        public decimal Reward { get; private set; } = 1;
        public int Difficulty { get; private set; } = 2;
        public List<Block> Chain { get; private set; } = new List<Block>();
        public List<Transaction> PendingTransactions { get; private set; } = new List<Transaction>();
        public Dictionary<string, decimal> Balance { get; private set; } = new Dictionary<string, decimal>();
        public bool IsValid
        {
            get
            {
                for (var index = 1; index < Chain.Count; index++)
                {
                    if (Chain[index].CurrentHash != Chain[index].CalculateHash()) return false;
                    if (Chain[index].PreviousHash != Chain[index - 1].CurrentHash) return false;
                }
                return true;
            }
        }

        public BlockChain() => AddGenesisBlock();

        private void AddGenesisBlock() => Chain.Add(new Block(new List<Transaction> { new Transaction(null, null, 0) }, null, Difficulty));

        private Block GetLastBlock() => Chain[Chain.Count - 1];

        private void AddBlock(List<Transaction> data) => Chain.Add(new Block(data, GetLastBlock().CurrentHash, Difficulty));

        public void CreatePendingTransaction(Transaction transaction) => PendingTransactions.Add(transaction);

        public void ProcessPendingTransactions(string sender)
        {
            AddBlock(PendingTransactions);
            CalculateBalances();
            PendingTransactions = new List<Transaction>();
            CreatePendingTransaction(new Transaction(null, sender, Reward));
        }

        private void CalculateBalances()
        {
            Balance = new Dictionary<string, decimal>();
            foreach(var block in Chain)
            {
                if (string.IsNullOrEmpty(block.PreviousHash)) continue;
                if (block.Data==null) continue;

                foreach (var transaction in block.Data)
                {
                    if(!string.IsNullOrEmpty(transaction.Sender))
                    {
                        if (!Balance.ContainsKey(transaction.Sender)) Balance.Add(transaction.Sender, 0);
                        Balance[transaction.Sender] -= transaction.Amount;
                    }

                    if(!string.IsNullOrEmpty(transaction.Receiver))
                    {
                        if (!Balance.ContainsKey(transaction.Receiver)) Balance.Add(transaction.Receiver, 0);
                        Balance[transaction.Receiver] += transaction.Amount;
                    }
                }
            }
        }
    }
}