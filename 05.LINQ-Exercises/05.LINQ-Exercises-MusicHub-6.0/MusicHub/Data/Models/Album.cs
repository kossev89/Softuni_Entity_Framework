﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public Album()
        {
      
        }
        [Key]
        public int Id { get; set; }
        [Required, StringLength(40)]
        public string Name { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        public decimal Price => this.Songs.Sum(x => x.Price);
        [ForeignKey ("Producer"), Required]
        public int? ProducerId { get; set; }
        public Producer Producer { get; set; }
        public ICollection<Song> Songs { get; set; }

    }
}