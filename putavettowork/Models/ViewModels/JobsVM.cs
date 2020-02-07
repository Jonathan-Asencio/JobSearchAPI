using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace putavettowork.Models.ViewModels
{
    //Jobs View Model
    public class JobsVM
    {
        public IEnumerable<SelectListItem> JobSearchList { get; set; }
        public Jobs Jobs { get; set; }

    }
}
