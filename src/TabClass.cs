using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LomaPro
{
    internal class TabClass
    {
        public int PeopleCount { get; set; }
        public double Balance { get; set; }
        public string Name { get; set; }

        public TabClass()
        {
            PeopleCount = 1;
            Balance = 0;
            Name = string.Empty;
        }
    }
}
