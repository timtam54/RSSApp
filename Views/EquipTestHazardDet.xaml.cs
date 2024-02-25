using RssMob.Models;
using RssMob.Services;
using System.Runtime.CompilerServices;

namespace RssMob.Views;

public partial class EquipTestHazardDet : ContentPage,iRefreshData
{
    IETTestHazardRepository _items;
    iRefreshData _par;
    int _id;
    int _EquipTypeTestID;
    public EquipTestHazardDet(iRefreshData par, int id, IETTestHazardRepository items, int EquipTypeTestID)
	{
        try { 
         _par = par;
        _EquipTypeTestID = EquipTypeTestID;
        _items = items;
        _id = id;
        InitializeComponent();
        RefreshDataAsync();
        }
        catch (Exception ex)
        {
             DisplayAlert("Error.EquipTestHazardDet", "Error.EquipTestHazardDet:" + ex.Message, "OK");
        }
    }
    public void NewID(int id)
    {
        try { 
        Loading = true;
        Item.HazardID = id;
        Item.SelHazard = Item.Hazards.Where(i => i.Value == Item.HazardID).FirstOrDefault();
        BindingContext = Item;
        
        }
        catch (Exception ex)
        {
            DisplayAlert("Error.EquipTestHazardDet", "Error.NewID:" + ex.Message, "OK");
        }
        Loading = false;
    }

  
    Models.EquipTypeTestHazards Item;
    public async Task<bool> RefreshDataAsync()
    {
        try { 
        Loading = true;
        if (_id == 0)
        {
            Item = new Models.EquipTypeTestHazards();
            Item.EquipTypeTestID = _EquipTypeTestID;
        }
        else
            Item = await _items.InspETTestHazard(_id);

        List<Hazard> haz = await (new HazardServices()).Hazards();

        Item.Hazards = (from ins in haz select new SelectListItem { Text = ins.Detail, Value = ins.id }).ToList();
        SelectListItem sli = new SelectListItem();
        sli.Text = "-Add New-";
        sli.Value = 0;
        Item.Hazards.Add(sli);
        if (_id == 0)
        {
            Item.SelHazard = null;// Item.Hazards.FirstOrDefault();

           // EquipTypeName = Item.SelEquipType.Text;
        }
        else
        {
            Item.SelHazard = Item.Hazards.Where(i => i.Value == Item.HazardID).FirstOrDefault();

          //  EquipTypeName = Item.SelEquipType.Text;

        }
        BindingContext = Item;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.EquipTestHazardDet", "Error.RefreshDataAsync:" + ex.Message, "OK");
        }
        Loading = false;
        return true;
    }
    string EquipTypeName;
    

    async void TestClicked(System.Object sender, System.EventArgs e)
    {

       // await Navigation.PushAsync(new Views.InspEquipTest(_id, Item.EquipTypeID, EquipTypeName));// Convert.ToInt32(_ID)));
    }

    async void SaveClose_Clicked(System.Object sender, System.EventArgs e)
    {
        try { 
        Models.EquipTypeTestHazards Item = (Models.EquipTypeTestHazards)BindingContext;
        Item.HazardID = Item.SelHazard.Value;
        if (Item.id != 0)
           await _items.Update(Item);
        else
          await _items.AddNew(Item);
        _par.RefreshDataAsync();
       
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.EquipTestHazardDet", "Error.SaveClose_Clicked:" + ex.Message, "OK");
        }
        await Application.Current.MainPage.Navigation.PopAsync();
    }
    bool Loading = true;
    async void HazardID_SelectedIndexChanged(object sender, EventArgs e)
    {
        try { 
        if (Loading)
            return;
        Picker pk = (Picker)sender;
        var ss = ((SelectListItem)pk.SelectedItem).Text;
        pk.Unfocus();
        if (ss == "-Add New-")
        {
            await Navigation.PushAsync(new Views.HazardDet(this, 0));
        }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.EquipTestHazardDet", "Error.HazardID_SelectedIndexChanged:" + ex.Message, "OK");
        }
    }
}