using CheckTest.Models;
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
    /// Логика взаимодействия для TestDetailPage.xaml
    /// </summary>
    public partial class TestDetailPage : Page
    {
        int id = 0; //Номер результата
        int id_task; //Номер задачи
        public TestDetailPage(int id_result)
        {
            InitializeComponent();
            id = id_result;
            id_task = ProgrammingResultsAPI.GetResultByIdResult(id).id_task;
            TaskIdBox.ItemsSource = TestTaskAPI.GetTestByIdTest(id_task); //Вывод всех тестов по данной задаче
            TaskIdBox.DisplayMemberPath = "id_test";
            TaskIdBox.SelectedValuePath = "id_test";
        }
        //Вывод информации при изменении номера теста
        private void TaskIdBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = (ComboBox)sender;
            var cur = TestTaskAPI.GetTestByIdTask(id_task);
            var cut = cur.Where(x => x.id_test == Convert.ToInt32(TaskIdBox.SelectedValue)); 
            var k = TestDetailsAPI.GetDetails();
            var kk = k.Where(x => x.id_test == Convert.ToInt32(a.SelectedValue) && x.id_result == id); //Все детали по текущей задаче и результату
            var kkk = kk.First().user_output;
            programText1.Text = Encoding.UTF8.GetString(TestTask.StringToByte(kkk)); //Выходные значения пользователя
            programText2.Text = Encoding.UTF8.GetString(TestTask.StringToByte(cut.FirstOrDefault().test_input)); //Входные значения
            programText3.Text = Encoding.UTF8.GetString(TestTask.StringToByte(cut.FirstOrDefault().test_output));//Выходные значения
        }
    }
}
