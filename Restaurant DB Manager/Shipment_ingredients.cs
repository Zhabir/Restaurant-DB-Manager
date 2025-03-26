using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_DB_Manager
{
    class Shipment_ingredients
    {
        public int shipment_code {  get; set; }
        public int ingredients_code { get; set; }
        public int? quantity { get; set; }
    }
}
