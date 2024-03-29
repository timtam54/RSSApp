﻿using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Linq.Expressions;

namespace RssMob.Views;

public partial class RssMap : ContentPage, iRefreshData
{
    int _LoginID;

    public RssMap(LoginPage dashboard,int LoginID)
    {
        try
        {
            _LoginID=LoginID;
            _Items = dashboard.Items;
            _dashboard = dashboard;
            InitializeComponent();
         //   Location location = new Location(-31.9523, 115.8613);
           // MapSpan mapSpan = new MapSpan(location, .5, .5);
            Microsoft.Maui.Controls.Maps.Map map = myMap;//
                                                         //new Microsoft.Maui.Controls.Maps.Map(mapSpan)
                                                         // {
                                                         //   MapType = MapType.Street
                                                         //};
            //map.MapType = MapType.Street;
           // map.MoveToRegion(mapSpan);
           // Content = map;
            map.IsTrafficEnabled = true;
            map.IsShowingUser = true;
            RefreshDataAsync(map);
            //Button btn = new Button();
            map.MapClicked += Map_MapClicked;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error.RssMap", "Error.RssMap:" + ex.Message, "OK");
        }
    }
    private void Building_Clicked(object sender, EventArgs e)
    {
        try
        {
           // Button btn = (Button)sender;
           // var InspectionID = btn.CommandParameter;
            Navigation.PushAsync(new Views.SearchClientBuildingAddress(_dashboard._client));
        }
        catch (Exception ex)
        {
            DisplayAlert("Error.Inspections", "Error.Inspections.Building_Clicked:" + ex.Message, "OK");
        }
    }
    public async Task<bool> RefreshDataAsync()
    {
        try { 
        await _dashboard.RefreshDataAsync();
        _Items = _dashboard.Items;
        return true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.RssMap", "Error.RssMap.RefreshDataAsync:" + ex.Message, "OK");
        }
        return false;
    }
    async void AddNew_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Inspect(0, _dashboard._client, this, _LoginID));
    }
    public void NewID(int id)
    {
        ;
    }
    async void Logout_Clicked(object sender, EventArgs e)
    {
        await _dashboard.Logout();
        await Navigation.PopAsync();
    }

    List<Models.InspectionView> _Items;
    readonly LoginPage _dashboard;
  


    public async void Map_MapClicked(object sender, MapClickedEventArgs e)
    {
        try { 
        await Navigation.PopAsync();
        _dashboard.OpenInsp(_LoginID);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.RssMap", "Error.RssMap.Map_MapClicked:" + ex.Message, "OK");
        }
    }
    public async void Grid_Clicked(object sender, EventArgs e)
    {
        try { 
        await Navigation.PopAsync();
        _dashboard.OpenInsp(_LoginID);
        }
        catch (Exception ex)
        {
          await  DisplayAlert("Error.RssMap", "Error.RssMap.Grid_Clicked:" + ex.Message, "OK");
        }
    }
    // public List<Models.InspectionView> Items { get; private set; }

    public async void RefreshDataAsync(Microsoft.Maui.Controls.Maps.Map map)
    {
        try { 
        await AddLatLonPins(map);
        Location location = new Location(-31.9523, 115.8613);
        MapSpan mapSpan = new MapSpan(location, .5, .5);
        map.MoveToRegion(mapSpan);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.RssMap", "Error.RssMap.RefreshDataAsync:" + ex.Message, "OK");
        }
    }

    private async Task AddLatLonPins(Microsoft.Maui.Controls.Maps.Map map)
    {
        try
        {
            foreach (var iv in _Items.Where(i=>i.Status=="Active"))
            {
                
                    IEnumerable<Microsoft.Maui.Devices.Sensors.Location> locations = null;
                try
                {
                    locations = (await Geocoding.Default.GetLocationsAsync(iv.Address));
                }
                catch (Exception ex)
                {
                    var dd = ex.Message;
                }
                if (locations != null)
                    {
                        Microsoft.Maui.Devices.Sensors.Location location = locations?.FirstOrDefault();
                        if (location != null)
                        {
                            iv.Lat = location.Latitude;
                            iv.Lon = location.Longitude;
                            //+ "~" + location.Altitude;
                        }
                        else
                        {
                            iv.Lat = -31.9523;
                            iv.Lon = 115.8613;

                        }
                    }
                
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error1", ex.Message, "Cancel");
        }
        try
        {
            foreach (var iv in _Items)
            {
                if (iv.Lat != null)
                {
                    if (iv.Lon != null)
                    {
                        Pin pin = new Pin
                        {
                            Label = iv.ClientName + " " + iv.InspDate.ToShortDateString(),
                            Address = iv.Address,
                            Type = PinType.Place,
                            Location = new Location(iv.Lat.Value, iv.Lon.Value)
                        };
                        pin.StyleId = iv.id.ToString();
                        map.Pins.Add(pin);
                        pin.MarkerClicked += Pin_MarkerClicked;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.RssMap", "Error.RssMap.AddLatLonPins:" + ex.Message, "OK");
        }
    }

    private async void Pin_MarkerClicked(object sender, PinClickedEventArgs e)
    {
        try { 
        Pin Det = (Pin)sender;
        string InspectionID = Det.StyleId.ToString();
        await Navigation.PushAsync(new Views.Inspect(Convert.ToInt32(InspectionID),  _dashboard._client,this, _LoginID));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.RssMap", "Error.RssMap.Pin_MarkerClicked:" + ex.Message, "OK");
        }
    }
}
