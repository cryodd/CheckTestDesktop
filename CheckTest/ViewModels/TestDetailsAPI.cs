using CheckTest.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace CheckTest.ViewModels
{
    class TestDetailsAPI
    {
        public static IEnumerable<TestDetails> GetDetails()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync($"http://188.234.244.32:8090/api/test_details").Result;
                var content = response.Content.ReadAsStringAsync();
                var answer = JsonConvert.DeserializeObject<ResponseAPI<List<TestDetails>>>(content.Result);
                return answer.Data;
            }
        }
        public static bool PostDetails(int id_test, string user_output, int success, int id_result)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.PostAsync($"http://188.234.244.32:8090/api/test_details?id_test={id_test}&user_output={user_output}&success={success}&id_result={id_result}", null).Result;
                return response.IsSuccessStatusCode;
            }
        }
        public static bool DeleteDetailsByIdTask(int id_task)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = new HttpResponseMessage();
                foreach(var item in TestTaskAPI.GetTestByIdTask(id_task))
                {
                    foreach(var otem in GetDetails().Where(x =>x.id_test == item.id_test))
                    {
                        response = client.DeleteAsync($"http://188.234.244.32:8090/api/test_details/{otem.id_detail}").Result;
                    }
                }
                return response.IsSuccessStatusCode;
            }
        }
    }
}
