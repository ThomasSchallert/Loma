using System;
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
        public int Year { get; set; }

        public string CoverJsonPath => $"{Year}_{Location}_{Title}.json";

        public VacationCover()
        {
            Image_Path = string.Empty;
            Location = string.Empty;
            Title = string.Empty;
            Year = 0;
        }
    }
}
