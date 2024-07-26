﻿using Invoices.Data.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoices.Data.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public DateTime IssueDate { get; set; } // required by default --> translates to DATETIME 2

        [Required]
        public DateTime DueDate  { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public CurrencyType CurrencyType { get; set; }

        //TODO: Navigation properties
        [Required]
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }
        [Required]
        public virtual Client Client { get; set; } = null!;
    }
}
