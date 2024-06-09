using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LomaPro
{
    public class BillGroup
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public Picker picker { get; set; }
        public Picker PayerPicker { get; set; }
        public double HasToPay { get; set; }
        public double PaidAmount { get; set; }  // Neuer Eigenschaft
        public int SelectedIndex { get; set; }
    }

}
