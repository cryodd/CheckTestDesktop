using CheckTest.API;
using CheckTest.Models;
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
        TasksUsers curUser = new TasksUsers();
        public UsersPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            curUser = AuthAPI.GetUser(EmailSearchText.Text).FirstOrDefault();
            if ( curUser != null)
            {
                EmailText.Text = curUser.Email;
                PassText.Text = curUser.Password;
                NameText.Text = curUser.Name;
                RedBut.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Пользователя не существует");
                RedBut.IsEnabled = false;

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string name = NameText.Text;
            string pass = PassText.Text;
            string email = EmailText.Text;
            if (!String.IsNullOrWhiteSpace(name)&& !String.IsNullOrWhiteSpace(pass) && !String.IsNullOrWhiteSpace(email))
            {
                if (email == curUser.Email)
                {
                    if (AuthAPI.UpdateByEmail(email, curUser.Email, pass, name, Convert.ToInt32(curUser.Access)))
                    {
                        MessageBox.Show("Успешно");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка");
                    }
                }

                else {
                    if ((AuthAPI.GetUser(email).FirstOrDefault()) == null)
                    {
                        if (AuthAPI.UpdateByEmail(email, curUser.Email, pass, name, Convert.ToInt32(curUser.Access)))
                        {
                            MessageBox.Show("Успешно");
                        }
                        else
                        {
                            MessageBox.Show("Ошибка");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пользователь с таким email уже существует");

                    }
                }
                    
                
                
            }
            else
            {
                MessageBox.Show("Не все поля заполнены");
            }
        }

        private void UserAdd_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new UserAddPage());

        }
    }
}
