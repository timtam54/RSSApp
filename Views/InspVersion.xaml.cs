using RssMob.Models;
using RssMob.Services;
using System.Runtime.CompilerServices;

namespace RssMob.Views;

public partial class InspVersion : ContentPage
{
    readonly IVersionRepository _versions;
    iRefreshData _par; int _cnt;
    int _id;
    int _InspectionID;
    readonly IEmployeeRepository _emp = new EmployeeServices();
    public InspVersion(iRefreshData par, int id, IVersionRepository versions, int InspectionID,int cnt)
    {
        _par = par;
        _InspectionID = InspectionID;
        _versions = versions;
        _id = id;
        _cnt = cnt;
        InitializeComponent();
        RefreshDataAsync();
    }
    bool Loading = false;

    Models.Version Item; IBuildingRepository _Build;
    public async Task<bool> RefreshDataAsync()
    {
        Loading = true;
        if (_id == 0)
        {
            Item = new Models.Version();
            Item.VersionNo = (1 + _cnt);
        }
        else
            Item = await _versions.Version(_id);
        List<Models.Employee> Insps = await _emp.Employees();
        Item.Authors = (from ins in Insps select new SelectListItem { Text = ins.Given + " " + ins.Surname, Value = ins.id }).ToList();
        if (_id == 0) 
            Item.SelectAuthorID = null;
        else
            Item.SelectAuthorID = Item.Authors.Where(i => i.Value == Item.AuthorID).FirstOrDefault();
        BindingContext = Item;
        Loading = false;
        return true;
   }


    async void Button_SaveClose(System.Object sender, System.EventArgs e)
    {
        Models.Version Item = (Models.Version)BindingContext;
        Item.AuthorID = Item.SelectAuthorID.Value;
        Item.InspectionID = _InspectionID;
        if (Item.id != 0)
            await _versions.Update(Item);
        else
        {
            await _versions.AddNew(Item);
        }
        await _par.RefreshDataAsync();
        await Navigation.PopAsync();
    }
}