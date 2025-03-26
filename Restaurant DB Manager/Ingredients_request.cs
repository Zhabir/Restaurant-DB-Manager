using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_DB_Manager
{
    class Ingredients_request
    {
        public int request_code {  get; set; }
        public int ingredients_code {  get; set; }
        public int? quantity { get; set; }

        public void input(string quant)
        {
            if (int.TryParse(quant, out int qu))
            {
                quantity = qu;
            }
        }
    }
}
