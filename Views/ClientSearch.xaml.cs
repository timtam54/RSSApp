using RssMob.Services;

namespace RssMob.Views;

public partial class ClientSearch : ContentPage
{
    readonly IClientRepository _bui = new ClientServices();

    SearchClientBuildingAddress _par;
    public ClientSearch(SearchClientBuildingAddress par)
    {
        try { 
        _par = par;
        InitializeComponent();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error.ClientSearch", "Error.ClientSearch:" + ex.Message, "OK");
        }
    }
    List<Models.Client> _Items;
    string search = "";
    private async void entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try { 
        search = e.NewTextValue;
        if (search == null)
            return;
        await RefreshDataAsync();

        InspectionList.ItemsSource = _Items;

        BindingContext = _Items;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.ClientSearch", "Error.entry_TextChanged:" + ex.Message, "OK");
        }
    }

    public async Task<bool> RefreshDataAsync()
    {
        try { 
        _Items = await _bui.Clients(search);
        return true;
        }
        catch (Exception ex)
        {
           await DisplayAlert("Error.ClientSearch", "Error.RefreshDataAsync:" + ex.Message, "OK");
        }
        return false;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try { 
        Button button = sender as Button;
        int ClientID = Convert.ToInt32(button.CommandParameter);
        _par.CreateBuilding(ClientID);
        await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.ClientSearch", "Error.Button_Clicked:" + ex.Message, "OK");
        }

    }
}