using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public class InspEquipTestFailService : IInspEquipTestFailRepository
    {

        public InspEquipTestFailService()
        {
        }

        public async Task<InspEquipTypeTestFail> AddNew(InspEquipTypeTestFail inspEquipTypeTestFail)
        {
            var client = new HttpClient();
            string bod = Newtonsoft.Json.JsonConvert.SerializeObject(inspEquipTypeTestFail);
            var stringContent = new StringContent(bod, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.PostAsync("", stringContent);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<InspEquipTypeTestFail>(content);
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
                //var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<InspEquipTypeTest>(content);
                return true;
            }
            return false;
        }

        public async Task<bool> Update(InspEquipTypeTest inspEquipTypeTest)
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

        public async Task<List<InspEquipTypeTestFail>> InspEquipTypeTestFails(int Inspequiptypetestid)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync(Inspequiptypetestid.ToString());
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InspEquipTypeTestFail>>(content);
                return await Task.FromResult(ret.ToList());
            }
            return null;
        }

        string url = "https://roofsafetysolutions.azurewebsites.net/api/InspEquipTypeTestFails/";



        public async Task<InspEquipTypeTestFail> InspEquipTypeTestFail(int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("int/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<InspEquipTypeTestFail>(content);
                return await Task.FromResult(ret);
            }
            return null;
        }




     
        public Task<bool> Update(InspEquipTypeTestFail inspEquipTypeTestFail)
        {
            throw new NotImplementedException();
        }
    }
}
