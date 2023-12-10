using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public class EquipTypeTestServices : IEquipTypeTestRepository
    {
        public async Task<EquipTypeTest> AddNew(EquipTypeTest equipTypeTest)
        {
            var client = new HttpClient();
            string bod = Newtonsoft.Json.JsonConvert.SerializeObject(equipTypeTest);
            var stringContent = new StringContent(bod, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.PostAsync("", stringContent);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<EquipTypeTest>(content);
                return await Task.FromResult(ret);
            }
            return null;
        }

        public async Task<bool> Delete(int id)
        {
            var client = new HttpClient();
           // string bod = Newtonsoft.Json.JsonConvert.SerializeObject(equipTypeTest);
            //var stringContent = new StringContent(bod, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.DeleteAsync(id.ToString());
            if (response.IsSuccessStatusCode)
            {
               // string content = response.Content.ReadAsStringAsync().Result;
             //   var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<EquipTypeTest>(content);
                return true;
            }
            return false;
        }

        string url = "https://roofsafetysolutions.azurewebsites.net/api/EquipTypeTests/";

        public async Task<List<EquipTypeTest>> EquipTypeTests(int EquipTypeID)
        {
            var clients = new List<EquipTypeTest>();
            var client = new HttpClient();

            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync(EquipTypeID.ToString());
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                clients = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EquipTypeTest>>(content);
                return await Task.FromResult(clients.ToList());
            }
            return null;
        }

        public async Task<EquipTypeTest> EquipTypeTest(int id)
        {
          //  var clients = new List<Models.EquipTypeTest>();
            var client = new HttpClient();

            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("int/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                Models.EquipTypeTest clients = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.EquipTypeTest>(content);
                return await Task.FromResult(clients);
            }
            return null;
        }

        public async Task<bool> Update(EquipTypeTest equipTypeTest)
        {
            var client = new HttpClient();
            string bod = Newtonsoft.Json.JsonConvert.SerializeObject(equipTypeTest);
            var stringContent = new StringContent(bod, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.PutAsync("", stringContent);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                //var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<EquipTypeTest>(content);
                return true;// await Task.FromResult(ret);
            }
            return false;
        }
    }
}

