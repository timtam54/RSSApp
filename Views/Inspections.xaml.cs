

namespace RssMob.Views;

public partial class Inspections : ContentPage, iRefreshData
{
    List<Models.InspectionView> _Items;

    bool Loading = true;
    readonly LoginPage _dashboard;
    int _LoginID;
    public Inspections(LoginPage dashboard,int LoginID)
    {
        try { 
        Loading = true;
        _dashboard = dashboard;
            _LoginID = LoginID;
        _Items = dashboard.Items;
        InitializeComponent();
        Loaded += Inspections_Loaded;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error.Inspections", "Error.Inspections:" + ex.Message, "OK");
        }
    }
    public void NewID(int id)
    {
        ;
    }
    
    private async void entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try { 
       _dashboard.search = e.NewTextValue;
        if (_dashboard.search == null)
            return;
            _Items =_dashboard.Items.Where(i => i.ClientName.ToLower().ToString().Contains(_dashboard.search.ToLower()) || (i.Address!=null && i.Address.ToLower().ToString().Contains(_dashboard.search.ToLower())) || (i.TestingInstruments!=null && i.TestingInstruments.ToLower().ToString().Contains(_dashboard.search.ToLower())) || (i.Areas != null && i.Areas.ToLower().ToString().Contains(_dashboard.search.ToLower()))).ToList();// || i.Address.ToLower().ToString().Contains(_dashboard.search.ToLower())).
            Reload();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.Inspections", "Error.Inspections.entry_TextChanged:" + ex.Message, "OK");
        }
    }
    public async Task<bool> RefreshDataAsync()
    {
        try { 
       dteFrom.Date=_dashboard.DteFrom;
        dteTo.Date = _dashboard.DteTo;
        await _dashboard.RefreshDataAsync();
        _Items = _dashboard.Items;
        Reload();
        return true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.Inspections", "Error.Inspections.RefreshDataAsync:" + ex.Message, "OK");
        }
        return false;
    }
    private void Inspections_Loaded(object sender, EventArgs e)
    {
        try { 
        dteFrom.Date = _dashboard.DteFrom;
        dteTo.Date = _dashboard.DteTo;
        Reload();
        Loading = false;
        }
        catch (Exception ex)
        {
             DisplayAlert("Error.Inspections", "Error.Inspections.Inspections_Loaded:" + ex.Message, "OK");
        }
    }
   public void Reload()
    {
        InspectionList.ItemsSource = _Items;
        BindingContext = _Items;
    }
    async void Button_InspectItem(System.Object sender, System.EventArgs e)
    {
        try { 
        Button btn = (Button)sender;
        var InspectionID = btn.CommandParameter;
        await Navigation.PushAsync(new Views.Inspect(Convert.ToInt32(InspectionID),  _dashboard._client,this, _LoginID));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.Inspections", "Error.Inspections.Button_InspectItem:" + ex.Message, "OK");
        }
    }

    async void Button_MapView(System.Object sender, System.EventArgs e)
    {
        try { 
        await Navigation.PopAsync();//.PushAsync(new Views.maps(_Items));
        _dashboard.OpenMap();
        }
        catch (Exception ex)
        {
          await  DisplayAlert("Error.Inspections", "Error.Inspections.Button_MapView:" + ex.Message, "OK");
        }
    }

    async void AddNew_Clicked(object sender, EventArgs e)
    {
        try { 
        await Navigation.PushAsync(new Views.Inspect(0, _dashboard._client, this, _LoginID));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.Inspections", "Error.Inspections.AddNew_Clicked:" + ex.Message, "OK");
        }
    }

    async void Logout_Clicked(object sender, EventArgs e)
    {
        await _dashboard.Logout();
        await Navigation.PopAsync();
    }

    private async void dteFrom_DateSelected(object sender, DateChangedEventArgs e)
    {
        try { 
        if (Loading) return;
        _dashboard.DteFrom = dteFrom.Date;
        await RefreshDataAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.Inspections", "Error.Inspections.dteFrom_DateSelected:" + ex.Message, "OK");
        }
    }

    private async void dteTo_DateSelected(object sender, DateChangedEventArgs e)
    {
        try { 
        if (Loading) return;
        _dashboard.DteTo = dteTo.Date;
        await RefreshDataAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.Inspections", "Error.Inspections.dteTo_DateSelected:" + ex.Message, "OK");
        }
    }

    private void Building_Clicked(object sender, EventArgs e)
    {
        try
        {

            Navigation.PushAsync(new Views.SearchClientBuildingAddress( _dashboard._client));
        }
        catch (Exception ex)
        {
             DisplayAlert("Error.Inspections", "Error.Inspections.Building_Clicked:" + ex.Message, "OK");
        }
    }
}
