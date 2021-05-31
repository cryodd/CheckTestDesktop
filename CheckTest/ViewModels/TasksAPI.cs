using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using TestCheck.Models;

namespace TestCheck.ViewModels
{
    public class TasksAPI
    {
        public static List<Tasks> GetTasksList()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync($"http://188.234.244.32:8090/api/tasks").Result;
                var content = response.Content.ReadAsStringAsync();
                var answer = JsonConvert.DeserializeObject<ResponseAPI<List<Tasks>>>(content.Result);
                return answer.Data;
            }
        }
        public bool PostTask(string nametask, string desctask)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.PostAsync($"http://188.234.244.32:8090/api/tasks?name_task={nametask}&describe_task={desctask}", null).Result;
                return response.IsSuccessStatusCode;
            }
        }
    }
}
