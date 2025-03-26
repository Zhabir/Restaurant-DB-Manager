using System;
using System.Collections.Generic;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace Restaurant_DB_Manager
{
    /// <summary>
    /// Логика взаимодействия для deleteRecordWindow.xaml
    /// </summary>
    public partial class deleteRecordWindow : Window
    {
        NpgsqlConnection _connection;
        string _table;
        public deleteRecordWindow(NpgsqlConnection connection, string table)
        {
            InitializeComponent();
            _connection = connection;
            _table = table;
            create_at_first();
        }
        private void create_at_first()
        {
            var lbl = new Label
            {
                Margin = new Thickness(5),
                Content = "Выберите id строки которую хотите удалить"
            };
            var comboBox = new ComboBox
            {
                Margin = new Thickness(5),
                Name = "cmbName"
            };
            string tb_id = _table + "_id";
            using (var cmd = new NpgsqlCommand($"SELECT {tb_id} FROM {_table}", _connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox.Items.Add(reader[$"{tb_id}"].ToString());
                    }
                }
            }
            var button = new Button
            {
                Margin = new Thickness(5),
                Content = "Удалить"
            };
            var backButton = new Button
            {
                Margin = new Thickness(5),
                Name = "Back",
                Content = "Назад"
            };
            backButton.Click += BackButton_Click;
            stkPanel.Children.Add(backButton);
            button.Click += button_Click_Delete;
            stkPanel.Children.Add(lbl);
            comboBox.SelectedIndex = 0;
            stkPanel.Children.Add(comboBox);
            stkPanel.RegisterName(comboBox.Name, comboBox);
            stkPanel.Children.Add(button);
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void button_Click_Delete(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            int.TryParse(cmb.Text, out int tab_id);
            string tb_id = _table + "_id";
            try
            {
                using (var command = new NpgsqlCommand($"delete from {_table} where {tb_id} = {tab_id};", _connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
            }
        }
    }
}
