using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using CheckTest.Models;

namespace CheckTest.ViewModels
{
    public class TasksAPI
    {
        //Вывод всех заданий
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
        //Добавление заданий
        public static bool PostTask(string nametask, string desctask)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.PostAsync($"http://188.234.244.32:8090/api/tasks?name_task={nametask}&describe_task={desctask}", null).Result;
                return response.IsSuccessStatusCode;
            }
        }
        //Удаление заданий
        public static bool DeleteTaskByIdTask(int id_task)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.DeleteAsync($"http://188.234.244.32:8090/api/tasks/{id_task}").Result;
                return response.IsSuccessStatusCode;

            }
        }
        //Редактирование заданий
        public static bool UpdateTaskByIdTask(int id_task,string name, string desc)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.PutAsync($"http://188.234.244.32:8090/api/tasks/{id_task}?name_task={name}&describe_task={desc}",null).Result;
                return response.IsSuccessStatusCode;

            }
        }
    }
}
