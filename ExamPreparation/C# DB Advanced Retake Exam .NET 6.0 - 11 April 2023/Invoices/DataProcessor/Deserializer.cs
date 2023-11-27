namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Net;
    using System.Reflection.Metadata;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Castle.Core.Internal;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder output = new();
            XmlHelper xmlHelper = new();

            ClientImportDto[] clientsDtos = xmlHelper.Decerialize<ClientImportDto[]>(xmlString, "Clients");
            ICollection<Client> validClients = new HashSet<Client>();

            foreach (var c in clientsDtos)
            {
                if (!IsValid(c))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Address> validAddresses = new HashSet<Address>();
                foreach (var a in c.Addresses)
                {
                    if (!IsValid(a))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    Address address = new()
                    {
                        StreetName = a.StreetName,
                        StreetNumber = a.StreetNumber,
                        PostCode = a.PostCode,
                        City = a.City,
                        Country = a.Country   
                    };
                validAddresses.Add(address);
                }
                Client client = new()
                {
                    Name = c.Name,
                    NumberVat = c.NumberVat,
                    Addresses = validAddresses
                };
                validClients.Add(client);
                output.AppendLine(string.Format(SuccessfullyImportedClients, c.Name));
            }
            context.Clients.AddRange(validClients);
            context.SaveChanges();
            return output.ToString().Trim();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder output = new();
            InvoiceImportDto[] invoicesDtos = JsonConvert.DeserializeObject<InvoiceImportDto[]>(jsonString);
            ICollection<Invoice> validInvoices = new HashSet<Invoice>();

            foreach (var i in invoicesDtos)
            {
                if (!IsValid(i))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                if (i.DueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture) || i.IssueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                if (i.IssueDate>i.DueDate)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Invoice invoice = new()
                {
                    Number = i.Number,
                    IssueDate = i.IssueDate,
                    DueDate = i.DueDate,
                    Amount = i.Amount,
                    CurrencyType = (CurrencyType)i.CurrencyType,
                    ClientId = i.ClientId
                };
                validInvoices.Add(invoice);
                output.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice.Number));
            }
            context.Invoices.AddRange(validInvoices);
            context.SaveChanges();
            return output.ToString().Trim();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            StringBuilder output = new();
            ProductImportDto[] products = JsonConvert.DeserializeObject<ProductImportDto[]>(jsonString);
            ICollection<Product> validProducts = new HashSet<Product>();

            foreach (var p in products)
            {
                if (!IsValid(p))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                ICollection<Client> validClients = new HashSet<Client>();
                var allClients = context.Clients
                    .ToArray();

                foreach (var c in p.Clients)
                {
                    var client = allClients.FirstOrDefault(x => x.Id == c);
                    if (client==null)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    validClients.Add(client);
                }
                Product product = new()
                {
                    Name = p.Name,
                    Price = p.Price,
                    CategoryType = (CategoryType)p.CategoryType,
                };

                validProducts.Add(product);
                output.AppendLine(string.Format(SuccessfullyImportedProducts, product.Name, validClients.Count()));
            }
            context.Products.AddRange(validProducts);
            context.SaveChanges();
            return output.ToString().Trim();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
