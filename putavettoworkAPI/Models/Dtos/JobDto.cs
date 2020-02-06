using putavettoworkAPI.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static putavettoworkAPI.Models.Jobs;

namespace putavettoworkAPI.Models.Dtos
{
    public class JobDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public double Distance { get; set; }

        public DifficultyType Difficulty { get; set; }
        public int NationalParkId { get; set; }

        public JobSearchDto NationalPark { get; set; }
    }
}
