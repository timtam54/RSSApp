using RssMob.Services;

namespace RssMob.Views;

public partial class ClientSearch : ContentPage
{
    readonly IClientRepository _bui = new ClientServices();

    SearchClientBuildingAddress _par;
    public ClientSearch(SearchClientBuildingAddress par)
    {
        _par = par;
        InitializeComponent();

    }
    List<Models.Client> _Items;
    string search = "";
    private async void entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        search = e.NewTextValue;
        if (search == null)
            return;
        await RefreshDataAsync();

        InspectionList.ItemsSource = _Items;
        //NewList.ItemsSource = _New;
        BindingContext = _Items;
    }

    public async Task<bool> RefreshDataAsync()
    {
        //await _dashboard.RefreshDataAsync();
        _Items = await _bui.Clients(search);
        return true;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        int ClientID = Convert.ToInt32(button.CommandParameter);
        _par.CreateBuilding(ClientID);
        await Navigation.PopAsync();
        //_par.SetBuildingID(Convert.ToInt32(button.CommandParameter));

    }
}