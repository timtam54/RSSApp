using System;
using RssMob.Models;

namespace RssMob.Services
{
    public class LoginServices : ILoginRepository
    {

        public LoginServices()
        {
        }

        public async Task<Employee> Login(string email, string password)
        {
            var employee = new List<Employee>();
            var client = new HttpClient();
            string url = "https://roofsafetysolutions.azurewebsites.net/api/employees/" + email + "~" + password;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                employee = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Employee>>(content);
                return await Task.FromResult(employee.FirstOrDefault());
            }
            return null;
        }
    }
}

