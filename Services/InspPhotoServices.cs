using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public class InspPhotoServices : IInspPhotoRepository
    {
        public async Task<bool> Delete(int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.DeleteAsync(id.ToString());
            if (response.IsSuccessStatusCode)
            {
                //string content = response.Content.ReadAsStringAsync().Result;
                //var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<Inspection>(content);
                return true;
            }
            return false;
        }
        public InspPhotoServices()
        {
        }



        string url = "https://roofsafetysolutions.azurewebsites.net/api/inspphotos/";
        public async Task<List<InspPhoto>> InspPhotos(int id,string SoureTable)
        {
            var ret = new List<InspPhoto>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(url + id.ToString()+"~"+SoureTable);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InspPhoto>>(content);
                return await Task.FromResult(ret.ToList());
            }
            return null;
        }

    }
}