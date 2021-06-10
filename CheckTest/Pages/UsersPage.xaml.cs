using CheckTest.API;
using CheckTest.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CheckTest.Pages
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        TasksUsers curUser = new TasksUsers(); //Выбранный пользователь
        public UsersPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            curUser = AuthAPI.GetUser(EmailSearchText.Text).FirstOrDefault();
            if (curUser != null)
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
        //Изменение информации о пользователе
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string name = NameText.Text;
            string pass = PassText.Text;
            string email = EmailText.Text;
            //Проверка на пустые поля
            if (!String.IsNullOrWhiteSpace(name) && !String.IsNullOrWhiteSpace(pass) && !String.IsNullOrWhiteSpace(email))
            {
                //Если email не был изменен
                if (email == curUser.Email)
                {
                    //Если занесение в бд прошло успешно
                    if (AuthAPI.UpdateByEmail(email, curUser.Email, pass, name, Convert.ToInt32(curUser.Access)))
                    {
                        MessageBox.Show("Успешно");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка");
                    }
                }
                //Если email был изменен
                else
                {
                    //Если измененный email уже существует в бд
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
        //Переход на страницу с добавлением пользователей
        private void UserAdd_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new UserAddPage());
        }
    }
}
