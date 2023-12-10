using RestSharp;
using RssMob.Models;
using RssMob.Services;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace RssMob.Views;

public partial class Building : ContentPage
{
    
    iRefreshData _par;
    int _id;
    readonly IClientRepository _cli = new ClientServices();
    IBuildingRepository _Build;
    public Building(iRefreshData par, int id, IBuildingRepository Build)
    {
        InitializeComponent();
        _par = par;
        _id = id;
        _Build = Build;
        RefreshDataAsync();
    }
    Models.Building Item;
    bool Loading = false;
    public async Task<bool> RefreshDataAsync()
    {
        Loading = true;
       Item = new Models.Building();
        List<Models.Client> clis = await _cli.Clients("*");
        Item.Clients = (from ins in clis select new SelectListItem { Text = ins.name, Value = ins.id }).ToList();
            Item.SelectClientID = null;// Item.Insps.Where(i => i.Value == Item.InspectorID).FirstOrDefault();
      BindingContext = Item;
        Loading = false;
        return true;
    }
    async void Button_Save(System.Object sender, System.EventArgs e)
    {
        Models.Building Item = (Models.Building)BindingContext;
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
        //await RefreshDataAsync();
    }
}