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
                    HorizontalAlignment = HorizontalAlignment.Center
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
                    Width = 150,
                    Height = 50,
                    Uid = item.IdTask.ToString()
                };
                but.Click += but_click;
                FGG.Children.Add(but);
            }
            void but_click(object sender, RoutedEventArgs e)
            {
                var but = (Button)sender;
                this.NavigationService.Navigate(new TaskInfoPage(Convert.ToInt32(but.Uid)));
            }
        }
    }
}
