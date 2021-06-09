using CheckTest.API;
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

namespace CheckTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Pages.AuthPage()); //Перенаправление на страницу с авторизацией
            
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            if (Guy.CurrentUser != null)
            {
                HelloName.Text = $"Здраствуйте, {Guy.CurrentUser.First().Name}";
                HelloName.Visibility = Visibility;
                LogoutButton.Visibility = Visibility;
                //Кнопка для добавления заданий видна только для администраторов
                if (Guy.CurrentUser.First().Access == 1)
                {
                    AddTaskButton.Visibility = Visibility;
                }
                else
                {
                    AddTaskButton.Visibility = Visibility.Hidden;
                }
            }
            if (MainFrame.CanGoBack && Guy.CurrentUser!=null)
            {
                GoBackButton.Visibility = Visibility;
            }
            else
            {
                GoBackButton.Visibility = Visibility.Hidden;
            }
        }
        //Кнопка для выхода из учетной записи
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Guy.CurrentUser = null;
            HelloName.Visibility = Visibility.Hidden;
            GoBackButton.Visibility = Visibility.Hidden;
            LogoutButton.Visibility = Visibility.Hidden;
            AddTaskButton.Visibility = Visibility.Hidden;
            MainFrame.Navigate(new Pages.AuthPage());
        }
        //Кнопка назад
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.GoBack();
        }
        //Переход на страницу с добавлением тестов
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Pages.TaskAddPage());
        }
    }
}
