using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using MusicHub.Data.Models;
namespace MusicHub.Data.Models
{
    public class Song
    {
        public Song()
        {

        }

        [Key]
        public int Id { get; set; }
        [StringLength (20), Required]
        public string Name { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public Genre Genre { get; set; }
        [ForeignKey ("Album")]
        public int? AlbumId { get; set; }
        public Album Album { get; set; }
        [ForeignKey ("Writer"), Required]
        public int WriterId { get; set; }
        public Writer Writer { get; set; }
        [Required]
        public decimal Price { get; set; }
        public ICollection<SongPerformer> SongPerformers { get; set; }
    }
}
