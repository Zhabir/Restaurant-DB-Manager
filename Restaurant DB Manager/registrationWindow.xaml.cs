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
    /// Логика взаимодействия для registrationWindow.xaml
    /// </summary>
    public partial class registrationWindow : Window
    {
        private const string ConnectionStringTemplate = "Host={0};Username={1};Password={2};Database={3}";
        private readonly string _host = "localhost";
        private readonly string _database = "public_catering";
        private string _currentUser = null;
        private string _currentUserPassword = null;
        private NpgsqlConnection _connection = null;
        public registrationWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Text;
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                statusLbl.Foreground = Brushes.Red;
                statusLbl.Content = "Пожалуйста, введите логин и пароль.";
                return;
            }
            if (AuthenticateUser(login, password))
            {
                statusLbl.Foreground = Brushes.Green;
                statusLbl.Content = "Успешная аутентификация";
                _currentUser = txtLogin1.Text;
                _currentUserPassword = txtPassword1.Text;
                CreateRoleAndGrantPermissions(_currentUser, _currentUserPassword, _connection);
                this.Close();
            }
            else
            {
                statusLbl.Foreground = Brushes.Red;
                statusLbl.Content = "Неверный логин или пароль";
            }
        }
        private bool AuthenticateUser(string login, string password)
        {
            try
            {
                string connectionString = string.Format(ConnectionStringTemplate, _host, login, password, _database);
                _connection = new NpgsqlConnection(connectionString);
                _connection.Open();
                return true;
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Аутентификация не удалась: {ex.Message}");
                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
                return false;
            }
        }
        private void CreateRoleAndGrantPermissions(string login, string password, NpgsqlConnection connection)
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    string createRoleSql = $"CREATE ROLE {login} WITH LOGIN PASSWORD '{password}';";
                    using (var createRoleCommand = new NpgsqlCommand(createRoleSql, connection, transaction))
                    {
                        createRoleCommand.ExecuteNonQuery();
                    }

                    using (var grantConnectCommand = new NpgsqlCommand($"GRANT CONNECT ON DATABASE public_catering TO {login};", connection, transaction))
                    {
                        grantConnectCommand.ExecuteNonQuery();
                    }

                    using (var grantUsageCommand = new NpgsqlCommand($"GRANT USAGE ON SCHEMA public TO {login};", connection, transaction))
                    {
                        grantUsageCommand.ExecuteNonQuery();
                    }

                    using (var grantSelectCommand = new NpgsqlCommand($"GRANT SELECT ON ALL TABLES IN SCHEMA public TO {login};", connection, transaction))
                    {
                        grantSelectCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    statusLbl.Foreground = Brushes.Green;
                    statusLbl.Content = "Регистрация прошла успешно";
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    statusLbl.Foreground = Brushes.Red;
                    statusLbl.Content = "Ошибка регистрации";
                }
            }   
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
