using Newtonsoft.Json;
using RestSharp;
using RssMob.Models;
using RssMob.Services;
using static System.Net.WebRequestMethods;

namespace RssMob.Views;

public partial class SearchClientBuildingAddress : ContentPage, iRefreshData
{
    string _googleapikey = "AIzaSyAG6a1wcVG4vCjQATf6g1vP9vn5AeRPjTc&v=3.exp";
    readonly RestClient _rclient = new RestClient("https://maps.googleapis.com/");


    List<Models.Building> _Items;
    List<Models.Building> _New;
    readonly IBuildingRepository _bui = new BuildingServices();
    Inspect _par;
    IInspectionRepository _insp;
    public int BuildingID = 0;
    public SearchClientBuildingAddress(Inspect par, IInspectionRepository insp)
	{
        try { 
		InitializeComponent();
        _par=  par;
        _insp = insp;
        }
        catch (Exception ex)
        {
             DisplayAlert("Error.SearchClientBuildingAddress", "Error.SearchClientBuildingAddress:" + ex.Message, "OK");
        }

    }
    public SearchClientBuildingAddress( IInspectionRepository insp)
    {
        InitializeComponent();
      
        _insp = insp;
     

    }
    public void NewID(int id)
    {
        ;
    }
    public async Task<bool> RefreshDataAsync()
    {
        try { 
        _Items =await _bui.Buildings(search);
        return true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.RssMap", "Error.searchclientbuiding.RefreshDataAsync:" + ex.Message, "OK");
        }
        return false;
    }
    //private  void  Inspections_Loaded(object sender, EventArgs e)
    //{
       
    //}
    string search = ""; 
    private async void entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try { 
        search = e.NewTextValue;
        if (search == null)
            return;
        await RefreshDataAsync();
        string searchWA = search;// + " Western Australia";
        searchWA = searchWA.Replace("  "," ");
        searchWA = searchWA.Replace("  ", " ");
        searchWA = searchWA.Replace(" ", "+");
        //searchWA = searchWA.Replace(" ", "%20");

        //var request = new RestRequest($"maps/api/place/textsearch/json?query={searchWA}&location=-31.953512%2C115.857048&radius=50000&key={_googleapikey}");
        //textsearch/json?query
        string searchurl = $"maps/api/place/autocomplete/json?input={searchWA}&location=-31.953512%2C115.857048&radius=40000&key={_googleapikey}";
        var request = new RestRequest(searchurl);
        string response = (await _rclient.ExecuteAsync(request)).Content;
        var ss = response;
        if (response != null)
        {
            AutocompleteResponse deserialiseResponse = JsonConvert.DeserializeObject<AutocompleteResponse>(response);
            if (deserialiseResponse.predictions != null)
            {
                int ii = 100;   
                var cc = deserialiseResponse.predictions.Count;
                _New = new List<Models.Building>();
                foreach (var item in deserialiseResponse.predictions)
                {
                    Models.Building bd = new Models.Building();
                    bd.id = ii++;
                    bd.Address = item.description;
                    bd.BuildingName = item.place_id;
                    bd.ClientID = 0;
                    _New.Add(bd);
                }
            }
        }
            
        InspectionList.ItemsSource = _Items;
        NewList.ItemsSource = _New;
        BindingContext = _Items;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.SearcClientBuildingAddress", "Error.SearcClientBuildingAddress.entry_textchanged:" + ex.Message, "Cancel");

        }
    }
    public class MainTextMatchedSubstring
    {
        public int length { get; set; }
        public int offset { get; set; }
    }

    public class MatchedSubstring
    {
        public int length { get; set; }
        public int offset { get; set; }
    }

    public class Prediction
    {
        public string description { get; set; }
        public List<MatchedSubstring> matched_substrings { get; set; }
        public string place_id { get; set; }
        public string reference { get; set; }
        public StructuredFormatting structured_formatting { get; set; }
        public List<Term> terms { get; set; }
        public List<string> types { get; set; }
    }

    public class AutocompleteResponse
    {
        public List<Prediction> predictions { get; set; }
        public string status { get; set; }
    }

    public class StructuredFormatting
    {
        public string main_text { get; set; }
        public List<MainTextMatchedSubstring> main_text_matched_substrings { get; set; }
        public string secondary_text { get; set; }
    }

    public class Term
    {
        public int offset { get; set; }
        public string value { get; set; }
    }
   
   /* private async void search_TextChanged(object sender, TextChangedEventArgs e)
    {
        search = e.NewTextValue;
        if (search == null)
            return;
        await RefreshDataAsync();
        InspectionList.ItemsSource = _Items;
        NewList.ItemsSource = _New;
        BindingContext = _Items;
    }*/



    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        var placeid=btn.CommandParameter;
        if (placeid == null) return;
        string url = "https://www.google.com/maps/search/?api=1&query=Google&query_place_id=" + placeid;
        Uri uri = new Uri(url);
        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }

    string Address;
    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        Address = btn.CommandParameter.ToString();

        Views.ClientSearch xx = new Views.ClientSearch(this);
        await Navigation.PushAsync(xx);

    }

    public async void CreateBuilding(int ClientID)
    {
        Models.Building bd = new Models.Building();
        bd.Address = Address;
        bd.ClientID = ClientID;
        bd.BuildingName = Address;
        Models.Building newbd =await _bui.AddNew(bd);
       await _par.SetBuildingID(newbd.id);
        await _par.SaveNewInspDetails();
        await Navigation.PopAsync();
    }

    private void EditBuilding(object sender, EventArgs e)
    {
        Button button = sender as Button;
        if (_par == null)
        {
            int Buildingid = Convert.ToInt32(button.CommandParameter);
            Navigation.PushAsync(new Views.Building(this, Buildingid, _bui, _insp));
            return;
        }
        _par.SetBuildingID(Convert.ToInt32(button.CommandParameter));
        Navigation.PopAsync();
        _par.SaveNewInspDetails();
        Navigation.PopAsync();
    }
}