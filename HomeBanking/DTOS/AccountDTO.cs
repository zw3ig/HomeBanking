﻿using HomeBanking.Models;
using System;

namespace HomeBanking.DTOS
{
    public class AccountDTO
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }

    }
}
