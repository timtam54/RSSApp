using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public class EquipTypeTestFailServices : IEquipTypeTestFailRepository
    {
        public async Task<EquipTypeTestFail> AddNew(EquipTypeTestFail equipTypeTestFail)
        {
            var client = new HttpClient();
            string bod = Newtonsoft.Json.JsonConvert.SerializeObject(equipTypeTestFail);
            var stringContent = new StringContent(bod, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.PostAsync("", stringContent);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<EquipTypeTestFail>(content);
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

        string url = "https://roofsafetysolutions.azurewebsites.net/api/EquipTypeTestFails/";

        public async Task<List<EquipTypeTestFail>> EquipTypeTestFails(int EquipTypeTestID)
        {
            var clients = new List<EquipTypeTestFail>();
            var client = new HttpClient();

            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync(EquipTypeTestID.ToString());
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                clients = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EquipTypeTestFail>>(content);
                return await Task.FromResult(clients.ToList());
            }
            return null;
        }

        public async Task<EquipTypeTestFail> EquipTypeTestFail(int EquipTypeTestFailID)

        {
            //  var clients = new List<Models.EquipTypeTest>();
            var client = new HttpClient();

            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("int/" + EquipTypeTestFailID.ToString());
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                Models.EquipTypeTestFail clients = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.EquipTypeTestFail>(content);
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

     

     


        public Task<bool> Update(EquipTypeTestFail equipTypeTestFail)
        {
            throw new NotImplementedException();
        }
    }
}

