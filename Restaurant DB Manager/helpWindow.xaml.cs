using System;
using System.Collections.Generic;
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

namespace Restaurant_DB_Manager
{
    /// <summary>
    /// Логика взаимодействия для helpWindow.xaml
    /// </summary>
    public partial class helpWindow : Window
    {
        public helpWindow()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedText ="";
            if (MenuListBox.SelectedItem is ListBoxItem selectedItem)
            {
                selectedText = selectedItem.Content.ToString();
            }
            MenuListBox.SelectedItem = null;

            switch (selectedText)
            {
                case "Просмотр таблиц":
                    string text = File.ReadAllText("table_view_help.txt");
                    help_text.Document.Blocks.Clear();
                    help_text.Document.Blocks.Add(new Paragraph(new Run(text)));
                    break;
                case "Добавление":
                    string text2 = File.ReadAllText("insert_help.txt");
                    help_text.Document.Blocks.Clear();
                    help_text.Document.Blocks.Add(new Paragraph(new Run(text2)));
                    break;
                case "Удаление":
                    string text1 = File.ReadAllText("delete_helpt.txt");
                    help_text.Document.Blocks.Clear();
                    help_text.Document.Blocks.Add(new Paragraph(new Run(text1)));
                    break;
                case "Изменение":
                    string text3 = File.ReadAllText("update_help.txt");
                    help_text.Document.Blocks.Clear();
                    help_text.Document.Blocks.Add(new Paragraph(new Run(text3)));
                    break;
                case "Поиск":
                    string text4 = File.ReadAllText("search_help.txt");
                    help_text.Document.Blocks.Clear();
                    help_text.Document.Blocks.Add(new Paragraph(new Run(text4)));
                    break;
                case "Запуск скриптов":
                    string text5 = File.ReadAllText("script_help.txt");
                    help_text.Document.Blocks.Clear();
                    help_text.Document.Blocks.Add(new Paragraph(new Run(text5)));
                    break;
                case "Смена пароля":
                    string text6 = File.ReadAllText("change_pass_help.txt");
                    help_text.Document.Blocks.Clear();
                    help_text.Document.Blocks.Add(new Paragraph(new Run(text6)));
                    break;
                default:
                    break;

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
