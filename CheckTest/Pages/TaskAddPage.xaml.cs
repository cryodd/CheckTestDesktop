using CheckTest.ViewModels;
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
    /// Логика взаимодействия для TaskAddPage.xaml
    /// </summary>
    public partial class TaskAddPage : Page
    {
        public TaskAddPage()
        {
            InitializeComponent();
        }
        //Добавление заданий
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text;
            string desc = DescBox.Text;
            if (!String.IsNullOrWhiteSpace(name) && !String.IsNullOrWhiteSpace(desc)) //Добавление в бд
            {
                if (TasksAPI.PostTask(name, desc))
                {
                    MessageBox.Show("Добавление успешно");
                    this.NavigationService.Navigate(new Pages.TaskPage()); //Переход обратно на страницу со всеми заданиями
                }
            }
            else
            {
                ErrorText.Text = "Не все поля заполнены"; //Ошибка
            }
        }
    }
}
