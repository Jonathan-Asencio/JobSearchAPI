using putavettoworkAPI.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static putavettoworkAPI.Models.Job;

//Model for manually adding job postings
namespace putavettoworkAPI.Models.Dtos
{
    public class JobCreateDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public double Location { get; set; }
        
        public int JobSearchId { get; set; }

        
    }
}
