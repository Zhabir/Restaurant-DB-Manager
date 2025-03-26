using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_DB_Manager
{
    class DocumentScripts
    {
        public readonly string product_count_based_on_groups = "SELECT\r\n    g.groups_name,\r\n    COUNT(p.product_id) AS product_count\r\nFROM\r\n    groups g\r\nLEFT JOIN\r\n    product p ON g.groups_id = p.group_id\r\nWHERE NOT p.product_id IS NULL\r\nGROUP BY\r\n    g.groups_name\r\nORDER BY \r\n    g.groups_name;";
        public readonly string top_ten_ingredients = "SELECT\r\n    SUM(s.quantity) AS total_quantity,\r\n\ti.ingredients_name\r\nFROM\r\n    shipment_ingredients AS s\r\nJOIN\r\n    ingredients AS i ON s.ingredients_code = i.ingredients_id\r\nGROUP BY\r\n    i.ingredients_name\r\nORDER BY\r\n    total_quantity DESC,\r\n    i.ingredients_name DESC\r\nLIMIT 10;";
    }
}
