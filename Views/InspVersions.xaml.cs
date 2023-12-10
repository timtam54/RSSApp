using RssMob.Services;

namespace RssMob.Views;

public partial class InspVersions : ContentPage,iRefreshData
{
    readonly IVersionRepository _versions = new VersionServices();
    int _InspectionID;
    public InspVersions(int InspectionID)
    {
        _InspectionID = InspectionID;
        InitializeComponent();
        RefreshDataAsync();
    }
    List<Models.VersionRpt> Items;
    public async Task<bool> RefreshDataAsync()
    {
        Items = await _versions.Versions(_InspectionID);


        InspVersionList.ItemsSource = Items;
        return true;
    }

    async void Button_Edit(System.Object sender, System.EventArgs e)
    {
        Button btn = (Button)sender;
        var InspEquipID = btn.CommandParameter;
        await Navigation.PushAsync(new Views.InspVersion(this, Convert.ToInt32(InspEquipID), _versions, _InspectionID,Items.Count()));
    }

    async void Button_AddNew(System.Object sender, System.EventArgs e)
    {

        await Navigation.PushAsync(new Views.InspVersion(this, 0, _versions, _InspectionID, Items.Count()));

    }



    async void Button_Delete(System.Object sender, System.EventArgs e)
    {
        Button btn = (Button)sender;
        var InspEquipID = btn.CommandParameter;
        await _versions.Delete(Convert.ToInt32(InspEquipID));
        RefreshDataAsync();
    }

    async void Button_Close(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    public void NewID(int id)
    {
        ;
    }
}