using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
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
using Npgsql;
namespace Restaurant_DB_Manager
{
    /// <summary>
    /// Логика взаимодействия для searchRecordWindow.xaml
    /// </summary>
    public partial class searchRecordWindow : Window
    {
        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();
        NpgsqlConnection _connection;
        string _tableName;
        public searchRecordWindow(NpgsqlConnection connection, string tablename)
        {
            InitializeComponent();
            _connection = connection;
            _tableName = tablename;
        }

        private void back_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            string search_text = search_txtBox.Text;
            SearchSQL s = new SearchSQL();
            search(search_text, s.SearchScript(_tableName));
        }
        private void search(string search_text, string sql)
        {
            try
            {
                NpgsqlCommand command = new NpgsqlCommand(sql, _connection);
                command.Parameters.AddWithValue("@search_string", $"%{search_text}%");
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
                ds.Reset();
                da.Fill(ds);
                dt = ds.Tables[0];
                grid.ItemsSource = dt.DefaultView;
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
            }
        }
    }
}
