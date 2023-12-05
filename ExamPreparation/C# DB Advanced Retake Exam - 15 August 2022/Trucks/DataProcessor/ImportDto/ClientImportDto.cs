﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ImportDto
{
    public class ClientImportDto
    {
        [JsonProperty("Name")]
        [JsonRequired]
        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;
        [JsonProperty("Nationality")]
        [JsonRequired]
        [MinLength(2)]
        [MaxLength(40)]
        public string Nationality { get; set; } = null!;
        [JsonProperty("Type")]
        [JsonRequired]
        public string Type { get; set; } = null!;
        [JsonProperty("Trucks")]
        public HashSet<int> Trucks { get; set; } = new HashSet<int>();
    }
}
