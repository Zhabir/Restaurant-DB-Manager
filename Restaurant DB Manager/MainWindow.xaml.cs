using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;
namespace Restaurant_DB_Manager;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const string ConnectionStringTemplate = "Host={0};Username={1};Password={2};Database={3}";
    private readonly string _host = "localhost";
    private readonly string _database = "public_catering";
    private string _currentUser = null;
    private NpgsqlConnection _connection = null;
    public MainWindow()
    {
        InitializeComponent();
        this.Left = 200;
        this.Top = 200;
    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        string login = txtLogin.Text;
        string password = txtPassword.Text;
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            statusLbl.Content = "Пожалуйста, введите логин и пароль.";
            return;
        }
        if(AuthenticateUser(login, password))
        {
            _currentUser = login;
            statusLbl.Foreground = Brushes.Green;
            statusLbl.Content = "Успешная аутентификация";
            Window menu = new menuWindow(_connection, login);
            menu.Owner = this;
            this.Visibility = Visibility.Hidden;
            menu.Closed += child_window_closed;
            menu.Top = 200;
            menu.Left = 200;
            menu.ShowDialog();
        }
        else
        {
            statusLbl.Foreground = Brushes.Red;
            statusLbl.Content = "Неверный логин или пароль";
        }
    }
    private void child_window_closed(object sender, EventArgs e)
    {
        this.Visibility = Visibility.Visible;
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

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        Window regisWindow = new registrationWindow();
        regisWindow.Left = 200;
        regisWindow.Top = 200;
        regisWindow.ShowDialog();
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        _connection.Close();
    }
}