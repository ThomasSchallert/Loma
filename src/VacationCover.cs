﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LomaPro
{
    public class VacationCover
    {
        public string Image_Path { get; set; }
        public string Location { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public VacationCover()
        {
            Image_Path = string.Empty;
            Location = string.Empty;
            Title = string.Empty;
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
        }
    }
}