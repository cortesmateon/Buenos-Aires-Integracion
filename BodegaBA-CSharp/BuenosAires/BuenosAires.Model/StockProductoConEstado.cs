using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAires.Model
{
    public class StockProductoConEstado
    {
        public int idstock { get; set; }
        public int idprod { get; set; }
        public string nomprod { get; set; }
        public Nullable<int> nrofac { get; set; }
        public string estado { get; set; }
    }
}
