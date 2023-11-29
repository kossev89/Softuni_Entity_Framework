namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.Utilities;
    using Microsoft.VisualBasic;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Xml.Linq;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {

            XmlHelper xmlHelper = new();
            string rootName = "Clients";
            var clients = context.Clients
                .Where(x => x.Invoices.Any(y => y.IssueDate > date))
                .Select(e => new
                {
                    ClientName = e.Name,
                    VatNumber = e.NumberVat,
                    InvoicesCount = e.Invoices.Count,
                    Invoices = e.Invoices
                    .Select(i => new
                    {
                        InvoiceNumber = i.Number,
                        InvoiceAmount = i.Amount,
                        DueDate = i.DueDate,
                        Currency = i.CurrencyType,
                        i.IssueDate
                    })
                     .OrderBy(id => id.IssueDate)
                     .ThenByDescending(d => d.DueDate)
                     .ToArray()
                })
                .OrderByDescending(ic => ic.InvoicesCount)
                .ThenBy(n => n.ClientName)
                .ToArray();

            HashSet<ClientExportDto> clientExportDtos = new HashSet<ClientExportDto>();

            foreach (var client in clients)
            {
                ClientExportDto clientExportDto = new()
                {
                    Name = client.ClientName,
                    NumberVat = client.VatNumber,
                    InvoicesCount = client.Invoices.Count()
                };
                foreach (var invoice in client.Invoices)
                {
                    InvoiceExportDto invoiceExportDto = new()
                    {
                        Number = invoice.InvoiceNumber,
                        Amount = invoice.InvoiceAmount.ToString("G29"),
                        DueDate =invoice.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        CurrencyType = invoice.Currency
                    };
                    if (invoiceExportDto==null)
                    {
                        continue; 
                    }
                    clientExportDto.Invoices.Add(invoiceExportDto);
                }
                clientExportDtos.Add(clientExportDto);
            }
            return xmlHelper.Serialize(clientExportDtos, rootName).ToString();
           
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var products = context.Products
                .Where(x => x.ProductsClients.Any(y => y.Client.Name.Length >= nameLength))
                .Select(e => new
                {
                    e.Name,
                    e.Price,
                    Category = e.CategoryType.ToString(),
                    Clients = e.ProductsClients
                    .Where(n => n.Client.Name.Length >= nameLength)
                    .Select(c => new
                    {
                        c.Client.Name,
                        c.Client.NumberVat
                    })
                    .OrderBy(n => n.Name)
                    .ToArray()
                })
                .OrderByDescending(c => c.Clients.Count())
                .ThenBy(n => n.Name)
                .Take(5)
                .ToArray();

            string jsonExport = JsonConvert.SerializeObject(products,Formatting.Indented);
            return jsonExport.ToString();
        }
    }
}