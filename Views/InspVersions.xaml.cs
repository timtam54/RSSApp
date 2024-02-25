using RssMob.Services;

namespace RssMob.Views;

public partial class InspVersions : ContentPage,iRefreshData
{
    readonly IVersionRepository _versions = new VersionServices();
    IInspectionRepository _insp;
    int _Buildingid;
    public InspVersions(int Buildingid, IInspectionRepository insp)
    {
        try {
            _insp = insp;
            _Buildingid = Buildingid;
        InitializeComponent();
        RefreshDataAsync();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error.InspVersions", "Error.InspVersions:" + ex.Message, "OK");
        }
    }
    List<Models.VersionRpt> Items;
    public async Task<bool> RefreshDataAsync()
    {
        try { 
        Items = await _versions.Versions(_Buildingid);
            foreach (var item in Items)
            {
                item.Photo = "https://rssblob.blob.core.windows.net/rssimage/" + item.Photo;
            }

            InspVersionList.ItemsSource = Items;
        return true;
        }
        catch (Exception ex)
        {
           await DisplayAlert("Error.InspVersions", "Error.InspVersions.RefreshDataAsync:" + ex.Message, "OK");
        }
        return false;
    }

   

    async void Button_Close(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    public void NewID(int id)
    {
        ;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            Button btn = (Button)sender;
            var InspectionID = btn.CommandParameter;
             Navigation.PushAsync(new Views.Inspect(Convert.ToInt32(InspectionID), _insp, this, 0));
        }
        catch (Exception ex)
        {
             DisplayAlert("Error.Inspections", "Error.Inspections.Button_InspectItem:" + ex.Message, "OK");
        }
    }
}