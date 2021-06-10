using CheckTest.API;
using CheckTest.Models;
using CheckTest.ViewModels;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CheckTest.Pages
{
    /// <summary>
    /// Логика взаимодействия для TaskInfoPage.xaml
    /// </summary>
    public partial class TaskInfoPage : Page
    {
        int id; //Номер задания
        Visibility hide = Visibility.Hidden;
        public TaskInfoPage(int id)
        {
            InitializeComponent();
            this.id = id;
            var CurUser = Guy.CurrentUser; //Текущий пользователь
            var item = TasksAPI.GetTasksList().Where(x => x.IdTask == id).First(); //Текущее задание
            name.Text = item.NameTask;
            desc.Text = item.DescribeTask;
            //Вывод результатов пользователя по текущему заданию
            if (CurUser != null) //Проверка на то, вошел ли пользователь
            {
                IEnumerable<ProgrammingResults> ResultList = ProgrammingResultsAPI.GetResult().Where(x => x.id_task == id);
                //Для админа выводятся все результаты, для пользователя, только его
                if (CurUser.First().Access == 0)
                {
                    ResultList = ResultList.Where(x => x.email == CurUser.First().Email);
                }
                int IdRes = 1;
                foreach (var result in ResultList)
                {
                    string name = "";
                    WrapPanel panel = new WrapPanel();
                    if (CurUser.First().Access == 1)
                    {
                        name = "от пользователя с email`ом " + result.email;
                    }
                    //Текст с результатом
                    panel.Children.Add(new TextBlock
                    {
                        Text = IdRes++ + ". Результат " + name + ": " + result.result + "%",
                        FontSize = 28
                    });
                    //Если пользователь администратор
                    if (CurUser.First().Access == 1)
                    {
                        //Кнопка просмотра высланной работы сравниваемая с эталоном
                        var butPros = new Button
                        {
                            Content = "Просмотр",
                            FontSize = 28,
                            Uid = result.id_result.ToString()
                        };
                        //Кнопка редактирования результата
                        var butRed = new Button
                        {
                            Content = "Редактировать",
                            FontSize = 28,
                            Background = Brushes.Teal,
                            Uid = result.id_result.ToString()
                        };
                        butRed.Click += ButRed_Click;
                        butPros.Click += ButPros_Click;
                        panel.Children.Add(butPros);
                        panel.Children.Add(butRed);
                    }
                    ResultPreview.Children.Add(panel);
                }

            }
            //Скрытие добавление тестов для обычных пользователей и для неавторизированных пользователей(если вдруг такие смогут попасть на данную страницу)
            if (CurUser == null)
            {
                hide1.Visibility = hide;
                hide2.Visibility = hide;
            }
            else
            {
                if (Guy.CurrentUser.First().Access != 1)
                {
                    hide1.Visibility = hide;
                    hide2.Visibility = hide;
                }
            }

        }
        //Просмотр результата
        private void ButPros_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            this.NavigationService.Navigate(new TestDetailPage(Convert.ToInt32(but.Uid)));
        }

        //Редактирование результата
        private void ButRed_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            ResultUpdateWindow win = new ResultUpdateWindow((Convert.ToInt32(but.Uid))); //Открытие окна с редактированием
            win.Closed += Win_Closed;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }
        //При закрытии окна с редактированием, обновляется страница
        private void Win_Closed(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new TaskInfoPage(id));
        }
        //Считывание текста из файла
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
        //Тестирование файла
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Создание компилируемого файла

            Compiler compiler = new Compiler();
            var result = compiler.Compile(programText.Text); //Компиляция программы
            //Если есть ошибки
            if (result.HasErrors)
            {
                foreach (CompilerError item in result)
                {

                    MessageBox.Show("В " + item.Line + " строке, " + item.Column + " столбце, найдена ошибка " + item.ErrorNumber + " :\n " + item.ErrorText); //Вывод описания ошибки
                }
            }
            else
            {
                MessageBox.Show("Компиляция прошла успешно");
                List<Details> det = compiler.Test(id);  //Тестирование
                List<int> grade = new List<int>(); //Список со всеми баллами за тесты
                //Если тестирование не выявило ошибок
                if (det != null)
                {
                    //Добавление баллов в список
                    foreach (var item in det)
                    {
                        grade.Add(item.sucsess);
                    }
                    int gr = Convert.ToInt32(Math.Round(Average(grade) * 100)); //Средний балл
                    MessageBox.Show("Все тесты завершены, ваш балл = " + gr);
                    //Если Пользователь авторизован
                    if (Guy.CurrentUser != null) 
                    {
                        //Если занесение в БД информации о результате прошло успешно
                        if (ProgrammingResultsAPI.PostResult(id, Guy.CurrentUser.First().Email, gr))
                        {
                            bool suc = true;
                            var last = ProgrammingResultsAPI.GetResult().Last();
                            //Занесение детальной информации в бд
                            foreach (var item in det)
                            {
                                if (TestDetailsAPI.PostDetails(item.id_test, item.user_output, item.sucsess, last.id_result))
                                {
                                    

                                }
                                else
                                {
                                    suc = false;
                                    MessageBox.Show("Ошибка при занесении результатов");
                                    break;
                                }

                            }
                            if (suc)
                            {
                                MessageBox.Show("Результаты занесены");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при занесении результатов");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Вы не вошли");
                    }
                }
                else
                {
                    MessageBox.Show("Не все тесты завершены, обнаружена ошибка");
                }
            }


        }
        //Проверка на цифры
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0)); 
        }
        //Добавление полей для создания тестов
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
                    // "Ввод"
                    stackPanel.Children.Add(new TextBlock()
                    {
                        Text = "Ввод",
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 15
                    });
                    //Поле для ввода входных значений
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
                    // "Вывод"
                    stackPanel1.Children.Add(new TextBlock()
                    {
                        Text = "Вывод",
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 15,
                        HorizontalAlignment = HorizontalAlignment.Center
                    });
                    //Поле для выходных входных значений
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
                    //Линия, разделяющая тесты
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
        //Занесение тестов в бд
        private void TestSaveButton_Click(object sender, RoutedEventArgs e)
        {
            TestTaskAPI testTask = new TestTaskAPI();
            //Првоерка заполнености всех полей
            bool IsNull = false;
            //Цикл, которвый пробегает через все поля с занесенем тестов
            for (int i = 0; i < AdmTestAdd.Children.Count; i += 2)
            {
                var Panel = (WrapPanel)AdmTestAdd.Children[i];
                var InputPanel = (StackPanel)Panel.Children[0];
                var OutputPanel = (StackPanel)Panel.Children[1];
                var InputTextBox = (TextBox)InputPanel.Children[1]; // Поле со входными данными 
                var OutputTextBox = (TextBox)OutputPanel.Children[1]; // Поле о выходными данными 
                //Если хотя бы одно поле не будет содержать значений, выводит ошибку
                if (String.IsNullOrEmpty(InputTextBox.Text) || String.IsNullOrEmpty(OutputTextBox.Text))
                {
                    IsNull = true;
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
                var InputTextBox = (TextBox)InputPanel.Children[1];// Поле со входными данными 
                var OutputTextBox = (TextBox)OutputPanel.Children[1];// Поле о выходными данными 
                byte[] StringByteInput = Encoding.UTF8.GetBytes(InputTextBox.Text); //Байтовое представление строки
                byte[] StringByteOutput = Encoding.UTF8.GetBytes(OutputTextBox.Text);//Байтовое представление строки
                string InputText = ""; //Строчное представление байтов
                string OutputText = ""; //Строчное представление байтов
                //Приведение byte в sting 
                foreach (var item in StringByteInput)
                {
                    InputText += item;
                    InputText += '*'; //Разделитель байтов
                }
                InputText += StringByteInput.Length; //В конце будте указана длина строки
                foreach (var item in StringByteOutput)
                {
                    OutputText += item;
                    OutputText += '*'; //Разделитель байтов
                }
                OutputText += StringByteOutput.Length; //В конце будте указана длина строки

                //Занесение теста в базу данных
                HttpStatusCode status = testTask.PostTestByIdTask(id, InputText, OutputText);
                if (status == HttpStatusCode.OK)
                {
                    MessageBox.Show("Добавление успешно");
                }
                else
                {
                    MessageBox.Show("Добавление неудачно. Ошибка: " + status.ToString());
                }

            }
        }
        //Среднее значение
        private decimal Average(List<int> list)
        {
            int summ = 0;
            foreach (int num in list)
            {
                summ += num;
            }
            return summ / (decimal)list.Count;
        }
    }
}
