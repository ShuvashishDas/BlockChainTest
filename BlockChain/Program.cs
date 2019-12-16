using System;
using Newtonsoft.Json;

namespace SD.BlockChainTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("What's your name: ");
            var host = Console.ReadLine();
            Console.WriteLine($"Current host is: {host}");
            
            var myChain = InitiateChain();
            var response = 0;
            do
            {
                ShowInstructions();
                if (!int.TryParse(Console.ReadLine(), out response)) response = 0;
                switch (response)
                {
                    case 1: ConnectToServer(); break;
                    case 2: myChain = AddTransaction(myChain); break;
                    case 3: myChain = ProcessPendingTransacions(myChain, host); break;
                    case 4: PrintChain(myChain); break;
                    case 5: Console.Clear(); break;
                    default: break;
                }
            } while (response != 6);
        }

        private static BlockChain InitiateChain()
        {
            var startTime = DateTime.Now;
            var myChain = new BlockChain();
            var endTime = DateTime.Now;
            Console.WriteLine("=====================================================");
            Console.WriteLine($"Genesis Block generation started at: {startTime}");
            Console.WriteLine($"Genesis Block generation completed at: {endTime}");
            Console.WriteLine($"Total time taken: {(endTime - startTime).TotalSeconds}");
            Console.WriteLine("=====================================================");
            return myChain;
        }

        private static void ShowInstructions()
        {
            Console.WriteLine("=====================================================");
            Console.WriteLine("1. Connect to a server.");
            Console.WriteLine("2. Add transactions.");
            Console.WriteLine("3. Process pending transactions.");
            Console.WriteLine("4. Print chain.");
            Console.WriteLine("5. Clear screen.");
            Console.WriteLine("6. Exit.");
            Console.WriteLine("=====================================================");
        }

        private static BlockChain ProcessPendingTransacions(BlockChain chain, string host)
        {
            var startTime = DateTime.Now;
            chain.ProcessPendingTransactions(host);
            var endTime = DateTime.Now;
            Console.WriteLine("=====================================================");            
            Console.WriteLine($"Chain processing started at: {startTime}");
            Console.WriteLine($"Chain processing completed at: {endTime}");
            Console.WriteLine($"Total time taken: {(endTime-startTime).TotalSeconds}");
            Console.WriteLine("=====================================================");
            return chain;
        }

        private static void PrintChain(BlockChain chain)
        {
            Console.WriteLine($"Is chain valid: {chain.IsValid}");
            Console.WriteLine(JsonConvert.SerializeObject(chain, Formatting.Indented));
        }

        private static BlockChain AddTransaction(BlockChain chain)
        {
            Console.WriteLine("Enter data in the following format [{sender 1}, {receiver 1}, {amount 1}][{sender 2}, {receiver 2}, {amount 2}].....[{sender n}, {receiver n}, {amount n}]. Press ENTER to finish.: ");
            var transactions = Console.ReadLine().Split(']');
            foreach(var transaction in transactions)
            {
                var data = transaction.Replace('[', ' ').Replace(']', ' ').Trim().Split(',');
                if (data.Length < 3) continue;

                var amount = (decimal) 0;
                if (!decimal.TryParse(data[2].Trim(), out amount)) amount=0;
                chain.CreatePendingTransaction(new Transaction(data[0].Trim(), data[1].Trim(), amount));
            }
            return chain;
        }

        private static void ConnectToServer()
        {
            throw new NotImplementedException();
        }
    }
}