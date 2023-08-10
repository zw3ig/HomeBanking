using HomeBanking.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HomeBanking.DTOS
{
    public class AccountDTO
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }
        public ICollection<TransactionDTO> Transactions { get; set; }
    }
}
