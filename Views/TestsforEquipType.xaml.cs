using RssMob.Services;

namespace RssMob.Views;

public partial class TestsforEquipType : ContentPage, iRefreshData
{
    readonly IEquipTypeTestRepository equiptypetestservice = new EquipTypeTestServices();
    int _EquipTypeID;
    string _EquipType;
    public TestsforEquipType(int EquipTypeID, string EquipType)
    {
        try { 
        _EquipTypeID = EquipTypeID;
        _EquipType = EquipType;
        InitializeComponent();
        Title = "Tests for " + EquipType;
        RefreshDataAsync();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error.TestsforEquipType", "Error.TestsforEquipType:" + ex.Message, "OK");
        }
    }
    async void Button_Close(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    public async Task<bool> RefreshDataAsync()
    {
        try { 
        List<Models.EquipTypeTest> Items = await equiptypetestservice.EquipTypeTests(_EquipTypeID);
        // EquipTypeTestList.EquipType = _EquipType;
        EquipTypeTestList.ItemsSource = Items;
        return true;
        }
        catch (Exception ex)
        {
           await DisplayAlert("Error.TestsforEquipType", "Error.TestsforEquipType.RefreshDataAsync:" + ex.Message, "OK");
        }
        return false;
    }

    async void Button_Delete(object sender, EventArgs e)
    {
        try { 
        ImageButton btn = (ImageButton)sender;
        bool suc = await equiptypetestservice.Delete(Convert.ToInt32(btn.CommandParameter));
        if (!suc)
        {
            await DisplayAlert("Error", "This Test if referenced by other Inspection Items - so cannot be deleted", "Cancel");
            return;

        }
        await RefreshDataAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.TestsforEquipType", "Error.TestsforEquipType.Button_Delete:" + ex.Message, "OK");
        }
    }
    async void Button_AddNew(System.Object sender, System.EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Views.EquipTypeTest(this, _EquipType, _EquipTypeID, 0));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.TestsforEquipType", "Error.TestsforEquipType.Button_AddNew:" + ex.Message, "OK");
        }
    }
    public void NewID(int id)
    {

    }
    public async void TestHazard_Clicked(object sender, EventArgs e)
    {
        try { 
        Button btn = (Button)sender;
        var ID = btn.CommandParameter;

        await Navigation.PushAsync(new Views.EquipTypeTest(this, _EquipType, _EquipTypeID, Convert.ToInt32(ID)));

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.TestsforEquipType", "Error.TestsforEquipType.TestHazard_Clicked:" + ex.Message, "OK");
        }

    }

}