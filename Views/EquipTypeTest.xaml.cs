using Microsoft.Maui.Layouts;
using RssMob.Models;
using RssMob.Services;
using ThreadNetwork;

namespace RssMob.Views;

public partial class EquipTypeTest : ContentPage, iRefreshData
{
	string _EquipType;
    int _EquipTypeID;
    iRefreshData _par;
    int _id;
    public EquipTypeTest(iRefreshData par,string EquipType,int EquipTypeID,int id)
	{
        _par = par;
        _EquipType = EquipType;
        _EquipTypeID = EquipTypeID;
        _id = id;
        InitializeComponent();
        Title = "Add/Edit Test for " + EquipType;
        RefreshDataAsync();
    }
    public void NewID(int id)
    {
        ;
    }
    async void Button_Close(System.Object sender, System.EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PopAsync();
    }
    async void Button_Save(System.Object sender, System.EventArgs e)
    {
        bool newrec;
        Models.EquipTypeTest Item = (Models.EquipTypeTest)BindingContext;
        if (Item.id != 0)
        {
            await etr.Update(Item);
            newrec = false;
        }
        else
        {
            var ett = await etr.AddNew(Item);
            _id = ett.id;
            newrec = true;
        }
        if (newrec)
            await RefreshDataAsync();
        else
        {
            await _par.RefreshDataAsync();
            _par.NewID(_id);
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
    async void Button_AddNew(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new Views.EquipTestHazardDet(this, Convert.ToInt32(0), _client, _id));
    }

    IETTestHazardRepository _client = new ETTestHazardService();
    Models.EquipTypeTest Item;
    Services.IEquipTypeTestRepository etr=new Services.EquipTypeTestServices();
    public async Task<bool> RefreshDataAsync()
    {
        List<Models.EquipTypeTestHazards> Its;
        if (_id == 0)
        {
            Item = new Models.EquipTypeTest();
            Item.EquipTypeID = _EquipTypeID;
            Its = new List<EquipTypeTestHazards>();
            btnAddHaz.IsVisible = false;
            InspectionList.IsVisible = false;
            AddNewHaz.IsVisible = false;
        }
        else
        {
            Item = await etr.EquipTypeTest(_id);
            Its = await _client.InspETTestHazards(_id);
            List<Hazard> haz = await (new HazardServices()).Hazards();
            foreach (var It in Its)
            {
                It.Hazard = haz.Where(i => i.id == It.HazardID).FirstOrDefault();
            }
            btnAddHaz.IsVisible = true;
            InspectionList.ItemsSource = Its;
            InspectionList.IsVisible = true;
            AddNewHaz.IsVisible = true;
        }
        BindingContext = Item;
        return true;
    }
    async void Button_HazardDetails(System.Object sender, System.EventArgs e)
    {
        Button btn = (Button)sender;
        var id = btn.CommandParameter;
        await Navigation.PushAsync(new Views.EquipTestHazardDet(this, Convert.ToInt32(id), _client, _id));

    }

    async void Button_DeleteHazard(System.Object sender, System.EventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        var id = btn.CommandParameter;
        await _client.Delete(Convert.ToInt32(id));
        RefreshDataAsync();
    }
}