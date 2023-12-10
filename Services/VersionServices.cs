using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public class VersionServices : IVersionRepository
    {
        public VersionServices()
        {
        }

        public async Task<Models.Version> AddNew(Models.Version inspEquip)
        {
            var client = new HttpClient();
            string bod = Newtonsoft.Json.JsonConvert.SerializeObject(inspEquip);
            var stringContent = new StringContent(bod, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.PostAsync("", stringContent);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Version>(content);
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
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Version>(content);
                return true;
            }
            return false;
        }

        public Task<bool> Update(Models.Version inspEquip)
        {
            throw new NotImplementedException();
        }

        public async Task<Models.Version> Version(int id)
        {
      
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("int/" + id.ToString());

            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var inspections = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Version>(content);
                return await Task.FromResult(inspections);
            }
            return null;
        }
        string url = "https://roofsafetysolutions.azurewebsites.net/api/Versions/";

        public async Task<List<Models.VersionRpt>> Versions(int Inspectionid)
        {

            var client = new HttpClient();

            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync(Inspectionid.ToString());
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.VersionRpt>>(content);
                return await Task.FromResult(ret.ToList());
            }
            return null;
        }
    }
}