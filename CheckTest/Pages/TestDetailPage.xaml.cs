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
        int id = 0;
        public TestDetailPage(int id_result)
        {
            InitializeComponent();
            id = id_result;
            TaskIdBox.ItemsSource = TestTaskAPI.GetTestByIdTest(id_result);
            TaskIdBox.DisplayMemberPath = "id_test";
            TaskIdBox.SelectedValuePath = "id_test";
        }

        private void TaskIdBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = (ComboBox)sender;
            Console.WriteLine(a.SelectedValue.ToString());
            var cur = TestTaskAPI.GetTestByIdTask(id);
            var cut = cur.Where(x => x.id_test == Convert.ToInt32(TaskIdBox.SelectedValue));
            var k = TestDetailsAPI.GetDetails();
            var kk = k.Where(x => x.id_test == Convert.ToInt32(a.SelectedValue));
            var c = kk.Where(x => x.id_result == id);
            var kkk = c.First().user_output;
            programText1.Text = Encoding.UTF8.GetString(TestTask.StringToByte(kkk));
            programText2.Text = Encoding.UTF8.GetString(TestTask.StringToByte(cur.FirstOrDefault().test_input));
            programText3.Text = Encoding.UTF8.GetString(TestTask.StringToByte(cur.FirstOrDefault().test_output));
        }
    }
}
