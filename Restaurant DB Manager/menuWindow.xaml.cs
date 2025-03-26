using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace Restaurant_DB_Manager
{
    /// <summary>
    /// Логика взаимодействия для menuWindow.xaml
    /// </summary>
    public partial class menuWindow : Window
    {
        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();
        NpgsqlConnection _connection = new NpgsqlConnection();
        string role;
        string _currentTable = "bank";
        string _sql = "SELECT * FROM bank;";
        public menuWindow(NpgsqlConnection connection, string username)
        {
            InitializeComponent();
            _connection = connection;
            role= username;
            insert_button.Visibility = Visibility.Hidden;
            update_button.Visibility = Visibility.Hidden;
            delete_button.Visibility = Visibility.Hidden;
            seach_button.Visibility = Visibility.Hidden;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT \r\n    p.product_id,\r\n\tp.product_name,\r\n\tg.groups_name,\r\n\tm.measurement_unit_name,\r\n\tp.price,\r\n\tp.photo,\r\n\tp.exit,\r\n\tp.technology,\r\n\tp.recipy\r\nFROM\r\n    product p\r\nJOIN\r\n    groups g ON p.group_id = g.groups_id\r\nJOIN\r\n\tmeasurement_unit m ON p.measurement_id = m.measurement_unit_id";
            db_table_view("product", sql);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM bank;";
            db_table_view("bank", sql);
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM division;";
            db_table_view("division", sql);
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM groups;";
            db_table_view("groups", sql);
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT\r\n\ti.ingredients_id,\r\n\ti.ingredients_name,\r\n\tm.measurement_unit_name,\r\n\ti.price_increment,\r\n\ti.remains,\r\n\tv.vendor_name\r\nFROM\r\n\tingredients i\r\nJOIN\r\n\tmeasurement_unit m ON i.measurement_code = m.measurement_unit_id\r\nJOIN\r\n\tvendor v ON i.vendor_code = v.vendor_id";
            db_table_view("ingredients", sql);
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT\r\n\ti.ingredients_in_product_id,\r\n    p.product_name,\r\n\tir.ingredients_name\r\nFROM\r\n    product p\r\nJOIN\r\n    ingredients_in_product i ON p.product_id = i.product_code\r\nJOIN\r\n\tingredients ir ON i.ingredients_code = ir.ingredients_id";
            db_table_view("ingredients_in_product", sql);
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT\r\n\ti.ingredients_request_id,\r\n\td.division_name,\r\n\tr.request_date,\r\n\tir.ingredients_name,\r\n\ti.quantity\r\nFROM\r\n    ingredients_request i\r\nJOIN\r\n    request r ON r.request_id = i.request_code\r\nJOIN\r\n\tdivision d ON d.division_id = r.division_code\r\nJOIN\r\n\tingredients ir ON i.ingredients_code = ir.ingredients_id";
            db_table_view("ingredients_request", sql);
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM measurement_unit;";
            db_table_view("measurement_unit", sql);
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT\r\n\tpr.product_revenue_id,\r\n    p.product_name,\r\n\tr.revenue_amount,\r\n\tr.revenue_date\r\nFROM\r\n    product p\r\nJOIN\r\n    product_revenue pr ON p.product_id = pr.product_code\r\nJOIN\r\n    revenue r ON pr.revenue_code = r.revenue_id";
            db_table_view("product_revenue", sql);
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT\r\n\tr.request_id,\r\n\td.division_name,\r\n\tr.request_date\r\nFROM\r\n    request r\r\nJOIN\r\n    division d ON d.division_id = r.division_code";
            db_table_view("request", sql);
        }

        private void MenuItem_Click_10(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM revenue;";
            db_table_view("revenue", sql);
        }

        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT\r\n\ts.shipment_id,\r\n\tv.vendor_name,\r\n\ts.shipment_date\r\nFROM \r\n\tshipment s\r\nJOIN\r\n\tvendor v ON v.vendor_id = s.vendor_code";
            db_table_view("shipment", sql);
        }

        private void MenuItem_Click_12(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT\r\n\tsh.shipment_ingredients_id,\r\n\tv.vendor_name,\r\n\ts.shipment_date,\r\n\ti.ingredients_name,\r\n\tsh.quantity\r\nFROM \r\n\tshipment_ingredients sh\r\nJOIN\r\n\tshipment s ON s.shipment_id = sh.shipment_code\r\nJOIN\r\n\tvendor v ON v.vendor_id = s.vendor_code\r\nJOIN\r\n\tingredients i ON i.ingredients_id = sh.ingredients_code";
            db_table_view("shipment_ingredients", sql);
        }

        private void MenuItem_Click_13(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM street;";
            db_table_view("street", sql);
        }

        private void MenuItem_Click_14(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT\r\n\tv.vendor_id,\r\n\tv.vendor_name,\r\n\tv.house_number,\r\n\ts.street_name,\r\n\tv.surname,\r\n\tv.firstname,\r\n\tv.thirdname,\r\n\tb.bank_name,\r\n\tv.inn_number,\r\n\tv.account_number\r\nFROM\r\n\tvendor v\r\nJOIN\r\n\tstreet s ON s.street_id = v.street_code\r\nJOIN\r\n\tbank b ON b.bank_id = v.bank_code";
            db_table_view("vendor", sql);
        }

        private static UserPermissions GettUserPermissions(NpgsqlConnection connection, string role, string table)
        {
            UserPermissions permissions = new UserPermissions();
            using (var command = new NpgsqlCommand($"SELECT has_table_privilege('{role}', '{table}', 'INSERT')", connection))
            {
                permissions.CanInsert = (bool)command.ExecuteScalar();
            }

            using (var command = new NpgsqlCommand($"SELECT has_table_privilege('{role}', '{table}', 'DELETE')", connection))
            {
                permissions.CanDelete = (bool)command.ExecuteScalar();
            }

            using (var command = new NpgsqlCommand($"SELECT has_table_privilege('{role}', '{table}', 'UPDATE')", connection))
            {
                permissions.CanUpdate = (bool)command.ExecuteScalar();
            }

            return permissions;
        }

        private void role_buttons(string table)
        {
            insert_button.Visibility = Visibility.Hidden;
            update_button.Visibility = Visibility.Hidden;
            delete_button.Visibility = Visibility.Hidden;
            UserPermissions permissions = new UserPermissions();
            permissions = GettUserPermissions(_connection, role, table);
            if (permissions.CanInsert)
            {
                insert_button.Visibility = Visibility.Visible;
            }
            if (permissions.CanDelete)
            {
                delete_button.Visibility = Visibility.Visible;
            }
            if (permissions.CanUpdate)
            {
                update_button.Visibility = Visibility.Visible;
            }
            seach_button.Visibility = Visibility.Visible;
        }

        private void db_table_view(string table, string sql)
        {
            role_buttons(table);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, _connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            testGrid.ItemsSource = dt.DefaultView;
            _currentTable = table;
            _sql = sql;
        }

        private void insert_button_Click(object sender, RoutedEventArgs e)
        {
            Window addRecord = new addRecordWindow(_connection, _currentTable);
            addRecord.Owner = this;
            this.Visibility = Visibility.Hidden;
            addRecord.Closed += child_window_closed;
            addRecord.Top = 200;
            addRecord.Left = 200;
            addRecord.ShowDialog();
        }
        private void child_window_closed(object sender, EventArgs e)
        {
            db_table_view(_currentTable, _sql);
            this.Visibility = Visibility.Visible;
        }

        private void update_button_Click(object sender, RoutedEventArgs e)
        {
            Window updateRecord = new updateRecordWindow(_connection, _currentTable);
            updateRecord.Owner = this;
            this.Visibility = Visibility.Hidden;
            updateRecord.Closed += child_window_closed;
            updateRecord.Top = 200;
            updateRecord.Left = 200;
            updateRecord.ShowDialog();
        }

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            Window deleteRecord = new deleteRecordWindow(_connection, _currentTable);
            deleteRecord.Owner = this;
            this.Visibility = Visibility.Hidden;
            deleteRecord.Closed += child_window_closed;
            deleteRecord.Top = 200;
            deleteRecord.Left = 200;
            deleteRecord.ShowDialog();
        }

        private void seach_button_Click(object sender, RoutedEventArgs e)
        {
            Window searchRecord = new searchRecordWindow(_connection, _currentTable);
            searchRecord.Owner = this;
            this.Visibility = Visibility.Hidden;
            searchRecord.Closed += child_window_closed;
            searchRecord.Top = 200;
            searchRecord.Left = 200;
            searchRecord.ShowDialog();
        }

        private void MenuItem_Click_15(object sender, RoutedEventArgs e)
        {
            Window changPass = new changePasswordWindow(_connection);
            changPass.Owner = this;
            this.Visibility = Visibility.Hidden;
            changPass.Closed += child_window_closed;
            changPass.Top = 200;
            changPass.Left = 200;
            changPass.ShowDialog();
        }

        private void MenuItem_Click_16(object sender, RoutedEventArgs e)
        {
            showQueryToolWindow("");
        }

        private void MenuItem_Click_17(object sender, RoutedEventArgs e)
        {
            _connection.Close();
            this.Close();
        }

        private void MenuItem_Click_18(object sender, RoutedEventArgs e)
        {
            Window help = new helpWindow();
            help.Owner = this;
            this.Visibility = Visibility.Hidden;
            help.Closed += child_window_closed;
            help.Top = 200;
            help.Left = 200;
            help.ShowDialog();
        }

        private void MenuItem_Click_19(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Программу разработал студент группы АП-226 Мавлонхонов Жабир. Все права незащищены");
        }
        private void showQueryToolWindow(string script)
        {
            Window queryTool = new queryToolWindow(_connection, script);
            queryTool.Owner = this;
            this.Visibility = Visibility.Hidden;
            queryTool.Closed += child_window_closed;
            queryTool.Top = 200;
            queryTool.Left = 200;
            queryTool.ShowDialog();
        }
        private void MenuItem_Click_20(object sender, RoutedEventArgs e)
        {
            DocumentScripts dc = new DocumentScripts();
            showQueryToolWindow(dc.product_count_based_on_groups);
        }

        private void MenuItem_Click_21(object sender, RoutedEventArgs e)
        {
            DocumentScripts dc = new DocumentScripts();
            showQueryToolWindow(dc.top_ten_ingredients);
        }
    }
}
