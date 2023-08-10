﻿using HomeBanking.Models;
using System;
using System.Text.Json.Serialization;

namespace HomeBanking.DTOS
{
    public class TransactionDTO
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
