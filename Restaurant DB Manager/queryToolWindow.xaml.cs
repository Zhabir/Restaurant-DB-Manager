using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using Microsoft.Win32;
using Npgsql;
using static MaterialDesignThemes.Wpf.Theme;
using CheckBox = System.Windows.Controls.CheckBox;
using ComboBox = System.Windows.Controls.ComboBox;
using TextBox = System.Windows.Controls.TextBox;
namespace Restaurant_DB_Manager
{
    /// <summary>
    /// Логика взаимодействия для queryToolWindow.xaml
    /// </summary>
    public partial class queryToolWindow : Window
    {
        NpgsqlConnection _connection;
        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();
        public queryToolWindow(NpgsqlConnection connection, string sql)
        {
            InitializeComponent();
            _connection = connection;
            script_text.Document.Blocks.Clear();
            Paragraph paragraph = new Paragraph
            {
                Margin = new Thickness(0)
            };
            paragraph.Inlines.Add(new Run(""));
            script_text.Document.Blocks.Add(paragraph);
            execute_script(sql);
        }

        private void execute_script(string sql)
        {
            if (sql == "") return;
            try
            {
                using (var command = new NpgsqlCommand(sql, _connection))
                {
                    command.ExecuteNonQuery();
                }
                db_table_view(sql);
            }
            catch (NpgsqlException ex)
            {
                error_text.Document.Blocks.Clear();
                MessageBox.Show("Ошибка выполнения скрипта");
                error_text.AppendText(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неожиданная ошибка:{ex.Message}");
            }
        }
        private void execute_button_Click(object sender, RoutedEventArgs e)
        {
            var textRange = new TextRange(script_text.Document.ContentStart, script_text.Document.ContentEnd);
            execute_script(textRange.Text);
        }

        private void back_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void db_table_view(string sql)
        {
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, _connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            datagrid.ItemsSource = dt.DefaultView;
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*",
                Title = "Сохранить данные таблицы",
                DefaultExt = ".csv",
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    var csvcontent = new StringBuilder();
                    var csvContent = new StringBuilder();

                    var headers = new List<string>();
                    foreach (var column in datagrid.Columns)
                    {
                        string header = column.Header.ToString();
                        headers.Add(EscapeCsv(header));
                    }

                    csvContent.AppendLine(string.Join(",", headers));


                    foreach (var rowItem in datagrid.Items)
                    {
                        List<string> currentRowValues = new List<string>();

                        foreach (var column in datagrid.Columns)
                        {
                            var cellContent = column.GetCellContent(rowItem);
                            string cellText = GetTextFromContent(cellContent);
                            string csvSafeText = EscapeCsv(cellText);
                            currentRowValues.Add(csvSafeText);
                        }

                        csvContent.AppendLine(string.Join(",", currentRowValues));
                    }

                    File.WriteAllText(saveFileDialog.FileName, csvContent.ToString(), Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения:{ex.Message}");
                }
            }
        }
        private string GetTextFromContent(object content)
        {
            return content switch
            {
                TextBlock textBlock => textBlock.Text,
                TextBox textBox => textBox.Text,
                ComboBox comboBox => comboBox.Text,
                CheckBox checkBox => checkBox.IsChecked?.ToString() ?? "False", _ => content?.ToString() ?? string.Empty
            };
        }
        private string EscapeCsv(string value)
        {
            if (value.Contains(",") || value.Contains("\""))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }
    }
}
