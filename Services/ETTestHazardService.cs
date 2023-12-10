using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public class ETTestHazardService : IETTestHazardRepository
    {

        public ETTestHazardService()
        {
        }

        public async Task<EquipTypeTestHazards> AddNew(EquipTypeTestHazards inspEquipTypeTest)
        {
            var client = new HttpClient();
            string bod = Newtonsoft.Json.JsonConvert.SerializeObject(inspEquipTypeTest);
            var stringContent = new StringContent(bod, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.PostAsync("", stringContent);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<EquipTypeTestHazards>(content);
                return await Task.FromResult(ret);
            }
            return null;
        }

        public async Task<bool> Delete(int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.DeleteAsync(id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<InspEquipTypeTest>(content);
                return true;
            }
            return false;
        }

        public async Task<bool> Update(EquipTypeTestHazards inspEquipTypeTest)
        {
            string bod = Newtonsoft.Json.JsonConvert.SerializeObject(inspEquipTypeTest);
            var stringContent = new StringContent(bod, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.PutAsync(inspEquipTypeTest.id.ToString(), stringContent);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                // ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InspEquipTypeTest>>(content);
                return true;// await Task.FromResult(ret.ToList());
            }
            return false;
        }

        public async Task<List<EquipTypeTestHazards>> InspETTestHazards(int Inspequipid)

        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync(Inspequipid.ToString());
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EquipTypeTestHazards>>(content);
                return await Task.FromResult(ret.ToList());
            }
            return null;
        }

        string url = "https://roofsafetysolutions.azurewebsites.net/api/EquipTypeTestHazards/";

        public async Task<EquipTypeTestHazards> InspETTestHazard(int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("int/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<EquipTypeTestHazards>(content);
                return await Task.FromResult(ret);
            }
            return null;
        }

      


    }
}
