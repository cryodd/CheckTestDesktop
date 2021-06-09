using CheckTest.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CheckTest.ViewModels
{
    class ProgrammingResultsAPI
    {
        //Вставка результатов
        public static bool PostResult(int id_task, string email, int result)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.PostAsync($"http://188.234.244.32:8090/api/programming_results?email={email}&result={result}&id_task={id_task}", null).Result;
                return response.IsSuccessStatusCode;
            }
        }
        //Вывод результатов
        public static List<ProgrammingResults> GetResult()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync($"http://188.234.244.32:8090/api/programming_results").Result;
                var content = response.Content.ReadAsStringAsync();
                var answer = JsonConvert.DeserializeObject<ResponseAPI<List<ProgrammingResults>>>(content.Result);
                return answer.Data;
            }
        }
        public static ProgrammingResults GetResultByIdResult(int id_result)
        {
            using (var client = new HttpClient())
            {
                var answer = GetResult().Where(x => x.id_result == id_result).First();
                return answer;
            }
        }
        //Удаление результатов
        public static bool DeleteResultByIdResult(int id_result)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.DeleteAsync($"http://188.234.244.32:8090/api/programming_results/{id_result}").Result;
                return response.IsSuccessStatusCode;
            }
        }
        public static bool UpdateResultByIdResult(int id_result,int result, string email,int id_task)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.PutAsync($"http://188.234.244.32:8090/api/programming_results/{id_result}?email={email}&id_task={id_task}&result={result}",null).Result;
                return response.IsSuccessStatusCode;
            }
        }
    }
}
