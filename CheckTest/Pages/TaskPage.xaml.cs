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

namespace CheckTest.Pages
{
    /// <summary>
    /// Логика взаимодействия для TaskPage.xaml
    /// </summary>
    public partial class TaskPage : Page
    {
        public TaskPage()
        {
            InitializeComponent();
            foreach(var item in TasksAPI.GetTasksList())
            {

                FGG.Children.Add(new TextBlock()
                {
                    Text = "№ "+ item.IdTask + ". " + item.NameTask,
                    FontSize = 28,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness
                    {
                        Top = 20,
                        Bottom = 20
                    }
                });
                FGG.Children.Add(new TextBlock()
                {
                    Text = item.DescribeTask,
                    FontSize = 28,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness
                    {
                        Bottom = 20
                    }
                });
                var but = new Button()
                {
                    Content = "Перейти к заданию",
                    Uid = item.IdTask.ToString()
                };
                but.Click += but_click;
                FGG.Children.Add(but);
                FGG.Children.Add(new Line()
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
            void but_click(object sender, RoutedEventArgs e)
            {
                var but = (Button)sender;
                this.NavigationService.Navigate(new TaskInfoPage(Convert.ToInt32(but.Uid)));
            }
        }
    }
}
