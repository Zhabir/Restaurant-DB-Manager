using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_DB_Manager
{
    class Ingredients
    {
        public string? ingredients_name {  get; set; }
        public int measurement_code { get; set; }
        public float? price_increment { get; set; }
        public int? remains { get; set; }
        public int vendor_code { get; set; }

        public void input(string name, int meas_code, string price, string rems, int vend_code)
        {
            ingredients_name = name;
            measurement_code = meas_code;
            float.TryParse(price, out float price_inc);
            price_increment = price_inc;
            int.TryParse(rems, out int remains_);
            remains = remains_;
            vendor_code = vend_code;
        }
        public string check_input(string? price_increment, string? remains)
        {
            string return_string = "";
            if(price_increment != null)
            {
                if (float.TryParse(price_increment, out float price))
                {
                    if (price <= 10 || price >= 1000)
                    {
                            return_string += "Ценовая надбавка должна быть в пределах от 10 до 1000 включительно. ";
                    }
                }
                else
                {
                    return_string += "Ценовая надбавка должна быть числом. ";
                }
            }
            if (remains != null)
            {
                if (!int.TryParse(remains, out int rems))
                {
                    return_string += "Остатки должны быть целым числом. ";
                }
            }
            return return_string;
        }
    }
}
