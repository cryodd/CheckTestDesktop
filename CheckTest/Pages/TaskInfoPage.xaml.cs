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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы (*.txt, *.cs) | *.txt; *.cs;";
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                NameFile.Text = path;
                StreamReader streamReader = new StreamReader(path);
                programText.Text = streamReader.ReadToEnd();
                SendButton.IsEnabled = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Compiler compiler = new Compiler();
            foreach(var item in compiler.Compile(programText.Text,id))
            {
                Console.WriteLine(item);
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(NumberText.Text))
            {
                Console.WriteLine(" ");
            }
            else
            {
                AdmTestAdd.Children.Clear();
                TestSaveButton.IsEnabled = true;
                int a = Convert.ToInt32(NumberText.Text);
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
            }
        }

        private void TestSaveButton_Click(object sender, RoutedEventArgs e)
        {
            TestTaskAPI testTask = new TestTaskAPI();

            for (int i = 0; i < AdmTestAdd.Children.Count; i += 2)
            {
                var Panel = (WrapPanel)AdmTestAdd.Children[i];
                var InputPanel = (StackPanel)Panel.Children[0];
                var OutputPanel = (StackPanel)Panel.Children[1];
                var InputTextBox = (TextBox)InputPanel.Children[1];
                var OutputTextBox = (TextBox)OutputPanel.Children[1];
                byte[] vs = Encoding.UTF8.GetBytes(InputTextBox.Text);
                byte[] vs1 = Encoding.UTF8.GetBytes(OutputTextBox.Text);
                
                    Console.WriteLine(testTask.PostTestByIdTask(1, vs, vs1).ToString());
                
            }
        }
    }
}
