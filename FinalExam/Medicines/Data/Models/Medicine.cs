﻿using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Data.Models
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = null!;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public DateTime ProductionDate { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        [Required]
        [StringLength(100)]
        public string Producer { get; set; } = null!;
        [ForeignKey(nameof(PharmacyId))]
        [Required]
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public ICollection<PatientMedicine> PatientsMedicines { get; set; } = new HashSet<PatientMedicine>();

    }
}
