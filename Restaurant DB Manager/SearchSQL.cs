using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_DB_Manager
{
    class SearchSQL
    {
        public readonly string bank_search = "SELECT *\r\nFROM bank\r\nWHERE\r\n\tbank_id::TEXT ILIKE @search_string OR\r\n\tbank_name::TEXT ILIKE @search_string;";
        public readonly string division_search = "SELECT *\r\nFROM division\r\nWHERE\r\n\tdivision_id::TEXT ILIKE @search_string OR\r\n\tdivision_name::TEXT ILIKE @search_string;";
        public readonly string groups_search = "SELECT *\r\nFROM groups\r\nWHERE\r\n\tgroups_id::TEXT ILIKE @search_string OR\r\n\tgroups_name::TEXT ILIKE @search_string;\r\n";
        public readonly string ingredients_search = "SELECT *\r\nFROM ingredients\r\nWHERE\r\n\tingredients_id::TEXT ILIKE @search_string OR\r\n\tingredients_name::TEXT ILIKE @search_string OR\r\n\tmeasurement_code::TEXT ILIKE @search_string OR\r\n\tprice_increment::TEXT ILIKE @search_string OR\r\n\tremains::TEXT ILIKE @search_string OR\r\n\tvendor_code::TEXT ILIKE @search_string;";
        public readonly string ingredients_in_product_search = "SELECT *\r\nFROM ingredients_in_product\r\nWHERE\r\n\tingredients_in_product_id::TEXT ILIKE @search_string OR\r\n\tproduct_code::TEXT ILIKE @search_string OR\r\n\tingredients_code::TEXT ILIKE @search_string;";
        public readonly string ingredients_request_search = "SELECT *\r\nFROM ingredients_request\r\nWHERE\r\n\tingredients_request_id::TEXT ILIKE @search_string OR\r\n\trequest_code::TEXT ILIKE @search_string OR\r\n\tingredients_code::TEXT ILIKE @search_string OR\r\n\tquantity::TEXT ILIKE @search_string;";
        public readonly string measurement_search = "SELECT *\r\nFROM measurement_unit\r\nWHERE\r\n\tmeasurement_unit_id::TEXT ILIKE @search_string OR\r\n\tmeasurement_unit_name::TEXT ILIKE @search_string;";
        public readonly string product_search = "SELECT *\r\nFROM product\r\nWHERE\r\n\tproduct_id::TEXT ILIKE @search_string OR\r\n\tproduct_name::TEXT ILIKE @search_string OR\r\n\tgroup_id::TEXT ILIKE @search_string OR\r\n\tmeasurement_id::TEXT ILIKE @search_string OR\r\n\tprice::TEXT ILIKE @search_string OR\r\n\texit::TEXT ILIKE @search_string OR\r\n\ttechnology::TEXT ILIKE @search_string OR\r\n\trecipy::TEXT ILIKE @search_string;";
        public readonly string product_revenue_search = "SELECT *\r\nFROM product_revenue\r\nWHERE\r\n\tproduct_revenue_id::TEXT ILIKE @search_string OR\r\n\tproduct_code::TEXT ILIKE @search_string OR\r\n\trevenue_code::TEXT ILIKE @search_string;";
        public readonly string request_search = "SELECT *\r\nFROM request\r\nWHERE\r\n\trequest_id::TEXT ILIKE @search_string OR\r\n\tdivision_code::TEXT ILIKE @search_string OR\r\n\trequest_date::TEXT ILIKE @search_string;";
        public readonly string revenue_search = "SELECT *\r\nFROM revenue\r\nWHERE\r\n\trevenue_id::TEXT ILIKE @search_string OR\r\n\trevenue_date::TEXT ILIKE @search_string OR\r\n\trevenue_amount::TEXT ILIKE @search_string;";
        public readonly string shipment_search = "SELECT *\r\nFROM shipment\r\nWHERE\r\n\tshipment_id::TEXT ILIKE @search_string OR\r\n\tvendor_code::TEXT ILIKE @search_string OR\r\n\tshipment_date::TEXT ILIKE @search_string;";
        public readonly string shipment_ingredients_search = "SELECT *\r\nFROM shipment_ingredients\r\nWHERE\r\n\tshipment_ingredients_id::TEXT ILIKE @search_string OR\r\n\tshipment_code::TEXT ILIKE @search_string OR\r\n\tingredients_code::TEXT ILIKE @search_string OR\r\n\tquantity::TEXT ILIKE @search_string;";
        public readonly string street_search = "SELECT *\r\nFROM street\r\nWHERE\r\n\tstreet_id::TEXT ILIKE @search_string OR\r\n\tstreet_name::TEXT ILIKE @search_string;";
        public readonly string vendor_search = "SELECT *\r\nFROM vendor\r\nWHERE\r\n\tvendor_id::TEXT ILIKE @search_string OR\r\n\tvendor_name::TEXT ILIKE @search_string OR\r\n\thouse_number::TEXT ILIKE @search_string OR\r\n\tstreet_code::TEXT ILIKE @search_string OR\r\n\tsurname::TEXT ILIKE @search_string OR\r\n\tfirstname::TEXT ILIKE @search_string OR\r\n\tthirdname::TEXT ILIKE @search_string OR\r\n\tbank_code::TEXT ILIKE @search_string OR\r\n\tinn_number::TEXT ILIKE @search_string OR\r\n\taccount_number::TEXT ILIKE @search_string;";


        public string SearchScript(string tablename)
        {
            if (tablename == "bank") return bank_search;
            else if (tablename == "division") return division_search;
            else if (tablename == "groups") return groups_search;
            else if (tablename == "ingredients") return ingredients_search;
            else if (tablename == "ingredients_in_product") return ingredients_in_product_search;
            else if (tablename == "ingredients_request") return ingredients_request_search;
            else if (tablename == "measurement_unit") return measurement_search;
            else if (tablename == "product") return product_search;
            else if (tablename == "product_revenue") return product_revenue_search;
            else if (tablename == "request") return request_search;
            else if (tablename == "revenue") return revenue_search;
            else if (tablename == "shipment") return shipment_search;
            else if (tablename == "shipment_ingredients") return shipment_ingredients_search;
            else if (tablename == "street") return street_search;
            else if (tablename == "vendor") return vendor_search;
            else return "";
        }
    }
}
