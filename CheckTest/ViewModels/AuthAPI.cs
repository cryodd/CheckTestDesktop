using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using TestCheck.Models;

namespace TestCheck.API
{
    public class AuthAPI
    {
        public bool GetAuth(string login,string password)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync($"http://188.234.244.32:8090/api/tasks_users?email={login}&password={password}").Result;
                var content = response.Content.ReadAsStringAsync();
                var answer = JsonConvert.DeserializeObject<ResponseAPI<List<TasksUsers>>>(content.Result);
                if (answer.Success)
                {
                    Guy.CurrentUser = answer.Data;
                }
                else
                {
                    Guy.CurrentUser = null;
                }
                return answer.Success;
            }
        }
        public bool PostReg(string email,string pass,string name)
        {
            int access = 0;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.PostAsync($"http://188.234.244.32:8090/api/tasks_users?email={email}&password={pass}&name={name}&access={access}",null).Result;
                return response.IsSuccessStatusCode;
            }
        }
    }
}
