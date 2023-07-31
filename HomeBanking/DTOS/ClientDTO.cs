﻿using HomeBanking.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HomeBanking.DTOS
{
    public class ClientDTO
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<AccountDTO> Accounts { get; set; }
        public ICollection<ClientLoanDTO> Loans { get; set; }
    }
}
