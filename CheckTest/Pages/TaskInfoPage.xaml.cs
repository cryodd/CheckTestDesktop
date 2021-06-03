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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CheckTest.ViewModels;
using Microsoft.Win32;
using System.IO;


namespace CheckTest.Pages
{
    /// <summary>
    /// Логика взаимодействия для TaskInfoPage.xaml
    /// </summary>
    public partial class TaskInfoPage : Page
    {
        int id;
        public TaskInfoPage(int id)
        {
            InitializeComponent();
            this.id = id;
            var item = TasksAPI.GetTasksList().Where(x => x.IdTask == id).First();
            name.Text = item.NameTask;
            desc.Text = item.DescribeTask;
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
                int a = Convert.ToInt32(NumberText.Text);
                for (int i = 0; i < a; i++)
                {
                    AdmTestAdd.Children.Add(new TextBlock()
                    {
                        Text = "Ввод",
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 15
                    });
                    AdmTestAdd.Children.Add(new TextBox()
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
                    AdmTestAdd.Children.Add(new TextBlock()
                    {
                        Text = "Вывод",
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 15,
                        HorizontalAlignment = HorizontalAlignment.Center
                    });
                    AdmTestAdd.Children.Add(new TextBox()
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
    }
}
