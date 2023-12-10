using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public class ClientServices : IClientRepository
    {

        public async Task<List<Client>> Clients(string search)
        {
            try
            {
                List<Client> inspections;
                var client = new HttpClient();
                string url = "https://roofsafetysolutions.azurewebsites.net/api/Clients";
                client.BaseAddress = new Uri(url + "/" + search.ToString());
//                client.BaseAddress = new Uri(url);
                HttpResponseMessage response = await client.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    inspections = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Client>>(content);
                    return await Task.FromResult(inspections);
                }
            }
            catch (Exception ex)
            {
                var dd = ex.Message;
                var mm = dd;
            }
            return null;
        }
    }
}
