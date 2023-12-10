using RssMob.Models;

namespace RssMob.Views;

public partial class HazardDet : ContentPage
{
    iRefreshData _par;
	int _id;

    public HazardDet(iRefreshData par,int id)
	{
		_id = id;
		_par = par;
		InitializeComponent();
		RefreshDataAsync();

    }
	public bool RefreshDataAsync()
	{
		item = new Hazard();
		BindingContext = item;
		return true;

    }

    RssMob.Services.IHazardRepository _items = new RssMob.Services.HazardServices();
    async void SaveClose_Clicked(System.Object sender, System.EventArgs e)
    {
        Models.Hazard Item = (Models.Hazard)BindingContext;
       
        //if (Item.id != 0)
        //    await _items.Update(Item);
        //else
      var res=      await _items.AddNew(Item);
 
      await  _par.RefreshDataAsync();
        _par.NewID(res.id);
        await Application.Current.MainPage.Navigation.PopAsync();
    }
    Hazard item;
}