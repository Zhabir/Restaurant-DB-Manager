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
using Microsoft.Win32;
using Npgsql;
using MaterialDesignThemes;
using static MaterialDesignThemes.Wpf.Theme;
using TextBox = System.Windows.Controls.TextBox;
using ComboBox = System.Windows.Controls.ComboBox;
using Button = System.Windows.Controls.Button;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
namespace Restaurant_DB_Manager
{
    /// <summary>
    /// Логика взаимодействия для addRecordWindow.xaml
    /// </summary>
    public partial class addRecordWindow : Window
    {
        private NpgsqlConnection _connection = new NpgsqlConnection();
        private string _tableName;
        private byte[] _imageBytes;
        private DateTime selectedDate = new DateTime();
        public addRecordWindow(NpgsqlConnection connection, string tablename)
        {
            InitializeComponent();
            _connection = connection;
            _tableName = tablename;
            createBoxes();
        }

        private void createBoxes()
        {
            if (_tableName == "bank")
            {
                var textBox1 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "bank_name",
                    MaxLength = 50,
                };
                stkPanel.Children.Add(textBox1);
                stkPanel.RegisterName(textBox1.Name, textBox1);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox1, "Название банка");
            }
            else if(_tableName == "division")
            {
                var textBox1 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "division_name",
                    MaxLength = 50,
                };
                stkPanel.Children.Add(textBox1);
                stkPanel.RegisterName(textBox1.Name, textBox1);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox1, "Название подразделения");
            }
            else if(_tableName == "groups")
            {
                var textBox1 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "groups_name",
                    MaxLength = 50,
                };
                stkPanel.Children.Add(textBox1);
                stkPanel.RegisterName(textBox1.Name, textBox1);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox1, "Название группы");
            }
            else if( _tableName == "ingredients")
            {
                var textBox1 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "ingredients_name",
                    MaxLength = 50,
                };

                var comboBox = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "measurement_code"
                };
                using (var cmd = new NpgsqlCommand("SELECT measurement_unit_id FROM measurement_unit", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox.Items.Add(reader["measurement_unit_id"].ToString());
                        }
                    }
                }
                var textBox2 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "price_increment"
                };
                textBox2.PreviewTextInput += RealNumberValidationTextBox;
                var textBox3 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "remains"
                };
                textBox3.PreviewTextInput += IntNumberValidationTextBox;
                var comboBox1 = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "vendor_code"
                };
                using (var cmd = new NpgsqlCommand("SELECT vendor_id FROM vendor", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["vendor_id"].ToString());
                        }
                    }
                }
                comboBox1.SelectedIndex = 0;
                comboBox.SelectedIndex = 0;
                stkPanel.Children.Add(textBox1);
                stkPanel.RegisterName(textBox1.Name, textBox1);
                stkPanel.Children.Add(comboBox);
                stkPanel.RegisterName(comboBox.Name, comboBox);
                stkPanel.Children.Add(textBox2);
                stkPanel.RegisterName(textBox2.Name, textBox2);
                stkPanel.Children.Add(textBox3);
                stkPanel.RegisterName(textBox3.Name, textBox3);
                stkPanel.Children.Add(comboBox1);
                stkPanel.RegisterName(comboBox1.Name, comboBox1);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox1, "Название ингредиента");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox, "Код единицы измерения");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox2, "Добавочная стоимость");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox3, "Остатки");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox1, "Код поставщика");
            }
            else if( _tableName == "ingredients_in_product")
            {
                var comboBox1 = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "product_code"
                };
                using (var cmd = new NpgsqlCommand("SELECT product_id FROM product", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["product_id"].ToString());
                        }
                    }
                }
                var comboBox2 = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "ingredients_code"
                };
                using (var cmd = new NpgsqlCommand("SELECT ingredients_id FROM ingredients", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox2.Items.Add(reader["ingredients_id"].ToString());
                        }
                    }
                }
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                stkPanel.Children.Add(comboBox1);
                stkPanel.RegisterName(comboBox1.Name, comboBox1);
                stkPanel.Children.Add(comboBox2);
                stkPanel.RegisterName(comboBox2.Name, comboBox2);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox1, "Код продукции");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox2, "Код ингредиента");
            }
            else if( _tableName == "ingredients_request")
            {
                var comboBox1 = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "request_code"
                };
                using (var cmd = new NpgsqlCommand("SELECT request_id FROM request", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["request_id"].ToString());
                        }
                    }
                }
                var comboBox2 = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "ingredients_code"
                };
                using (var cmd = new NpgsqlCommand("SELECT ingredients_id FROM ingredients", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox2.Items.Add(reader["ingredients_id"].ToString());
                        }
                    }
                }
                var textBox1 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "quantity"
                };
                textBox1.PreviewTextInput += IntNumberValidationTextBox;
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                stkPanel.Children.Add(comboBox1);
                stkPanel.RegisterName(comboBox1.Name, comboBox1);
                stkPanel.Children.Add(comboBox2);
                stkPanel.RegisterName(comboBox2.Name, comboBox2);
                stkPanel.Children.Add(textBox1);
                stkPanel.RegisterName(textBox1.Name, textBox1);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox1, "Код заявки");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox2, "Код ингредиента");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox1, "Количество ингредиента");
            }
            else if( _tableName == "measurement_unit")
            {
                var textBox1 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "measurement_unit_name",
                    MaxLength = 50
                };
                stkPanel.Children.Add(textBox1);
                stkPanel.RegisterName(textBox1.Name, textBox1);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox1, "Название единицы измерения");
            }
            else if( _tableName == "product")
            {
                var button1 = new Button
                {
                    Margin= new Thickness(5),
                    Name = "select_image",
                    Content = "Выбрать фото"
                };
                button1.Click += button1_click;
                var textBox1 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "product_name",
                    MaxLength = 50
                };
                var comboBox1 = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "group_id"
                };
                using (var cmd = new NpgsqlCommand("SELECT groups_id FROM groups", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["groups_id"].ToString());
                        }
                    }
                }
                var comboBox = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "measurement_id"
                };
                using (var cmd = new NpgsqlCommand("SELECT measurement_unit_id FROM measurement_unit", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox.Items.Add(reader["measurement_unit_id"].ToString());
                        }
                    }
                }
                var textBox2 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "price"
                };
                textBox2.PreviewTextInput += RealNumberValidationTextBox;
                var textBox3 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "exit"
                };
                textBox3.PreviewTextInput += RealNumberValidationTextBox;
                var textBox4 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "technology"
                };
                var textBox5 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "recipy"
                };
                comboBox1.SelectedIndex = 0;
                comboBox.SelectedIndex = 0;
                stkPanel.Children.Add(textBox1);
                stkPanel.RegisterName(textBox1.Name, textBox1);
                stkPanel.Children.Add(comboBox1);
                stkPanel.RegisterName(comboBox1.Name, comboBox1);
                stkPanel.Children.Add(comboBox);
                stkPanel.RegisterName(comboBox.Name, comboBox);
                stkPanel.Children.Add(textBox2);
                stkPanel.RegisterName(textBox2.Name, textBox2);
                stkPanel.Children.Add(button1);
                stkPanel.RegisterName(button1.Name, button1);
                stkPanel.Children.Add(textBox3);
                stkPanel.RegisterName(textBox3.Name, textBox3);
                stkPanel.Children.Add(textBox4);
                stkPanel.RegisterName(textBox4.Name, textBox4);
                stkPanel.Children.Add(textBox5);
                stkPanel.RegisterName(textBox5.Name, textBox5);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox1, "Название продукции");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox1, "Код группы");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox, "Код единицы измерения");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox2, "Цена");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox3, "Выход");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox4, "Технология приготовления");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox5, "Рецепт");
            }
            else if( _tableName == "product_revenue")
            {
                var comboBox1 = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "product_code"
                };
                using (var cmd = new NpgsqlCommand("SELECT product_id FROM product", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["product_id"].ToString());
                        }
                    }
                }
                var comboBox2 = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "revenue_code"
                };
                using (var cmd = new NpgsqlCommand("SELECT revenue_id FROM revenue", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox2.Items.Add(reader["revenue_id"].ToString());
                        }
                    }
                }
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                stkPanel.Children.Add(comboBox1);
                stkPanel.RegisterName(comboBox1.Name, comboBox1);
                stkPanel.Children.Add(comboBox2);
                stkPanel.RegisterName(comboBox2.Name, comboBox2);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox1, "Код продукции");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox2, "Код выручки");
            }
            else if( _tableName == "request")
            {
                var comboBox1 = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "division_code"
                };
                using (var cmd = new NpgsqlCommand("SELECT division_id FROM division", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["division_id"].ToString());
                        }
                    }
                }
                comboBox1.SelectedIndex = 0;
                var datebox = new DatePicker
                {
                    Margin = new Thickness(5),
                    Name = "request_Date",
                    SelectedDateFormat = DatePickerFormat.Short
                };
                datebox.SelectedDateChanged += selectedDateChanged;
                stkPanel.Children.Add(comboBox1);
                stkPanel.RegisterName(comboBox1.Name, comboBox1);
                stkPanel.Children.Add(datebox);
                stkPanel.RegisterName(datebox.Name, datebox);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox1, "Код подразделения");
            }
            else if( _tableName == "revenue")
            {
                var datebox = new DatePicker
                {
                    Margin = new Thickness(5),
                    Name = "revenue_Date",
                    SelectedDateFormat = DatePickerFormat.Short
                };
                datebox.SelectedDateChanged += selectedDateChanged;
                var textBox1 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "revenue_amount"
                };
                textBox1.PreviewTextInput += RealNumberValidationTextBox;
                stkPanel.Children.Add(datebox);
                stkPanel.RegisterName(datebox.Name, datebox);
                stkPanel.Children.Add(textBox1);
                stkPanel.RegisterName(textBox1.Name, textBox1);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox1, "Размер выручки");

            }
            else if( _tableName == "shipment")
            {
                var comboBox1 = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "vendor_code"
                };
                using (var cmd = new NpgsqlCommand("SELECT vendor_id FROM vendor", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["vendor_id"].ToString());
                        }
                    }
                }
                comboBox1.SelectedIndex = 0;
                var datebox = new DatePicker
                {
                    Margin = new Thickness(5),
                    Name = "shipment_Date",
                    SelectedDateFormat = DatePickerFormat.Short
                };
                datebox.SelectedDateChanged += selectedDateChanged;
                stkPanel.Children.Add(comboBox1);
                stkPanel.RegisterName(comboBox1.Name, comboBox1);
                stkPanel.Children.Add(datebox);
                stkPanel.RegisterName(datebox.Name, datebox);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox1, "Код поставщика");
            }
            else if( _tableName == "shipment_ingredients")
            {
                var comboBox1 = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "shipment_code"
                };
                using (var cmd = new NpgsqlCommand("SELECT shipment_id FROM shipment", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["shipment_id"].ToString());
                        }
                    }
                }
                var comboBox2 = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "ingredients_code"
                };
                using (var cmd = new NpgsqlCommand("SELECT ingredients_id FROM ingredients", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox2.Items.Add(reader["ingredients_id"].ToString());
                        }
                    }
                }
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                var textBox1 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "quantity"
                };
                textBox1.PreviewTextInput += IntNumberValidationTextBox;
                stkPanel.Children.Add(comboBox1);
                stkPanel.RegisterName(comboBox1.Name, comboBox1);
                stkPanel.Children.Add(comboBox2);
                stkPanel.RegisterName(comboBox2.Name, comboBox2);
                stkPanel.Children.Add(textBox1);
                stkPanel.RegisterName(textBox1.Name, textBox1);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox1, "Код поставки");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox2, "Код ингредиента");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox1, "Количество");
            }
            else if( _tableName == "street")
            {
                var textBox1 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "street_name",
                    MaxLength = 50
                };
                stkPanel.Children.Add(textBox1);
                stkPanel.RegisterName(textBox1.Name, textBox1);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox1, "Название улицы");
            }
            else if( _tableName == "vendor")
            {
                var textBox1 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "vendor_name",
                    MaxLength = 50
                };
                var textBox2 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "house_number"
                };
                textBox2.PreviewTextInput += HouseNumberValidationTextBox;
                var comboBox1 = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "street_code"
                };
                using (var cmd = new NpgsqlCommand("SELECT street_id FROM street", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["street_id"].ToString());
                        }
                    }
                }
                comboBox1.SelectedIndex = 0;
                var textBox3 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "surname",
                    MaxLength = 50
                };
                var textBox4 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "firstname",
                    MaxLength = 50
                };
                var textBox5 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "thirdname",
                    MaxLength = 50
                };
                var comboBox2 = new ComboBox
                {
                    Margin = new Thickness(5),
                    Name = "bank_code"
                };
                using (var cmd = new NpgsqlCommand("SELECT bank_id FROM bank", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox2.Items.Add(reader["bank_id"].ToString());
                        }
                    }
                }
                comboBox2.SelectedIndex = 0;
                var textBox6 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "inn_number",
                    MaxLength = 50
                };
                var textBox7 = new TextBox
                {
                    Margin = new Thickness(5),
                    Name = "account_number",
                    MaxLength = 50
                };
                stkPanel.Children.Add(textBox1);
                stkPanel.RegisterName(textBox1.Name, textBox1);
                stkPanel.Children.Add(textBox2);
                stkPanel.RegisterName(textBox2.Name, textBox2);
                stkPanel.Children.Add(comboBox1);
                stkPanel.RegisterName(comboBox1.Name, comboBox1);
                stkPanel.Children.Add(textBox3);
                stkPanel.RegisterName(textBox3.Name, textBox3);
                stkPanel.Children.Add(textBox4);
                stkPanel.RegisterName(textBox4.Name, textBox4);
                stkPanel.Children.Add(textBox5);
                stkPanel.RegisterName(textBox5.Name, textBox5);
                stkPanel.Children.Add(comboBox2);
                stkPanel.RegisterName(comboBox2.Name, comboBox2);
                stkPanel.Children.Add(textBox6);
                stkPanel.RegisterName(textBox6.Name, textBox6);
                stkPanel.Children.Add(textBox7);
                stkPanel.RegisterName(textBox7.Name, textBox7);
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox1, "Название поставщика");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox2, "Номер дома");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox1, "Код улицы");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox3, "Фамилия");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox4, "Имя");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox5, "Отчество");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(comboBox2, "Код банка");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox6, "Номер ИНН");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(textBox7, "Номер счета");
            }
            var addButton = new Button
            {
                Margin = new Thickness(5),
                Name = "Add",
                Content = "Добавить"
            };
            addButton.Click += AddButton_Click;
            var backButton = new Button
            {
                Margin = new Thickness(5),
                Name = "Back",
                Content = "Назад"
            };
            backButton.Click += BackButton_Click;
            stkPanel.Children.Add(addButton);
            stkPanel.Children.Add(backButton);
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(_tableName == "ingredients")
            {
                ingredientsAdd(_connection);
            }
            else if( _tableName == "bank")
            {
                bankAdd(_connection);
            }
            else if(_tableName == "division")
            {
                divisionAdd(_connection);
            }
            else if(_tableName == "groups")
            {
                groupsAdd(_connection);
            }
            else if(_tableName == "ingredients_in_product")
            {
                ingredients_in_productAdd(_connection);
            }
            else if(_tableName == "ingredients_request")
            {
                ingredients_requestAdd(_connection);
            }
            else if(_tableName == "measurement_unit")
            {
                measurement_unitAdd(_connection);
            }
            else if(_tableName == "product")
            {
                productAdd(_connection);
            }
            else if (_tableName == "product_revenue")
            {
                product_revenueAdd(_connection);
            }
            else if (_tableName == "request")
            {
                requestAdd(_connection);
            }
            else if (_tableName == "revenue")
            {
                revenueAdd(_connection);
            }
            else if(_tableName == "shipment")
            {
                shipmentAdd(_connection);
            }
            else if(_tableName == "shipment_ingredients")
            {
                shipment_ingredientsAdd(_connection);
            }
            else if(_tableName == "street")
            {
                streetAdd(_connection);
            }
            else if( _tableName == "vendor")
            {
                vendorAdd(_connection);
            }

        }

        private void button1_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*",
                Title = "Выберите изображение"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _imageBytes = File.ReadAllBytes(openFileDialog.FileName);
            }
        }
        private void selectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker picker = sender as DatePicker;
            if (picker != null)
            {
                if (picker.SelectedDate.HasValue)
                {
                    DateTime selectedDate = picker.SelectedDate.Value;
                }
            }
        }
        private void IntNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }
        private void RealNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string newText = textBox.Text + e.Text;

            Regex regex = new Regex(@"^[0-9]*(?:\,[0-9]*)?$");

            if (!regex.IsMatch(newText))
            {
                e.Handled = true;
            }
        }
        private void HouseNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string newText = textBox.Text + e.Text;

            Regex regex = new Regex(@"^\d+(?:\/\d+)?$");

            if (!regex.IsMatch(newText))
            {
                e.Handled = true;
            }
        }
        private void bankAdd(NpgsqlConnection connection)
        {
            TextBox textBox = stkPanel.FindName("bank_name") as TextBox;
            Bank bank = new Bank();
            if(textBox != null)
            {
                bank.bank_name = textBox.Text;
                try
                {
                    using (var command = new NpgsqlCommand("insert into bank (bank_name) values (@bank.bank_name);", connection))
                    {
                        command.Parameters.AddWithValue("@bank.bank_name", bank.bank_name);
                        command.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show($"Ошибка добавления: {e.Message}");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Неизвестная ошибка: {e.Message}");
                }
            }
        }
        private void divisionAdd(NpgsqlConnection connection)
        {
            TextBox textBox = stkPanel.FindName("division_name") as TextBox;
            Division division = new Division();
            if (textBox != null)
            {
                division.division_name = textBox.Text;
                try
                {
                    using (var command = new NpgsqlCommand("insert into division (division_name) values (@division.division_name);", connection))
                    {
                        command.Parameters.AddWithValue("@division.division_name", division.division_name);
                        command.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show($"Ошибка добавления: {e.Message}");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Неизвестная ошибка: {e.Message}");
                }
            }
        }
        private void groupsAdd(NpgsqlConnection connection)
        {
            TextBox textBox = stkPanel.FindName("groups_name") as TextBox;
            Groups groups = new Groups();
            if (textBox != null)
            {
                groups.groups_name = textBox.Text;
                try
                {
                    using (var command = new NpgsqlCommand("insert into groups (groups_name) values (@groups.groups_name);", connection))
                    {
                        command.Parameters.AddWithValue("@groups.groups_name", groups.groups_name);
                        command.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show($"Ошибка добавления: {e.Message}");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Неизвестная ошибка: {e.Message}");
                }
            }
        }
        private void ingredientsAdd(NpgsqlConnection connection)
        {
            TextBox textBoxName = stkPanel.FindName("ingredients_name") as TextBox;
            ComboBox comboBoxMeasure = stkPanel.FindName("measurement_code") as ComboBox;
            TextBox textBoxPrice = stkPanel.FindName("price_increment") as TextBox;
            TextBox textBoxRemain = stkPanel.FindName("remains") as TextBox;
            ComboBox comboBoxVendor = stkPanel.FindName("vendor_code") as ComboBox;
            if (comboBoxMeasure.Items.Count > 0 && comboBoxVendor.Items.Count > 0)
            {
                int.TryParse(comboBoxMeasure.Text, out int meas_code);
                int.TryParse(comboBoxVendor.Text, out int vend_code);
                Ingredients i = new Ingredients();
                string check = "";
                check = i.check_input(textBoxPrice.Text, textBoxRemain.Text);
                if (check == "")
                {
                    i.input(textBoxName.Text, meas_code, textBoxPrice.Text, textBoxRemain.Text, vend_code);
                    try
                    {
                        using (var command = new NpgsqlCommand("insert into ingredients (ingredients_name, measurement_code, price_increment, remains, vendor_code) values (@i.ingredients_name, @i.measurement_code, @i.price_increment, @i.remains, @i.vendor_code);", connection))
                        {
                            command.Parameters.AddWithValue("@i.ingredients_name", i.ingredients_name);
                            command.Parameters.AddWithValue("@i.measurement_code", i.measurement_code);
                            command.Parameters.AddWithValue("@i.price_increment", i.price_increment);
                            command.Parameters.AddWithValue("@i.remains", i.remains);
                            command.Parameters.AddWithValue("@i.vendor_code", i.vendor_code);
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (NpgsqlException e)
                    {
                        MessageBox.Show($"Ошибка добавления: {e.Message}");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Неизвестная ошибка: {e.Message}");
                    }
                }
                else
                {
                    MessageBox.Show(check);
                }
            }
            else
            {
                MessageBox.Show("Добавьте сначала поставщика и единицы измерения");
            }
        }
        private void ingredients_in_productAdd(NpgsqlConnection connection)
        {
            ComboBox comboBoxProduct = stkPanel.FindName("product_code") as ComboBox;
            ComboBox comboBoxIngredients = stkPanel.FindName("ingredients_code") as ComboBox;
            if (comboBoxProduct.Items.Count > 0 && comboBoxIngredients.Items.Count > 0)
            {
                int.TryParse(comboBoxProduct.Text, out int prod_code);
                int.TryParse(comboBoxIngredients.Text, out int ing_code);
                Ingredients_in_product ir = new Ingredients_in_product();
                ir.product_code = prod_code;
                ir.ingredients_code = ing_code;
                try
                {
                    using (var command = new NpgsqlCommand("insert into ingredients_in_product (product_code, ingredients_code) values (@ir.product_code, @ir.ingredients_code);", connection))
                    {
                        command.Parameters.AddWithValue("@ir.product_code", ir.product_code);
                        command.Parameters.AddWithValue("@ir.ingredients_code", ir.ingredients_code);
                        command.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show($"Ошибка добавления: {e.Message}");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Неизвестная ошибка: {e.Message}");
                }
            }
            else
            {
                MessageBox.Show("Добавьте сначала продукцию и ингредиенты в базу данных");
            }
        }
        private void ingredients_requestAdd(NpgsqlConnection connection)
        {
            ComboBox comboBoxRequest = stkPanel.FindName("request_code") as ComboBox;
            ComboBox comboBoxIngredients = stkPanel.FindName("ingredients_code") as ComboBox;
            TextBox textBoxQuantity = stkPanel.FindName("quantity") as TextBox;
            if (comboBoxRequest.Items.Count > 0 && comboBoxIngredients.Items.Count > 0)
            {
                int.TryParse(comboBoxRequest.Text, out int req_code);
                int.TryParse(comboBoxIngredients.Text, out int ing_code);
                Ingredients_request ir = new Ingredients_request();
                ir.request_code = req_code;
                ir.ingredients_code = ing_code;
                ir.input(textBoxQuantity.Text);
                try
                {
                    using (var command = new NpgsqlCommand("insert into ingredients_request (request_code, ingredients_code, quantity) values (@ir.request_code, @ir.ingredients_code, @ir.quantity);", connection))
                    {
                        command.Parameters.AddWithValue("@ir.request_code", ir.request_code);
                        command.Parameters.AddWithValue("@ir.ingredients_code", ir.ingredients_code);
                        command.Parameters.AddWithValue("@ir.quantity", ir.quantity);
                        command.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show($"Ошибка добавления: {e.Message}");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Неизвестная ошибка: {e.Message}");
                }
            }
            else
            {
                MessageBox.Show("Сначала добавьте заявки и ингредиенты");
            }
        }
        private void measurement_unitAdd(NpgsqlConnection connection)
        {
            TextBox txtName = stkPanel.FindName("measurement_unit_name") as TextBox;
            if (txtName != null)
            {
                Measurement_unit mUnit = new Measurement_unit();
                mUnit.measurement_unit_name = txtName.Text;
                try
                {
                    using (var command = new NpgsqlCommand("insert into measurement_unit (measurement_unit_name) values (@mUnit.measurement_unit_name);", connection))
                    {
                        command.Parameters.AddWithValue("@mUnit.measurement_unit_name", mUnit.measurement_unit_name);
                        command.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show($"Ошибка добавления: {e.Message}");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Неизвестная ошибка: {e.Message}");
                }
            }
        }
        private void productAdd(NpgsqlConnection connection)
        {
            TextBox tName = stkPanel.FindName("product_name") as TextBox;
            ComboBox cGroup = stkPanel.FindName("group_id") as ComboBox;
            ComboBox cMUnit = stkPanel.FindName("measurement_id") as ComboBox;
            TextBox tPrice = stkPanel.FindName("price") as TextBox;
            TextBox tExit = stkPanel.FindName("exit") as TextBox;
            TextBox tTech = stkPanel.FindName("technology") as TextBox;
            TextBox tRecipy = stkPanel.FindName("recipy") as TextBox;
            if (cGroup.Items.Count > 0 && cMUnit.Items.Count > 0)
            {
                Product p = new Product();
                int.TryParse(cGroup.Text, out int g_code);
                int.TryParse(cMUnit.Text, out int m_code);
                string alert = p.input(tName.Text, g_code, m_code, tPrice.Text, _imageBytes, tExit.Text, tTech.Text, tRecipy.Text);
                if (alert == "")
                {
                    try
                    {
                        using (var command = new NpgsqlCommand(@"insert into product (product_name, group_id, measurement_id, price, photo, exit, technology, recipy) values (@p.product_name, @p.group_code, @p.measurement_id, @p.price, @p.photo, @p.exit, @p.technology, @p.recipy);", connection))
                        {
                            command.Parameters.AddWithValue("@p.product_name", p.product_name);
                            command.Parameters.AddWithValue("@p.group_code", p.group_code);
                            command.Parameters.AddWithValue("@p.measurement_id", p.measurement_id);
                            command.Parameters.AddWithValue("@p.price", p.price);
                            command.Parameters.AddWithValue("@p.photo", p.photo);
                            command.Parameters.AddWithValue("@p.exit", p.exit);
                            command.Parameters.AddWithValue("@p.technology", p.technology);
                            command.Parameters.AddWithValue("@p.recipy", p.recipy);
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (NpgsqlException e)
                    {
                        MessageBox.Show($"Ошибка добавления: {e.Message}");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Неизвестная ошибка: {e.Message}");
                    }
                }
                else
                {
                    MessageBox.Show($"{alert}");
                }
            }
            else
            {
                MessageBox.Show("Сначала добавьте группы и единицы измерения");
            }
        }
        private void product_revenueAdd(NpgsqlConnection connection)
        {
            ComboBox comboBoxProduct = stkPanel.FindName("product_code") as ComboBox;
            ComboBox comboBoxRevenue = stkPanel.FindName("revenue_code") as ComboBox;
            if (comboBoxProduct.Items.Count > 0 && comboBoxRevenue.Items.Count > 0)
            {
                int.TryParse(comboBoxProduct.Text, out int prod_code);
                int.TryParse(comboBoxRevenue.Text, out int ing_code);
                Product_revenue pr = new Product_revenue();
                pr.product_code = prod_code;
                pr.revenue_code = ing_code;
                try
                {
                    using (var command = new NpgsqlCommand("insert into product_revenue (product_code, revenue_code) values (@pr.product_code, @pr.revenue_code);", connection))
                    {
                        command.Parameters.AddWithValue("@pr.product_code", pr.product_code);
                        command.Parameters.AddWithValue("@pr.revenue_code", pr.revenue_code);
                        command.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show($"Ошибка добавления: {e.Message}");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Неизвестная ошибка: {e.Message}");
                }
            }
            else
            {
                MessageBox.Show("Добавьте сначала продукцию и выручку в базу данных");
            }
        }
        private void requestAdd(NpgsqlConnection connection)
        {
            ComboBox comboBoxDivision = stkPanel.FindName("division_code") as ComboBox;
            DatePicker dtReq = stkPanel.FindName("request_Date") as DatePicker;
            if (comboBoxDivision.Items.Count > 0)
            {
                int.TryParse(comboBoxDivision.Text, out int div_code);
                Request r = new Request();
                r.division_code = div_code;
                r.request_date = (DateTime)dtReq.SelectedDate;
                try
                {
                    using (var command = new NpgsqlCommand("insert into request (division_code, request_date) values (@r.division_code, @r.request_date);", connection))
                    {
                        command.Parameters.AddWithValue("@r.division_code", r.division_code);
                        command.Parameters.AddWithValue("@r.request_date", r.request_date);
                        command.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show($"Ошибка добавления: {e.Message}");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Неизвестная ошибка: {e.Message}");
                }
            }
            else
            {
                MessageBox.Show("Добавьте сначала подразделения в базу данных");
            }
        }
        private void revenueAdd(NpgsqlConnection connection)
        {
            DatePicker dtRev = stkPanel.FindName("revenue_Date") as DatePicker;
            TextBox tAmount = stkPanel.FindName("revenue_amount") as TextBox;
            if (dtRev != null && dtRev.SelectedDate != null)
            {
                Revenue r = new Revenue();
                r.revenue_date = (DateTime)dtRev.SelectedDate;
                if(float.TryParse(tAmount.Text, out float rev_amount)) r.revenue_amount = rev_amount;
                try
                {
                    using (var command = new NpgsqlCommand("insert into revenue (revenue_date, revenue_amount) values (@r.revenue_date, @r.revenue_amount);", connection))
                    {
                        command.Parameters.AddWithValue("@r.revenue_date", r.revenue_date);
                        command.Parameters.AddWithValue("@r.revenue_amount", r.revenue_amount);
                        command.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show($"Ошибка добавления: {e.Message}");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Неизвестная ошибка: {e.Message}");
                }
            }
        }
        private void shipmentAdd(NpgsqlConnection connection)
        {
            ComboBox comboBoxVendor = stkPanel.FindName("vendor_code") as ComboBox;
            DatePicker dtSh = stkPanel.FindName("shipment_Date") as DatePicker;
            if (comboBoxVendor.Items.Count > 0)
            {
                Shipment s = new Shipment();
                int.TryParse(comboBoxVendor.Text, out int ven_code);
                s.vendor_code = ven_code;
                s.shipment_date = (DateTime)dtSh.SelectedDate;
                try
                {
                    using (var command = new NpgsqlCommand("insert into shipment (vendor_code, shipment_date) values (@s.vendor_code, @s.shipment_date);", connection))
                    {
                        command.Parameters.AddWithValue("@s.vendor_code", s.vendor_code);
                        command.Parameters.AddWithValue("@s.shipment_date", s.shipment_date);
                        command.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show($"Ошибка добавления: {e.Message}");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Неизвестная ошибка: {e.Message}");
                }
            }
            else
            {
                MessageBox.Show("Сначала добавьте поставщиков");
            }
        }
        private void shipment_ingredientsAdd(NpgsqlConnection connection)
        {
            ComboBox comboBoxShipment = stkPanel.FindName("shipment_code") as ComboBox;
            ComboBox comboBoxIngredients = stkPanel.FindName("ingredients_code") as ComboBox;
            TextBox textBoxQuantity = stkPanel.FindName("quantity") as TextBox;
            if (comboBoxShipment.Items.Count > 0 && comboBoxIngredients.Items.Count > 0)
            {
                int.TryParse(comboBoxShipment.Text, out int ship_code);
                int.TryParse(comboBoxIngredients.Text, out int ing_code);
                Shipment_ingredients si = new Shipment_ingredients();
                si.shipment_code = ship_code;
                si.ingredients_code = ing_code;
                if (int.TryParse(textBoxQuantity.Text, out int amount)) si.quantity = amount;
                try
                {
                    using (var command = new NpgsqlCommand("insert into shipment_ingredients (shipment_code, ingredients_code, quantity) values (@si.shipment_code, @si.ingredients_code, @si.quantity);", connection))
                    {
                        command.Parameters.AddWithValue("@si.shipment_code", si.shipment_code);
                        command.Parameters.AddWithValue("@si.ingredients_code", si.ingredients_code);
                        command.Parameters.AddWithValue("@si.quantity", si.quantity);
                        command.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show($"Ошибка добавления: {e.Message}");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Неизвестная ошибка: {e.Message}");
                }
            }
            else
            {
                MessageBox.Show("Сначала добавьте поставки и ингредиенты");
            }
        }
        private void streetAdd(NpgsqlConnection connection)
        {
            TextBox textBox = stkPanel.FindName("street_name") as TextBox;
            Street s = new Street();
            if (textBox != null)
            {
                s.street_name = textBox.Text;
                try
                {
                    using (var command = new NpgsqlCommand("insert into street (street_name) values (@s.street_name);", connection))
                    {
                        command.Parameters.AddWithValue("@s.street_name", s.street_name);
                        command.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show($"Ошибка добавления: {e.Message}");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Неизвестная ошибка: {e.Message}");
                }
            }
        }
        private void vendorAdd(NpgsqlConnection connection)
        {
            TextBox tName = stkPanel.FindName("vendor_name") as TextBox;
            TextBox tHouse = stkPanel.FindName("house_number") as TextBox;
            ComboBox cStret = stkPanel.FindName("street_code") as ComboBox;
            TextBox tSurName = stkPanel.FindName("surname") as TextBox;
            TextBox tFirstName = stkPanel.FindName("firstname") as TextBox;
            TextBox tThirdName = stkPanel.FindName("thirdname") as TextBox;
            ComboBox cBank = stkPanel.FindName("bank_code") as ComboBox;
            TextBox tINN = stkPanel.FindName("inn_number") as TextBox;
            TextBox tAccount = stkPanel.FindName("account_number") as TextBox;
            Vendor v = new Vendor();

            if (cStret.Items.Count > 0 && cBank.Items.Count > 0)
            {
                v.vendor_name = tName.Text;
                v.house_number = tHouse.Text;
                int.TryParse(cStret.Text, out int str_code);
                v.street_code = str_code;
                v.surname = tSurName.Text;
                v.firstname = tFirstName.Text;
                v.thirdname = tThirdName.Text;
                int.TryParse(cBank.Text, out int b_code);
                v.bank_code = b_code;
                v.inn_number = tINN.Text;
                v.account_number = tAccount.Text;
                try
                {
                    using (var command = new NpgsqlCommand("insert into vendor (vendor_name, house_number, street_code, surname, firstname, thirdname, bank_code, inn_number, account_number) values (@v.vendor_name, @v.house_number, @v.street_code, @v.surname, @v.firstname, @v.thirdname, @v.bank_code, @v.inn_number, @v.account_number);", connection))
                    {
                        command.Parameters.AddWithValue("@v.vendor_name", v.vendor_name);
                        command.Parameters.AddWithValue("@v.house_number", v.house_number);
                        command.Parameters.AddWithValue("@v.street_code", v.street_code);
                        command.Parameters.AddWithValue("@v.surname", v.surname);
                        command.Parameters.AddWithValue("@v.firstname", v.firstname);
                        command.Parameters.AddWithValue("@v.thirdname", v.thirdname);
                        command.Parameters.AddWithValue("@v.bank_code", v.bank_code);
                        command.Parameters.AddWithValue("@v.inn_number", v.inn_number);
                        command.Parameters.AddWithValue("@v.account_number", v.account_number);
                        command.ExecuteNonQuery();
                    }
                }
                catch (NpgsqlException e)
                {
                    MessageBox.Show($"Ошибка добавления: {e.Message}");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Неизвестная ошибка: {e.Message}");
                }
            }
            else
            {
                MessageBox.Show("Сначала добавьте улицы и банки");
            }
        }
    }
}
