﻿using AutoMapper;
using putavettoworkAPI.Dtos;
using putavettoworkAPI.Models;
using putavettoworkAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace putavettoworkAPI.Mapper
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<JobSearch, JobSearchDto>().ReverseMap();
            CreateMap<Job, JobDto>().ReverseMap();
            CreateMap<Job, JobCreateDto>().ReverseMap();
            CreateMap<Job, JobUpdateDto>().ReverseMap();
        }
    }
}
