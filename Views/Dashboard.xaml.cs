using RssMob.Services;

namespace RssMob.Views;

public partial class Dashboard : ContentPage
{
  public  readonly IInspectionRepository _client = new InspectionServices();
    LoginPage _loginpage;
    public Dashboard(LoginPage loginpage)
	{
		InitializeComponent();
        _loginpage = loginpage;
        Loaded += Dashboard_Loaded;
	}
   public List<Models.InspectionView> Items;
    private async void Dashboard_Loaded(object sender, EventArgs e)
    {
        await RefreshDataAsync();
    }
    public async Task<bool> RefreshDataAsync()
    {
        Items = await _client.Inspections();
        foreach (var item in Items)
        {
            item.Photo = "https://rssblob.blob.core.windows.net/rssimage/" + item.Photo;
        }
        return true;
    }
     void Button_MapView(System.Object sender, System.EventArgs e)
    {
         OpenMap();
    }

    public async void OpenMap()
    {
        ;// await Navigation.PushAsync(new Views.RssMap(this));
    }

     void Button_Tabular(System.Object sender, System.EventArgs e)
    {
        OpenInsp();
    }

    public async void OpenInsp()
    {
       // await Navigation.PushAsync(new Views.RssMap(this));
       // await Navigation.PushAsync(new Views.Inspections(this));
    }

    async void Logout_Clicked(object sender, EventArgs e)
    {
        await Logout();
    }

    public async Task Logout()
    {
        _loginpage.logout();
        await Navigation.PopAsync();
    }

}
