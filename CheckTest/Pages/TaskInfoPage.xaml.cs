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
using CheckTest.Models;

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
            if (CurUser != null)
            {
                IEnumerable<ProgrammingResults> ResultList = ProgrammingResultsAPI.GetResult().Where(x => x.id_task == id); //Для админа выводятся все результаты, для пользователя, только его
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
                    panel.Children.Add(new TextBlock
                    {
                        Text = IdRes++ + ". Результат" + name + ": " + result.result + "%",
                        FontSize = 28
                    });
                    if (CurUser.First().Access == 1)
                    {
                        //Кнопка удаления результата, только для админов
                        var but = new Button
                        {
                            Content = "Удалить",
                            FontSize = 28,
                            Background = Brushes.Red,
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
                        but.Click += But_Click;
                        butRed.Click += ButRed_Click;
                        panel.Children.Add(butRed);
                        panel.Children.Add(but);
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
        //Редактирование результата
        private void ButRed_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            ResultUpdateWindow win = new ResultUpdateWindow((Convert.ToInt32(but.Uid))); //Открытие окна с редактированием
            win.Closed += Win_Closed;
            win.Show();
        }
        //При закрытии окна с редактированием, обновляется страница
        private void Win_Closed(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new TaskInfoPage(id));
        }

        //Удаление результата
        private void But_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            ProgrammingResultsAPI.DeleteResultByIdResult(Convert.ToInt32(but.Uid));
            this.NavigationService.Navigate(new TaskInfoPage(id));
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
            var result = compiler.Compile(programText.Text);
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
                List<int> grade = compiler.Test(id); //Тестирование
                if (grade!=null)
                {
                    int gr =Convert.ToInt32( Math.Round(Average(grade) * 100)); //Средний балл
                    MessageBox.Show("Все тесты завершены, ваш балл = " + gr);
                    if (Guy.CurrentUser != null) //Если вдруг сюда попал неавторизированный пользователь
                    {
                        if (ProgrammingResultsAPI.PostResult(id, Guy.CurrentUser.First().Email, gr))
                        {
                            MessageBox.Show("Результаты занесены");
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
        //Среднее значение
        private decimal Average(List<int> list)
        {
            int summ = 0;
            foreach(int num in list)
            {
                summ += num;
            }
            return summ / (decimal)list.Count;
        }
    }
}
