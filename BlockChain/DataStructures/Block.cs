using System;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SD.BlockChainTest
{
    public class Block
    {
        private int _nonce;

        public DateTime TimeStamp { get; private set; }
        public List<Transaction> Data { get; private set; }
        public string CurrentHash { get; private set; }
        public string PreviousHash { get; private set; }

        public Block(List<Transaction> data, string previousHash, int difficulty)
        {
            _nonce=0;
            Data=data;
            TimeStamp=DateTime.Now;
            PreviousHash=previousHash;
            MineBlock(difficulty);
        }

        private void MineBlock(int difficulty)
        {
            var prefix = new string('0', difficulty);
            CurrentHash=CalculateHash();
            while(!CurrentHash.StartsWith(prefix))
            {
                _nonce++;
                CurrentHash=CalculateHash();
            }
        }

        public string CalculateHash()
        {
            var hashData = string.Empty;
            using(var sha256Hash = SHA256.Create())
            {
                var rawData = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes($"{TimeStamp}-{PreviousHash}-{JsonConvert.SerializeObject(Data)}-{_nonce}"));
                hashData = Convert.ToBase64String(rawData);
                sha256Hash.Dispose();
            }
            return hashData;
        }
    }
}