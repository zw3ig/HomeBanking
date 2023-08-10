﻿using Microsoft.VisualBasic;
using System;
using System.Text.Json.Serialization;

namespace HomeBanking.DTOS
{
    public class CardDTO
    {
        public long Id { get; set; }
        public string CardHolder { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public string Number { get; set; }
        public int Cvv { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ThruDate { get; set;}
    }
}
