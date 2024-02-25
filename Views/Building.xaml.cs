using RestSharp;
using RssMob.Models;
using RssMob.Services;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using ThreadNetwork;

namespace RssMob.Views;

public partial class Building : ContentPage, iRefreshData
{
    public void NewID(int id)
    {
        ;
    }

    iRefreshData _par;
    int _id;
    readonly IClientRepository _cli = new ClientServices();
    IBuildingRepository _Build;
    IInspectionRepository _insp;
    public Building(iRefreshData par, int id, IBuildingRepository Build, IInspectionRepository insp)
    {
        try {
            _insp = insp;
        InitializeComponent();
        _par = par;
        _id = id;
        _Build = Build;
        RefreshDataAsync();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error.Building", "Error.Building:" + ex.Message, "OK");
        }
    }

    async void Button_PhotoAdd(System.Object sender, System.EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Photo(_id, this, Photo.PhotoType.Building));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.InspEquip", "Error.InspEquip.Button_PhotoAdd:" + ex.Message, "Cancel");

        }
    }
    List<Models.InspPhoto> Items;
    Models.Building Item;
   // bool Loading = false;

    async void Button_PhotoDelete(object sender, EventArgs e)
    {
        try
        {
            ImageButton btn = (ImageButton)sender;
            await ip.Delete(Convert.ToInt32(btn.CommandParameter));
            await RefreshDataAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error - Button_PhotoDelete", ex.Message, "OK");
        }
    }
    public async Task<bool> RefreshDataAsync()
    {
        try {
            Indi.IsRunning = true;
            Indi.IsVisible = true;

          //  Loading = true;
            if (_id == 0)
                Item = new Models.Building();
            else
                Item =await _Build.Building(_id);

        List<Models.Client> clis = await _cli.Clients("*");
        Item.Clients = (from ins in clis select new SelectListItem { Text = ins.name, Value = ins.id }).ToList();
            Item.SelectClientID = null;// Item.Insps.Where(i => i.Value == Item.InspectorID).FirstOrDefault();
      BindingContext = Item;


            if (_id == 0)
            {
              //  Item.SelEquipType = Item.Et.FirstOrDefault();
               // EquipTypeName = Item.SelEquipType.Text;
                Items = new List<InspPhoto>();
                InspPhotosCarousel.IsVisible = false;
                InspPhotosCarousel.ItemsSource = Items;
                
                //btnAddPhoto.IsVisible = false;
               
            }
            else
            {
                Item.SelectClientID = Item.Clients.Where(i => i.Value == Item.ClientID).FirstOrDefault();
               // EquipTypeName = Item.SelEquipType.Text;
                Items = await ip.InspPhotos(_id, "B");
                foreach (var item in Items)
                {
                    item.photoname = "https://rssblob.blob.core.windows.net/rssimage/" + item.photoname;
                }
                InspPhotosCarousel.IsVisible = true;
                InspPhotosCarousel.ItemsSource = Items;
               
               // btnAddPhoto.IsVisible = true;
              
            }
            BindingContext = Item;
            Indi.IsRunning = false;
            Indi.IsVisible = false;


         //   Loading = false;
        return true;
        }
        catch (Exception ex)
        {
           await DisplayAlert("Error.Building", "Error.Building.RefreshDataAsync:" + ex.Message, "OK");
        }
        return false;
    }
    IInspPhotoRepository ip = new InspPhotoServices();
    async void SaveClose_Clicked(System.Object sender, System.EventArgs e)
    {
        try 
        { 
        Models.Building Item = (Models.Building)BindingContext;
        if (Item.SelectClientID!=null)
        Item.ClientID = Item.SelectClientID.Value;
        
       // if (Item.id != 0)
        var bd=    await _Build.AddNew(Item);
        // else
        // {
        //     _InspectionID = (await _insp.AddNew(Item)).id;
        // }
        _id = bd.id;
        await _par.RefreshDataAsync();
        _par.NewID(bd.id);
        await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.Building", "Error.Building.Button_Save:" + ex.Message, "OK");
        }
    }

    private void Inspections_Clicked(object sender, EventArgs e)
    {
         Navigation.PushAsync(new Views.InspVersions(Convert.ToInt32(_id),_insp));
    }
}