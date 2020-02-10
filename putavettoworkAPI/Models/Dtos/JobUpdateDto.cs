﻿using putavettoworkAPI.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static putavettoworkAPI.Models.Job;

namespace putavettoworkAPI.Models.Dtos
{
    public class JobUpdateDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public double Location { get; set; }
        public int JobSearchId { get; set; }

        
    }
}
