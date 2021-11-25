﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CardService.Domain
{
    /// <summary>
    /// Domain card Model with full options
    /// </summary>
    public class Card
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CVC { get; set; }
        public string Pan { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public bool IsDefault { get; set; }
        public string CardName { get; set; }
    }
}
