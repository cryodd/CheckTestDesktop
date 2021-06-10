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
    /// Логика взаимодействия для UserAddPage.xaml
    /// </summary>
    public partial class UserAddPage : Page
    {
        public UserAddPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AuthAPI obj = new AuthAPI();
            string email = LoginText.Text;
            string pass = PassText.Text;
            string pass2 = PassText1.Text;
            string name = NameText.Text;
            //Проверка на пустые поля
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(name))
            {
                ErrorText.Text = "Не все поля введены";
            }
            else
            {
                //Проверка на совпадение паролей
                if (pass != pass2 || pass2 != pass)
                {
                    ErrorText.Text = "Пароли не совпадают";
                }
                else
                {
                    //Регистрация
                    if (obj.PostReg(email, pass, name))
                    {
                        MessageBox.Show("Зарегистрированно");
                        this.NavigationService.Navigate(new TaskPage()); //Переход на страницу со всеми задачами
                    }
                    else
                    {
                        MessageBox.Show("Ошибка");
                    }
                }
            }

        }
    }
}
