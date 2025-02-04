﻿using CheckTest.Models;
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
    class TestTaskAPI
    {
        public static List<Tests> GetTestByIdTest(int id_test)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync($"http://188.234.244.32:8090/api/tests/{id_test}").Result;
                var content = response.Content.ReadAsStringAsync();
                var answer = JsonConvert.DeserializeObject<ResponseAPI<List<Tests>>>(content.Result);

                return answer.Data;
            }
        }
        public static List<Tests> GetTestByIdTask(int id_task)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync($"http://188.234.244.32:8090/api/tests/{id_task}").Result;
                var content = response.Content.ReadAsStringAsync();
                var answer = JsonConvert.DeserializeObject<ResponseAPI<List<Tests>>>(content.Result);
                
                return answer.Data;
            }
        }
        public HttpStatusCode PostTestByIdTask(int id_task,string input, string output)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.PostAsync($"http://188.234.244.32:8090/api/tests?test_input={input}&test_output={output}&id_task={id_task}",null).Result;
                return response.StatusCode;
                
            }
        }
        public static bool DeleteTestByIdTask(int id_task)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.DeleteAsync($"http://188.234.244.32:8090/api/tests/{id_task}").Result;
                return response.IsSuccessStatusCode;
            
            }
        }
        public static bool PutTestByIdTask(int id_test,int id_task,string test_input, string test_output)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.PutAsync($"http://188.234.244.32:8090/tests/{id_test}?test_input={test_input}&test_output={test_output}&id_task={id_task}",null).Result;
                return response.IsSuccessStatusCode;
            
            }
        }

    }
}
