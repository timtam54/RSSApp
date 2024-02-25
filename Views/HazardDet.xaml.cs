using RssMob.Models;
using System.Text;

namespace RssMob.Views;

public partial class HazardDet : ContentPage
{
    iRefreshData _par;
	int _id;

    public HazardDet(iRefreshData par,int id)
	{
        try { 
		_id = id;
		_par = par;
		InitializeComponent();
		RefreshDataAsync();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error.HazardDet", "Error.HazardDet:" + ex.Message, "OK");
        }
    }
	public bool RefreshDataAsync()
	{
        try { 
		item = new Hazard();
		BindingContext = item;
		return true;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error.HazardDet", "Error.HazardDet.RefreshDataAsync:" + ex.Message, "OK");
        }
        return false;
    }

    RssMob.Services.IHazardRepository _items = new RssMob.Services.HazardServices();
    async void SaveClose_Clicked(System.Object sender, System.EventArgs e)
    {
        try { 
        Models.Hazard Item = (Models.Hazard)BindingContext;
       
      
      var res=      await _items.AddNew(Item);
 
      await  _par.RefreshDataAsync();
        _par.NewID(res.id);
        await Application.Current.MainPage.Navigation.PopAsync();
        }
        catch (Exception ex)
        {
           await DisplayAlert("Error.HazardDet", "Error.HazardDet.SaveClose_Clicked:" + ex.Message, "OK");
        }
    }
    Hazard item;
}