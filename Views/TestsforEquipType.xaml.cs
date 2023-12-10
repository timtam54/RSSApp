using RssMob.Services;

namespace RssMob.Views;

public partial class TestsforEquipType : ContentPage, iRefreshData
{
    readonly IEquipTypeTestRepository equiptypetestservice = new EquipTypeTestServices();
    int _EquipTypeID;
    string _EquipType;
    public TestsforEquipType(int EquipTypeID, string EquipType)
    {
        _EquipTypeID = EquipTypeID;
        _EquipType = EquipType;
        InitializeComponent();
        Title = "Tests for " + EquipType;
        RefreshDataAsync();
    }
    async void Button_Close(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    public async Task<bool> RefreshDataAsync()
    {
        List<Models.EquipTypeTest> Items = await equiptypetestservice.EquipTypeTests(_EquipTypeID);
        // EquipTypeTestList.EquipType = _EquipType;
        EquipTypeTestList.ItemsSource = Items;
        return true;
    }

    async void Button_Delete(object sender, EventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        bool suc = await equiptypetestservice.Delete(Convert.ToInt32(btn.CommandParameter));
        if (!suc)
        {
            DisplayAlert("Error", "This Test if referenced by other Inspection Items - so cannot be deleted", "Cancel");
            return;

        }
        RefreshDataAsync();

    }
    async void Button_AddNew(System.Object sender, System.EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Views.EquipTypeTest(this, _EquipType, _EquipTypeID, 0));
        }
        catch (Exception ex)
        {
            ;// DisplayAlert("Error", ex.Message, "Cancel");
        }
    }
    public void NewID(int id)
    {

    }
    public async void TestHazard_Clicked(object sender, EventArgs e)
    {

        Button btn = (Button)sender;
        var ID = btn.CommandParameter;

        await Navigation.PushAsync(new Views.EquipTypeTest(this, _EquipType, _EquipTypeID, Convert.ToInt32(ID)));

        //await Navigation.PushAsync(new Views.EquipTestHazards(Convert.ToInt32(ID), btn.Text));

    }

}