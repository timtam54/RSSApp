using RssMob.Models;
using RssMob.Services;

namespace RssMob.Views;

public partial class EquipTypeSelect : ContentPage, iRefreshData
{
    //readonly IVersionRepository _versions = new VersionServices();
    //readonly IEquipTypeRepository _versions = new EquipTypeServices();
    InspEquip inspEquip;
   // int _InspectionID;
    public EquipTypeSelect(InspEquip _inspEquip,List<EquipType> equiptypes)
    {
        try
        {
            inspEquip = _inspEquip;
            InitializeComponent();
            Items = equiptypes.OrderBy(i=>i.EquipTypeDesc).ToList();
            RefreshDataAsync();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error.InspVersions", "Error.InspVersions:" + ex.Message, "OK");
        }
    }
    List<Models.EquipType> Items;
    public async Task<bool> RefreshDataAsync()
    {
        try
        {
//            Items = await _versions.EquipTypes();


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
        Button btn = (Button)sender;
        int id = (int)btn.CommandParameter;
        inspEquip.NewID(id);
         Navigation.PopAsync();
    }
}