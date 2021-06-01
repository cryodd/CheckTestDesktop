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
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string login = EmailText.Text;
            string password = PassText.Password;
            AuthAPI obj = new AuthAPI();
            if (obj.GetAuth(login, password))
            {
                this.NavigationService.Navigate(new TaskPage());
                ErrorText.Text = "";
            }
            else
            {
                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                {
                    ErrorText.Text = "Не введен логин или пароль";
                }
                else
                {
                    ErrorText.Text = "Не верно введен логин или пароль";
                }
            }
        }
    }
}
