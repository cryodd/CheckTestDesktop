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

namespace CheckTest.Pages
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var curUser = AuthAPI.GetUser(EmailSearchText.Text).FirstOrDefault();
            if ( curUser != null)
            {
                
            }
            else
            {
                MessageBox.Show("Пользователя не существует");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void UserAdd_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new UserAddPage());

        }
    }
}
