using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace putavettoworkAPI.Dtos
{
    public class JobSearchDto
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
    }
}
