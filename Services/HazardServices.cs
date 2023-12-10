using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public class HazardServices : IHazardRepository
    {

        public HazardServices()
        {
        }


        public async Task<Hazard> AddNew(Hazard hazard)
        {

            var client = new HttpClient();
            string bod = Newtonsoft.Json.JsonConvert.SerializeObject(hazard);
            var stringContent = new StringContent(bod, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.PostAsync("", stringContent);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<Hazard>(content);
                return await Task.FromResult(ret);
            }
            return null;
        }
        string url = "https://roofsafetysolutions.azurewebsites.net/api/Hazards";

        public async  Task<List<Hazard>> Hazards()
        {
            var clients = new List<Hazard>();
            var client = new HttpClient();
             client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                clients = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Hazard>>(content);
                return await Task.FromResult(clients.ToList());
            }
            return null;
        }

        
        
    }
}