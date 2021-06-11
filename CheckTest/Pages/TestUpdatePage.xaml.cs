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
    /// Логика взаимодействия для TestUpdatePage.xaml
    /// </summary>
    public partial class TestUpdatePage : Page
    {
        int id = 0;
        public TestUpdatePage(int id_task)
        {
            InitializeComponent();
            id = id_task;
            CBOX.ItemsSource = TestTaskAPI.GetTestByIdTask(id_task); //Вывод всех тестов по заданой задаче
            CBOX.DisplayMemberPath = "id_test";
            CBOX.SelectedValuePath = "id_test";
        }
        //Редактирование тестов
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Проверка на пустые поля
            if (!String.IsNullOrWhiteSpace(InputText.Text) && !String.IsNullOrWhiteSpace(OutputText.Text))
            {
                //Преобразование строки в байты
                string InputText1 = "";
                string OutputText1 = "";
                byte[] vs = Encoding.UTF8.GetBytes(InputText.Text);
                byte[] vs1 = Encoding.UTF8.GetBytes(OutputText.Text);
                foreach (var item in vs)
                {
                    InputText1 += item;
                    InputText1 += '*'; //Разделитель байтов
                }
                InputText1 += vs.Length; //В конце будте указана длина строки
                foreach (var item in vs1)
                {
                    OutputText1 += item;
                    OutputText1 += '*';
                }
                OutputText1 += vs1.Length; 
                //Если изменение в бд успешно
                if(TestTaskAPI.PutTestByIdTask(Convert.ToInt32(CBOX.SelectedValue), id, InputText1, OutputText1))
                {
                    MessageBox.Show("Успешно");
                    this.NavigationService.Navigate(new TaskPage());
                }
                else
                {
                    MessageBox.Show("Ошибка");

                }
            }
            else
            {
                MessageBox.Show("Не все поля заполнены");
            }
        }
        //Вывод информации по выбранному тесту
        private void CBOX_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = (ComboBox)sender;
            var cur = TestTaskAPI.GetTestByIdTask(id).Where(x => x.id_test == Convert.ToInt32(CBOX.SelectedValue)); //Все тесты по текущей задаче
            InputText.Text = Encoding.UTF8.GetString(TestTask.StringToByte(cur.FirstOrDefault().test_input)); //Вывод входных данных
            OutputText.Text = Encoding.UTF8.GetString(TestTask.StringToByte(cur.FirstOrDefault().test_output)); //Вывод выходных данных
            ButtonSave.IsEnabled = true;
        }
    }
}
