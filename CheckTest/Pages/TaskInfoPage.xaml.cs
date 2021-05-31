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
using CheckTest.ViewModels;


namespace CheckTest.Pages
{
    /// <summary>
    /// Логика взаимодействия для TaskInfoPage.xaml
    /// </summary>
    public partial class TaskInfoPage : Page
    {
        public TaskInfoPage(int id)
        {
            InitializeComponent();

            var item = TasksAPI.GetTasksList().Where(x => x.IdTask == id).First();
            name.Text = item.NameTask;
            desc.Text = item.DescribeTask;
        }
    }
}
