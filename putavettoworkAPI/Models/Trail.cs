using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace putavettoworkAPI.Models
{
    public class Trail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public double Distance { get; set; }
        public enum DifficultyType { Easy, Medium, Difficult, Expert }

        public DifficultyType Difficulty { get; set; }
        public int NationalParkId { get; set; }

        [ForeignKey("NationalParkId")]
        public JobSearch NationalPark { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
