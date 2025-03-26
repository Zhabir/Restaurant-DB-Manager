using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
namespace Restaurant_DB_Manager
{
    /// <summary>
    /// Логика взаимодействия для updateRecordWindow.xaml
    /// </summary>
    public partial class updateRecordWindow : Window
    {
        NpgsqlConnection _connection;
        string _table;
        byte[] _imageBytes;
        public updateRecordWindow(NpgsqlConnection connection, string tablename)
        {
            InitializeComponent();
            _connection = connection;
            _table = tablename;
            create_at_first();
        }
        private void create_at_first()
        {
            var lbl = new Label
            {
                Margin = new Thickness(5),
                Content = "Выберите id строки которую хотите изменить"
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
                Content = "Изменить"
            };
            button.Click += button_Click;
            stkPanel.Children.Add(lbl);
            comboBox.SelectedIndex = 0;
            stkPanel.Children.Add(comboBox);
            stkPanel.RegisterName(comboBox.Name, comboBox);
            stkPanel.Children.Add(button);
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            createBoxes();
        }
        private void createBoxes()
        {
            if (_table == "bank")
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
            else if (_table == "division")
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
            else if (_table == "groups")
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
            else if (_table == "ingredients")
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
            else if (_table == "ingredients_in_product")
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
            else if (_table == "ingredients_request")
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
            else if (_table == "measurement_unit")
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
            else if (_table == "product")
            {
                var button1 = new Button
                {
                    Margin = new Thickness(5),
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
            else if (_table == "product_revenue")
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
            else if (_table == "request")
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
            else if (_table == "revenue")
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
            else if (_table == "shipment")
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
            else if (_table == "shipment_ingredients")
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
            else if (_table == "street")
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
            else if (_table == "vendor")
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
            var updateButton = new Button
            {
                Margin = new Thickness(5),
                Name = "Update",
                Content = "Подтвердить изменения"
            };
            updateButton.Click += UpdateButton_Click;
            var backButton = new Button
            {
                Margin = new Thickness(5),
                Name = "Back",
                Content = "Назад"
            };
            backButton.Click += BackButton_Click;
            stkPanel.Children.Add(updateButton);
            stkPanel.Children.Add(backButton);
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_table== "ingredients")
            {
                ingredientsUpdate(_connection);
            }
            else if (_table== "bank")
            {
                bankUpdate(_connection);
            }
            else if (_table== "division")
            {
                divisionUpdate(_connection);
            }
            else if (_table== "groups")
            {
                groupsUpdate(_connection);
            }
            else if (_table== "ingredients_in_product")
            {
                ingredients_in_productUpdate(_connection);
            }
            else if (_table== "ingredients_request")
            {
                ingredients_requestUpdate(_connection);
            }
            else if (_table== "measurement_unit")
            {
                measurement_unitUpdate(_connection);
            }
            else if (_table== "product")
            {
                productUpdate(_connection);
            }
            else if (_table== "product_revenue")
            {
                product_revenueUpdate(_connection);
            }
            else if (_table== "request")
            {
                requestUpdate(_connection);
            }
            else if (_table== "revenue")
            {
                revenueUpdate(_connection);
            }
            else if (_table== "shipment")
            {
                shipmentUpdate(_connection);
            }
            else if (_table== "shipment_ingredients")
            {
                shipment_ingredientsUpdate(_connection);
            }
            else if (_table== "street")
            {
                streetUpdate(_connection);
            }
            else if (_table== "vendor")
            {
                vendorUpdate(_connection);
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
        private void bankUpdate(NpgsqlConnection connection)
        {
            TextBox textBox = stkPanel.FindName("bank_name") as TextBox;
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            Bank bank = new Bank();
            if (textBox != null)
            {
                int.TryParse(cmb.Text, out int bank_code);
                bank.bank_name = textBox.Text;
                try
                {
                    using (var command = new NpgsqlCommand("update bank SET bank_name = @bank.bank_name where bank_id = @bank_code;", connection))
                    {
                        command.Parameters.AddWithValue("@bank.bank_name", bank.bank_name);
                        command.Parameters.AddWithValue("@bank_code", bank_code);
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
        private void divisionUpdate(NpgsqlConnection connection)
        {
            TextBox textBox = stkPanel.FindName("division_name") as TextBox;
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            Division division = new Division();
            if (textBox != null)
            {
                int.TryParse(cmb.Text, out int div_code);
                division.division_name = textBox.Text;
                try
                {
                    using (var command = new NpgsqlCommand("update division SET division_name = @division.division_name where division_id = @div_code;", connection))
                    {
                        command.Parameters.AddWithValue("@division.division_name", division.division_name);
                        command.Parameters.AddWithValue("@div_code", div_code);
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
        private void groupsUpdate(NpgsqlConnection connection)
        {
            TextBox textBox = stkPanel.FindName("groups_name") as TextBox;
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            Groups groups = new Groups();
            if (textBox != null)
            {
                int.TryParse(cmb.Text, out int groups_code);
                groups.groups_name = textBox.Text;
                try
                {
                    using (var command = new NpgsqlCommand("update groups SET groups_name = @groups.groups_name where groups_id = @groups_code;", connection))
                    {
                        command.Parameters.AddWithValue("@groups.groups_name", groups.groups_name);
                        command.Parameters.AddWithValue("@groups_code", groups_code);
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
        private void ingredientsUpdate(NpgsqlConnection connection)
        {
            TextBox textBoxName = stkPanel.FindName("ingredients_name") as TextBox;
            ComboBox comboBoxMeasure = stkPanel.FindName("measurement_code") as ComboBox;
            TextBox textBoxPrice = stkPanel.FindName("price_increment") as TextBox;
            TextBox textBoxRemain = stkPanel.FindName("remains") as TextBox;
            ComboBox comboBoxVendor = stkPanel.FindName("vendor_code") as ComboBox;
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            if (comboBoxMeasure.Items.Count > 0 && comboBoxVendor.Items.Count > 0)
            {
                int.TryParse(comboBoxMeasure.Text, out int meas_code);
                int.TryParse(comboBoxVendor.Text, out int vend_code);
                int.TryParse(cmb.Text, out int ing_code);
                Ingredients i = new Ingredients();
                string check = "";
                check = i.check_input(textBoxPrice.Text, textBoxRemain.Text);
                if (check == "")
                {
                    i.input(textBoxName.Text, meas_code, textBoxPrice.Text, textBoxRemain.Text, vend_code);
                    try
                    {
                        using (var command = new NpgsqlCommand("update ingredients SET ingredients_name = @i.ingredients_name, measurement_code = @i.measurement_code, price_increment = @i.price_increment, remains = @i.remains, vendor_code = @i.vendor_code where ingredients_id = @ing_code;", connection))
                        {
                            command.Parameters.AddWithValue("@i.ingredients_name", i.ingredients_name);
                            command.Parameters.AddWithValue("@i.measurement_code", i.measurement_code);
                            command.Parameters.AddWithValue("@i.price_increment", i.price_increment);
                            command.Parameters.AddWithValue("@i.remains", i.remains);
                            command.Parameters.AddWithValue("@i.vendor_code", i.vendor_code);
                            command.Parameters.AddWithValue("@ing_code", ing_code);
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
        private void ingredients_in_productUpdate(NpgsqlConnection connection)
        {
            ComboBox comboBoxProduct = stkPanel.FindName("product_code") as ComboBox;
            ComboBox comboBoxIngredients = stkPanel.FindName("ingredients_code") as ComboBox;
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            if (comboBoxProduct.Items.Count > 0 && comboBoxIngredients.Items.Count > 0)
            {
                int.TryParse(comboBoxProduct.Text, out int prod_code);
                int.TryParse(comboBoxIngredients.Text, out int ing_code);
                int.TryParse(cmb.Text, out int ingp_code);
                Ingredients_in_product ir = new Ingredients_in_product();
                ir.product_code = prod_code;
                ir.ingredients_code = ing_code;
                try
                {
                    using (var command = new NpgsqlCommand("update ingredients_in_product SET product_code = @ir.product_code, ingredients_code = @ir.ingredients_code where ingredients_in_product_id = @ingp_code;", connection))
                    {
                        command.Parameters.AddWithValue("@ir.product_code", ir.product_code);
                        command.Parameters.AddWithValue("@ir.ingredients_code", ir.ingredients_code);
                        command.Parameters.AddWithValue("@ingp_code", ingp_code);
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
        private void ingredients_requestUpdate(NpgsqlConnection connection)
        {
            ComboBox comboBoxRequest = stkPanel.FindName("request_code") as ComboBox;
            ComboBox comboBoxIngredients = stkPanel.FindName("ingredients_code") as ComboBox;
            TextBox textBoxQuantity = stkPanel.FindName("quantity") as TextBox;
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            if (comboBoxRequest.Items.Count > 0 && comboBoxIngredients.Items.Count > 0)
            {
                int.TryParse(comboBoxRequest.Text, out int req_code);
                int.TryParse(comboBoxIngredients.Text, out int ing_code);
                int.TryParse(cmb.Text, out int ingp_code);
                Ingredients_request ir = new Ingredients_request();
                ir.request_code = req_code;
                ir.ingredients_code = ing_code;
                ir.input(textBoxQuantity.Text);
                try
                {
                    using (var command = new NpgsqlCommand("update ingredients_request SET request_code = @ir.request_code, ingredients_code = @ir.ingredients_code, quantity = @ir.quantity where ingredients_request_id = @ingp_code;", connection))
                    {
                        command.Parameters.AddWithValue("@ir.request_code", ir.request_code);
                        command.Parameters.AddWithValue("@ir.ingredients_code", ir.ingredients_code);
                        command.Parameters.AddWithValue("@ir.quantity", ir.quantity);
                        command.Parameters.AddWithValue("@ingp_code", ingp_code);
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
        private void measurement_unitUpdate(NpgsqlConnection connection)
        {
            TextBox txtName = stkPanel.FindName("measurement_unit_name") as TextBox;
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            int.TryParse(cmb.Text, out int _code);
            if (txtName != null)
            {
                Measurement_unit mUnit = new Measurement_unit();
                mUnit.measurement_unit_name = txtName.Text;
                try
                {
                    using (var command = new NpgsqlCommand("update measurement_unit SET measurement_unit_name = @mUnit.measurement_unit_name where measurement_unit_id = @_code;", connection))
                    {
                        command.Parameters.AddWithValue("@mUnit.measurement_unit_name", mUnit.measurement_unit_name);
                        command.Parameters.AddWithValue("@_code", _code);
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
        private void productUpdate(NpgsqlConnection connection)
        {
            TextBox tName = stkPanel.FindName("product_name") as TextBox;
            ComboBox cGroup = stkPanel.FindName("group_id") as ComboBox;
            ComboBox cMUnit = stkPanel.FindName("measurement_id") as ComboBox;
            TextBox tPrice = stkPanel.FindName("price") as TextBox;
            TextBox tExit = stkPanel.FindName("exit") as TextBox;
            TextBox tTech = stkPanel.FindName("technology") as TextBox;
            TextBox tRecipy = stkPanel.FindName("recipy") as TextBox;
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            int.TryParse(cmb.Text, out int _code);
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
                        using (var command = new NpgsqlCommand(@"update product SET product_name = @p.product_name, group_id = @p.group_id, measurement_id = @p.measurement_id, price = @p.price, photo = @p.photo, exit = @p.exit, technology = @p.technology, recipy = @p.recipy where product_id = @_code;", connection))
                        {
                            command.Parameters.AddWithValue("@p.product_name", p.product_name);
                            command.Parameters.AddWithValue("@p.group_code", p.group_code);
                            command.Parameters.AddWithValue("@p.measurement_id", p.measurement_id);
                            command.Parameters.AddWithValue("@p.price", p.price);
                            command.Parameters.AddWithValue("@p.photo", p.photo);
                            command.Parameters.AddWithValue("@p.exit", p.exit);
                            command.Parameters.AddWithValue("@p.technology", p.technology);
                            command.Parameters.AddWithValue("@p.recipy", p.recipy);
                            command.Parameters.AddWithValue("@_code", _code);
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
        private void product_revenueUpdate(NpgsqlConnection connection)
        {
            ComboBox comboBoxProduct = stkPanel.FindName("product_code") as ComboBox;
            ComboBox comboBoxRevenue = stkPanel.FindName("revenue_code") as ComboBox;
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            int.TryParse(cmb.Text, out int _code);
            if (comboBoxProduct.Items.Count > 0 && comboBoxRevenue.Items.Count > 0)
            {
                int.TryParse(comboBoxProduct.Text, out int prod_code);
                int.TryParse(comboBoxRevenue.Text, out int ing_code);
                Product_revenue pr = new Product_revenue();
                pr.product_code = prod_code;
                pr.revenue_code = ing_code;
                try
                {
                    using (var command = new NpgsqlCommand("update product_revenue SET product_code = @pr.product_code, revenue_code = @pr.revenue_code where product_revenue_id = @_code;", connection))
                    {
                        command.Parameters.AddWithValue("@pr.product_code", pr.product_code);
                        command.Parameters.AddWithValue("@pr.revenue_code", pr.revenue_code);
                        command.Parameters.AddWithValue("_code", _code);
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
        private void requestUpdate(NpgsqlConnection connection)
        {
            ComboBox comboBoxDivision = stkPanel.FindName("division_code") as ComboBox;
            DatePicker dtReq = stkPanel.FindName("request_Date") as DatePicker;
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            int.TryParse(cmb.Text, out int _code);
            if (comboBoxDivision.Items.Count > 0)
            {
                int.TryParse(comboBoxDivision.Text, out int div_code);
                Request r = new Request();
                r.division_code = div_code;
                r.request_date = (DateTime)dtReq.SelectedDate;
                try
                {
                    using (var command = new NpgsqlCommand("update request SET division_code = @r.division_code, request_date = @r.request_date where request_id = @_code;", connection))
                    {
                        command.Parameters.AddWithValue("@r.division_code", r.division_code);
                        command.Parameters.AddWithValue("@r.request_date", r.request_date);
                        command.Parameters.AddWithValue("@_code", _code);
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
        private void revenueUpdate(NpgsqlConnection connection)
        {
            DatePicker dtRev = stkPanel.FindName("revenue_Date") as DatePicker;
            TextBox tAmount = stkPanel.FindName("revenue_amount") as TextBox;
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            int.TryParse(cmb.Text, out int _code);
            if (dtRev != null && dtRev.SelectedDate != null)
            {
                Revenue r = new Revenue();
                r.revenue_date = (DateTime)dtRev.SelectedDate;
                if (float.TryParse(tAmount.Text, out float rev_amount)) r.revenue_amount = rev_amount;
                try
                {
                    using (var command = new NpgsqlCommand("update revenue SET revenue_date = @r.revenue_date, revenue_amount = @r.revenue_amount where revenue_id = @_code;", connection))
                    {
                        command.Parameters.AddWithValue("@r.revenue_date", r.revenue_date);
                        command.Parameters.AddWithValue("@r.revenue_amount", r.revenue_amount);
                        command.Parameters.AddWithValue("@_code", _code);
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
        private void shipmentUpdate(NpgsqlConnection connection)
        {
            ComboBox comboBoxVendor = stkPanel.FindName("vendor_code") as ComboBox;
            DatePicker dtSh = stkPanel.FindName("shipment_Date") as DatePicker;
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            int.TryParse(cmb.Text, out int _code);
            if (comboBoxVendor.Items.Count > 0)
            {
                Shipment s = new Shipment();
                int.TryParse(comboBoxVendor.Text, out int ven_code);
                s.vendor_code = ven_code;
                s.shipment_date = (DateTime)dtSh.SelectedDate;
                try
                {
                    using (var command = new NpgsqlCommand("update shipment SET vendor_code = @s.vendor_code, shipment_date = @s.shipment_date where shipment_id = @_code;", connection))
                    {
                        command.Parameters.AddWithValue("@s.vendor_code", s.vendor_code);
                        command.Parameters.AddWithValue("@s.shipment_date", s.shipment_date);
                        command.Parameters.AddWithValue("@_code", _code);
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
        private void shipment_ingredientsUpdate(NpgsqlConnection connection)
        {
            ComboBox comboBoxShipment = stkPanel.FindName("shipment_code") as ComboBox;
            ComboBox comboBoxIngredients = stkPanel.FindName("ingredients_code") as ComboBox;
            TextBox textBoxQuantity = stkPanel.FindName("quantity") as TextBox;
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            int.TryParse(cmb.Text, out int _code);
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
                    using (var command = new NpgsqlCommand("update shipment_ingredients SET shipment_code = @si.shipment_code, ingredients_code = @si.ingredients_code, quantity = @si.quantity where shipment_ingredients_id = @_code;", connection))
                    {
                        command.Parameters.AddWithValue("@si.shipment_code", si.shipment_code);
                        command.Parameters.AddWithValue("@si.ingredients_code", si.ingredients_code);
                        command.Parameters.AddWithValue("@si.quantity", si.quantity);
                        command.Parameters.AddWithValue("@_code", _code);
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
        private void streetUpdate(NpgsqlConnection connection)
        {
            TextBox textBox = stkPanel.FindName("street_name") as TextBox;
            Street s = new Street();
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            int.TryParse(cmb.Text, out int _code);
            if (textBox != null)
            {
                s.street_name = textBox.Text;
                try
                {
                    using (var command = new NpgsqlCommand("update street SET street_name = @s.street_name where street_id = @_code;", connection))
                    {
                        command.Parameters.AddWithValue("@s.street_name", s.street_name);
                        command.Parameters.AddWithValue("@_code", _code);
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
        private void vendorUpdate(NpgsqlConnection connection)
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
            ComboBox cmb = stkPanel.FindName("cmbName") as ComboBox;
            int.TryParse(cmb.Text, out int _code);
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
                    using (var command = new NpgsqlCommand("update vendor SET vendor_name = @v.vendor_name, house_number = @v.house_number, street_code = @v.street_code, surname = @v.surname, firstname = @v.firstname, thirdname = @v.thirdname, bank_code = @v.bank_code, inn_number = @v.inn_number, account_number = @v.account_number where vendor_id = @_code;", connection))
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
                        command.Parameters.AddWithValue("@_code", _code);
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
