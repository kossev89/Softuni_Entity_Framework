using Invoices.Data.Models.Enums;
using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Reflection.Metadata;
using System.Globalization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType ("Invoice")]
    public class InvoiceExportDto
    {
        [XmlElement ("InvoiceNumber")]
        public int Number { get; set; }
        [XmlElement("InvoiceAmount")]
        public string Amount { get; set; }
        [XmlElement("DueDate")]
        public string DueDate { get; set; }
        [XmlElement("Currency")]
        public CurrencyType CurrencyType { get; set; }
    }
}
