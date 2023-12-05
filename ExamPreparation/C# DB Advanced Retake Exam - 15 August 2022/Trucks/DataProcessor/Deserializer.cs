namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Castle.Core.Internal;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;
    using Trucks.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder result = new();
            string rootName = "Despatchers";
            DespatcherImportDto[] despatcherDto = XmlSerializationHelper.Deserialize<DespatcherImportDto[]>(xmlString, rootName);
            List<Despatcher> despatchers = new();

            foreach (var dto in despatcherDto)
            {
                if (!IsValid(dto)
                    || dto.Position.IsNullOrEmpty()
                    )
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }
                Despatcher despatcher = new()
                {
                    Name = dto.Name,
                    Position = dto.Position
                };

                foreach (var t in dto.Trucks)
                {
                    if (!IsValid(t))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck truck = new()
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        VinNumber = t.VinNumber,
                        TankCapacity = t.TankCapacity,
                        CargoCapacity = t.CargoCapacity,
                        CategoryType = (CategoryType)t.CategoryType,
                        MakeType = (MakeType)t.MakeType
                    };
                    despatcher.Trucks.Add(truck);
                }

                despatchers.Add(despatcher);
                result.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count()));
            }

            context.Despatchers.AddRange(despatchers);
            context.SaveChanges();
            return result.ToString().Trim();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder result = new();
            List<Client> clients = new();
            ClientImportDto[] clientDtos = JsonConvert.DeserializeObject<ClientImportDto[]>(jsonString);

            foreach (var c in clientDtos)
            {
                if (!IsValid(c)
                    || c.Type == "usual"
                    )
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }
                Client client = new()
                {
                    Name = c.Name,
                    Nationality = c.Nationality,
                    Type = c.Type
                };

                foreach (var t in c.Trucks)
                {
                    if (!context.Trucks.Any(x => x.Id == t))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }
                    ClientTruck clientTruck = new()
                    {
                        ClientId = client.Id,
                        TruckId = t
                    };
                    client.ClientsTrucks.Add(clientTruck);
                }

                clients.Add(client);
                result.AppendLine(string.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count()));
            }
            context.Clients.AddRange(clients);
            context.SaveChanges();
            return result.ToString().Trim();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}