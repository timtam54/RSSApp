using Newtonsoft.Json;
using RestSharp;

namespace RssMob.Views;

public partial class Inspections : ContentPage, iRefreshData
{
    List<Models.InspectionView> _Items;

    bool Loading = true;
    readonly LoginPage _dashboard;
    public Inspections(LoginPage dashboard)
    {
        Loading = true;
        _dashboard = dashboard;
        _Items = dashboard.Items;
        InitializeComponent();
        Loaded += Inspections_Loaded;
    }
    public void NewID(int id)
    {
        ;
    }
    
    private async void entry_TextChanged(object sender, TextChangedEventArgs e)
    {
       _dashboard.search = e.NewTextValue;
        if (_dashboard.search == null)
            return;
        await RefreshDataAsync();
       
    }
    public async Task<bool> RefreshDataAsync()
    {
        dteFrom.Date=_dashboard.DteFrom;
        dteTo.Date = _dashboard.DteTo;
        await _dashboard.RefreshDataAsync();
        _Items = _dashboard.Items;
        Reload();
        return true;
    }
    private void Inspections_Loaded(object sender, EventArgs e)
    {
        dteFrom.Date = _dashboard.DteFrom;
        dteTo.Date = _dashboard.DteTo;
        Reload();
        Loading = false;
    }
   public void Reload()
    {
        InspectionList.ItemsSource = _Items;
        BindingContext = _Items;
    }
    async void Button_InspectItem(System.Object sender, System.EventArgs e)
    {
        Button btn = (Button)sender;
        var InspectionID = btn.CommandParameter;
        await Navigation.PushAsync(new Views.Inspect(Convert.ToInt32(InspectionID),  _dashboard._client,this));
    }

    async void Button_MapView(System.Object sender, System.EventArgs e)
    {
        await Navigation.PopAsync();//.PushAsync(new Views.maps(_Items));
        _dashboard.OpenMap();

    }

    async void AddNew_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Inspect(0, _dashboard._client, this));
        //await Navigation.PushAsync(new Views.SearchClientBuildingAddress(this, _dashboard._client));
        //        await Navigation.PushAsync(new Views.Inspect(0, "New", _dashboard._client,this));

    }

    async void Logout_Clicked(object sender, EventArgs e)
    {
        await _dashboard.Logout();
        await Navigation.PopAsync();
    }

    private async void dteFrom_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (Loading) return;
        _dashboard.DteFrom = dteFrom.Date;
        await RefreshDataAsync();

    }

    private async void dteTo_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (Loading) return;
        _dashboard.DteTo = dteTo.Date;
        await RefreshDataAsync();

    }
}
