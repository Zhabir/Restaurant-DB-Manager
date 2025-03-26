using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace Restaurant_DB_Manager
{
    class Product
    {
        public string? product_name {get; set;}
        public int group_code {get; set;}
        public int measurement_id { get; set;}
        public float? price {get; set;}
        public byte[]? photo {get; set;}
        public float? exit { get; set;}
        public string? technology { get; set;}
        public string? recipy {get; set;}

        public string input(string? prod_name, int g_code, int m_code, string? prod_price, byte[]? prod_photo, string? prod_exit, string? prod_tech, string? prod_rec)
        {
            string return_string = "";
            product_name = prod_name;
            group_code = g_code;
            measurement_id = m_code;
            photo = prod_photo;
            if (prod_price != null)
            {
                if (float.TryParse(prod_price, out float tmp))
                {
                    if (tmp >= 100 && tmp < 10000)
                    {
                        price = tmp;
                    }
                    else
                    {
                        return_string += "Цена должна быть в диапазоне от 100 до 10000";
                    }
                }
            }
            if(prod_exit != null)
            {
                if(float.TryParse(prod_exit, out float tmp))
                {
                    exit = tmp;
                }
            }
            technology = prod_tech;
            recipy = prod_rec;
            return return_string;
        }
    }
}
