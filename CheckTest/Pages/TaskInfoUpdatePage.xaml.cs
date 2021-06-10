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
    public partial class TaskInfoUpdatePage : Page
    {
        int id; //Номер задания
        public TaskInfoUpdatePage(int id_task)
        {
            InitializeComponent();
            id = id_task;
            var current = TasksAPI.GetTasksList().Where(x => x.IdTask == id).First();
            NameBox.Text = current.NameTask;
            DescBox.Text = current.DescribeTask;
        }
        //Редактирование заданий
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text;
            string desc = DescBox.Text;
            //Проверка на пустые поля
            if (!String.IsNullOrWhiteSpace(name) && !String.IsNullOrWhiteSpace(desc))
            {
                if (TasksAPI.UpdateTaskByIdTask(id,name, desc))
                {
                    MessageBox.Show("Редактирование успешно");
                    this.NavigationService.Navigate(new Pages.TaskPage()); //Переход обратно на страницу со всеми заданиями
                }
            }
            else
            {
                ErrorText.Text = "Не все поля заполнены"; //Ошибка
            }
        }
        //Переход на старницу с редактированием тестов
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new TestUpdatePage(id));
        }
    }
}
