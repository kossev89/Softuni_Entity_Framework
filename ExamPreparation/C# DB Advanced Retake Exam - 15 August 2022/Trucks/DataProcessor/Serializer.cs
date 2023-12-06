namespace Trucks.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.Xml.Linq;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ExportDto;
    using Trucks.Utilities;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            var despatchers = context.Despatchers
                .Where(x => x.Trucks.Any())
                .Select(e => new
                {
                    e.Name,
                    Trucks = e.Trucks
                    .Select(t => new
                    {
                        t.RegistrationNumber,
                        t.MakeType
                    })
                    .OrderBy(x => x.RegistrationNumber)
                    .ToArray()
                })
                .OrderByDescending(c => c.Trucks.Count())
                .ThenBy(n => n.Name)
                .ToArray();

            List<DespatcherExportXmlDto> despetchersToExport = new();

            foreach (var d in despatchers)
            {
                DespatcherExportXmlDto despatcher = new()
                {
                    DespatcherName = d.Name,
                    TrucksCount = d.Trucks.Count()
                };

                foreach (var t in d.Trucks)
                {
                    TruckExportXmlDto truck = new()
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        Make = t.MakeType.ToString(),
                    };
                    despatcher.Trucks.Add(truck);
                }

                despetchersToExport.Add(despatcher);
            }

            string rootName = "Despatchers";
            return XmlSerializationHelper.Serialize(despetchersToExport, rootName);
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients
                .Where(x => x.ClientsTrucks.Any(y => y.Truck.TankCapacity >= capacity))
                .Select(e => new
                {
                    Name = e.Name,
                    Trucks = e.ClientsTrucks
                    .Where(x => x.Truck.TankCapacity >= capacity)
                    .Select(t => new
                    {
                        TruckRegistrationNumber = t.Truck.RegistrationNumber,
                        t.Truck.VinNumber,
                        t.Truck.TankCapacity,
                        t.Truck.CargoCapacity,
                        t.Truck.CategoryType,
                        t.Truck.MakeType
                    })
                    .OrderBy(x => x.MakeType)
                    .ThenByDescending(x => x.CargoCapacity)
                    .ToArray()
                })
                .OrderByDescending(x => x.Trucks.Count())
                .ThenBy(x => x.Name)
                .Take(10)
                .ToArray();

            List<ClientExportJsonDto> clientsToExport = new();

            foreach (var item in clients)
            {
                ClientExportJsonDto client = new()
                {
                    Name = item.Name
                };

                foreach (var t in item.Trucks)
                {
                    TruckExportJsonDto truck = new()
                    {
                        RegistrationNumber = t.TruckRegistrationNumber,
                        VinNumber = t.VinNumber,
                        TankCapacity = t.TankCapacity,
                        CargoCapacity = t.CargoCapacity,
                        CategoryType = t.CategoryType.ToString(),
                        MakeType = t.MakeType.ToString()
                    };
                    client.Trucks.Add(truck);
                }

                clientsToExport.Add(client);
            }


            return JsonConvert.SerializeObject(clientsToExport, Formatting.Indented).ToString();
        }
    }
}
