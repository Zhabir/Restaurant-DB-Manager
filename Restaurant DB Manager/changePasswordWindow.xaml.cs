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
namespace Restaurant_DB_Manager
{
    /// <summary>
    /// Логика взаимодействия для changePasswordWindow.xaml
    /// </summary>
    public partial class changePasswordWindow : Window
    {
        NpgsqlConnection _connection;
        public changePasswordWindow(NpgsqlConnection connection)
        {
            InitializeComponent();
            _connection = connection;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(txtPassword.Text == txtNewPassword.Text && txtNewPassword1.Text == txtPassword.Text)
            {
                try
                {
                    using (var command = new NpgsqlCommand($"ALTER USER CURRENT_USER WITH PASSWORD '{txtPassword.Text}'", _connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    statusLbl.Foreground = Brushes.Green;
                    statusLbl.Content = "Пароль успешно изменен";
                }
                catch (NpgsqlException ex)
                {
                    MessageBox.Show($"Ошибка смены пароля:{ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Неожиданная ошибка: {ex.Message}");
                }
            }
            else
            {
                statusLbl.Foreground = Brushes.Red;
                statusLbl.Content = "Пароли должны быть одинаковыми";
            }
        }
    }
}
