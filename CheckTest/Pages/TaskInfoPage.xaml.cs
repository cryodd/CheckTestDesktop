using System;
using System.Net;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CheckTest.ViewModels;
using Microsoft.Win32;
using System.IO;
using CheckTest.API;
using System.CodeDom.Compiler;

namespace CheckTest.Pages
{
    /// <summary>
    /// Логика взаимодействия для TaskInfoPage.xaml
    /// </summary>
    public partial class TaskInfoPage : Page
    {
        int id;
        Visibility hide = Visibility.Hidden;
        public TaskInfoPage(int id)
        {
            InitializeComponent();
            this.id = id;
            var item = TasksAPI.GetTasksList().Where(x => x.IdTask == id).First();
            name.Text = item.NameTask;
            desc.Text = item.DescribeTask;
            //Скрытие добавление тестов для обычных пользователей и для неавторизированных пользователей(если вдруг такие смогут попасть на данную страницу)

            //if (Guy.CurrentUser == null) 
            //{
            //    hide1.Visibility = hide;
            //    hide2.Visibility = hide;
            //}
            //else
            //{
            //    if (Guy.CurrentUser.First().Access != 1)
            //    {
            //        hide1.Visibility = hide;
            //        hide2.Visibility = hide;
            //    }
            //}

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Чтение файла
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы (*.txt, *.cs) | *.txt; *.cs;";
            //Добавление текста файла в textbox для превью
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                NameFile.Text = path;
                StreamReader streamReader = new StreamReader(path);
                programText.Text = streamReader.ReadToEnd();
                SendButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Файл не выбран");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Создание компилируемого файла
            Compiler compiler = new Compiler();
            var result = compiler.Compile(programText.Text, id);
            if (result.HasErrors)
            {
                foreach (CompilerError item in result)
                {

                    MessageBox.Show("В " + item.Line + " строке, " + item.Column + " столбце, найдена ошибка " + item.ErrorNumber + " :\n " + item.ErrorText);
                }
            }
            else
            {
                MessageBox.Show("Компиляция прошла успешно");
            }
            
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0)); //Проверка на цифры
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Проверка на заполненность поля
            if (!String.IsNullOrEmpty(NumberText.Text))
            {
                AdmTestAdd.Children.Clear(); //Отчистка всех предыдущих полей
                int a = Convert.ToInt32(NumberText.Text); //Количество добавляемых тестов

                //Добавление полей для ввода входных и выходных данных для теста
                for (int i = 0; i < a; i++)
                {
                    WrapPanel panel = new WrapPanel();
                    StackPanel stackPanel = new StackPanel()
                    {
                        Margin = new Thickness()
                        {
                            Right = 20 
                        }
                    };
                    StackPanel stackPanel1 = new StackPanel();
                    stackPanel.Children.Add(new TextBlock()
                    {
                        Text = "Ввод",
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 15
                    });
                    stackPanel.Children.Add(new TextBox()
                    {
                        Height = 200,
                        Width = 200,
                        Margin = new Thickness
                        {
                            Top = 10
                        },
                        Background = Brushes.White,
                        TextWrapping = TextWrapping.Wrap,
                        AcceptsReturn = true,
                        FontSize = 15,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                    });
                    stackPanel1.Children.Add(new TextBlock()
                    {
                        Text = "Вывод",
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 15,
                        HorizontalAlignment = HorizontalAlignment.Center
                    });
                    stackPanel1.Children.Add(new TextBox()
                    {
                        Height = 200,
                        Width = 200,
                        Margin = new Thickness
                        {
                            Top = 10
                        },
                        Background = Brushes.White,
                        TextWrapping = TextWrapping.Wrap,
                        AcceptsReturn = true,
                        FontSize = 15
                    });
                    panel.Children.Add(stackPanel);
                    panel.Children.Add(stackPanel1);
                    AdmTestAdd.Children.Add(panel);

                    AdmTestAdd.Children.Add(new Line()
                    {
                        X1 = 10,
                        X2 = 800,
                        Y1 = 30,
                        Y2 = 30,
                        Stroke = Brushes.Gray,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                }

                //Проверка того, добавлены ли поля для занесения тестов
                if (AdmTestAdd.Children.Count != 0)
                {
                    TestSaveButton.IsEnabled = true;
                }
                else
                {
                    TestSaveButton.IsEnabled = false;
                }
            }
        }

        private void TestSaveButton_Click(object sender, RoutedEventArgs e)
        {
            TestTaskAPI testTask = new TestTaskAPI();
            //Основная часть занесения тестов в бд
            


            //Првоерка заполнености всех полей
            bool IsNull = false;
            for (int i = 0; i < AdmTestAdd.Children.Count; i += 2)
            {
                var Panel = (WrapPanel)AdmTestAdd.Children[i];
                var InputPanel = (StackPanel)Panel.Children[0];
                var OutputPanel = (StackPanel)Panel.Children[1];
                var InputTextBox = (TextBox)InputPanel.Children[1];
                var OutputTextBox = (TextBox)OutputPanel.Children[1];
                if(String.IsNullOrEmpty(InputTextBox.Text)|| String.IsNullOrEmpty(OutputTextBox.Text))
                {
                    IsNull = true ;
                    break;
                }
            }
            if (IsNull)
            {
                MessageBox.Show("Не все поля заполнены");
                return;
            }
            //Цикл, в котором пробегаются все тесты
            for (int i = 0; i < AdmTestAdd.Children.Count; i += 2)
            {
                var Panel = (WrapPanel)AdmTestAdd.Children[i];
                var InputPanel = (StackPanel)Panel.Children[0];
                var OutputPanel = (StackPanel)Panel.Children[1];
                var InputTextBox = (TextBox)InputPanel.Children[1];
                var OutputTextBox = (TextBox)OutputPanel.Children[1];
                byte[] StringByteInput = Encoding.UTF8.GetBytes(InputTextBox.Text);
                byte[] StringByteOutput = Encoding.UTF8.GetBytes(OutputTextBox.Text);
                string InputText = "";
                string OutputText = "";
                //Приведение byte в sting 
                foreach (var item in StringByteInput)
                {
                    InputText += item;
                }
                foreach (var item in StringByteOutput)
                {
                    OutputText += item;
                }

                //Занесение теста в базу данных
                HttpStatusCode status = testTask.PostTestByIdTask(id, InputText, OutputText);
                if (status==HttpStatusCode.OK)
                {
                    MessageBox.Show("Добавление успешно");
                }
                else
                {
                    MessageBox.Show("Добавление неудачно. Ошибка: " + status.ToString());
                }

            }
        }
    }
}
