using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.DataProcessor.ImportDto
{
    public class InvoiceImportDto
    {
        [JsonProperty("Number")]
        [Required]
        [Range(1_000_000_000, 1_500_000_000)]
        public int Number { get; set; }
        [JsonProperty("IssueDate")]
        [Required]
        public DateTime IssueDate { get; set; }
        [JsonProperty("DueDate")]
        [Required]
        public DateTime DueDate { get; set; }
        [JsonProperty("Amount")]
        [Required]
        public decimal Amount { get; set; }
        [JsonProperty("CurrencyType")]
        [Required]
        [Range(0,2)]
        public int CurrencyType { get; set; }
        [JsonProperty("ClientId")]
        [Required]
        public int ClientId { get; set; }
    }
}
