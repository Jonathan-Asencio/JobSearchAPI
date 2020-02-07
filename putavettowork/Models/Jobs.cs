using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace putavettowork.Models
{
    public class Jobs
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public double Salary { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime Created { get; set; }
        public byte[] Picture { get; set; }
        public DateTime Established { get; set; }

        public int JobSearchkId { get; set; }

        public JobSearch JobSearch { get; set; }
    }
}
 //public enum DifficultyType {Easy, Moderate, Difficult, Expert }
        //public DifficultyType Difficulty { get; set; }