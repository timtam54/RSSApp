using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public class InspEquipServices : IInspEquipRepository
    {

        public InspEquipServices()
        {
        }

        public async Task<InspEquip> AddNew(InspEquip inspEquip)
        {
            var client = new HttpClient();
            string bod = Newtonsoft.Json.JsonConvert.SerializeObject(inspEquip);
            var stringContent = new StringContent(bod, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.PostAsync("", stringContent);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<InspEquip>(content);
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
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<InspEquip>(content);
                return true;
            }
            return false;
        }


        public async Task<InspEquip> InspItem(int Inspequipid)
        {
            var ret = new InspEquip();
            var client = new HttpClient();

            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("int/" + Inspequipid.ToString());
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<InspEquip>(content);
                return await Task.FromResult(ret);
            }
            return null;
        }
        string url = "https://roofsafetysolutions.azurewebsites.net/api/inspequips/";
        public async Task<List<InspEquipView>> InspItems(int Inspectionid)
        {
            var ret = new List<InspEquipView>();
            var client = new HttpClient();

            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync(Inspectionid.ToString());
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InspEquipView>>(content);
                return await Task.FromResult(ret.ToList());
            }
            return null;
        }

        public async Task<bool> Update(InspEquip inspEquip)
        {
            string bod = Newtonsoft.Json.JsonConvert.SerializeObject(inspEquip);
            var stringContent = new StringContent(bod, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.PutAsync(inspEquip.id.ToString(), stringContent);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                // ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InspEquipTypeTest>>(content);
                return true;// await Task.FromResult(ret.ToList());
            }
            return false;
        }
    }
}

