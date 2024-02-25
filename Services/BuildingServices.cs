using ObjCRuntime;
using RssMob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssMob.Services
{
    public class BuildingServices : IBuildingRepository
    {
        public async Task<Building> AddNew(Building hazard)
        {

            var client = new HttpClient();
            string bod = Newtonsoft.Json.JsonConvert.SerializeObject(hazard);
            var stringContent = new StringContent(bod, Encoding.UTF8, "application/json");
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.PostAsync("", stringContent);
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<Building>(content);
                return await Task.FromResult(ret);
            }
            return null;
        }
        string url = "https://roofsafetysolutions.azurewebsites.net/api/Buildings";
        public async Task<Building> Building(int id)
        {
            try
            {
                Building inspections;
                var client = new HttpClient();


                client.BaseAddress = new Uri(url + "/int/" + id.ToString());
                HttpResponseMessage response = await client.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    inspections = Newtonsoft.Json.JsonConvert.DeserializeObject<Building>(content);
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


        public async Task<List<Building>> Buildings(string search)
        {
            try
            {
                List<Building> inspections;
                var client = new HttpClient();
         

                client.BaseAddress = new Uri(url+ "/" + search.ToString());
                HttpResponseMessage response = await client.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    inspections = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Building>>(content);
                    foreach (var item in inspections)
                    {
                        var mm = item.BuildingName;
                        var mms = mm.Split(new char[] { Char.Parse("~") }, StringSplitOptions.RemoveEmptyEntries);
                        if (mms.Length > 1)
                        {
                            item.ClientName = mms[0];
                            item.BuildingName = mms[1];
                        }
                    }
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
