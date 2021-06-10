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
using System.Windows.Shapes;

namespace CheckTest.Pages
{
    /// <summary>
    /// Логика взаимодействия для ResultUpdateWindow.xaml
    /// </summary>
    public partial class ResultUpdateWindow : Window
    {
        int id;
        ProgrammingResults current;
        public ResultUpdateWindow(int id_result)
        {
            id = id_result;
            current = ProgrammingResultsAPI.GetResultByIdResult(id); //Текущий результат
            InitializeComponent();
            ResultBox.Text = current.result.ToString(); //Запись текущего результата
        }
        //Редактирование результата
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string result = ResultBox.Text;
            if (!String.IsNullOrWhiteSpace(result) && Convert.ToInt32(result) <= 100)
            {
                if(ProgrammingResultsAPI.UpdateResultByIdResult(id, Convert.ToInt32(result), current.email, current.id_task)) //Занесение в бд
                {
                    MessageBox.Show("Успешно");
                }
                else
                {
                    MessageBox.Show("Неуспешно");
                }
                this.Close();
            }
        }
        //Проверка на цифры
        private void ResultBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0)); 
        }
    }
}
