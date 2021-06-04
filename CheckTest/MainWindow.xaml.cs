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
            MainFrame.Navigate(new Pages.TaskInfoPage(1));
            
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            if (Guy.CurrentUser != null)
            {
                HelloName.Text = $"Здраствуйте, {Guy.CurrentUser.First().Name}";
                HelloName.Visibility = Visibility;
                LogoutButton.Visibility = Visibility;
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

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Guy.CurrentUser = null;
            HelloName.Visibility = Visibility.Hidden;
            GoBackButton.Visibility = Visibility.Hidden;
            LogoutButton.Visibility = Visibility.Hidden;
            MainFrame.Navigate(new Pages.AuthPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.GoBack();
        }
    }
}
