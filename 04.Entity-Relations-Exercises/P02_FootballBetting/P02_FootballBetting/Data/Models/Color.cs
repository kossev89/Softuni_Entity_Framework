﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Color
    {
        public Color()
        {

        }

        [Key]
        public int ColorId { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
