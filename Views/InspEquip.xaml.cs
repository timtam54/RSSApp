using RssMob.Models;
using RssMob.Services;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Schema;

namespace RssMob.Views;

public partial class InspEquip : ContentPage,iRefreshData
{
    IInspEquipRepository _inspitems;
    iRefreshData _par;
    int _id;
    int _InspectionID;
    string _EquipType;
    public  InspEquip(iRefreshData par, int id, IInspEquipRepository inspitems, int InspectionID,string EquipType)
    {
        try
        {
            _par = par;
            _InspectionID = InspectionID;
            _inspitems = inspitems;
            _id = id;
            _EquipType = EquipType;
            InitializeComponent();
            RefreshDataAsync();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error",ex.Message,"OK");
        }
    }
    protected async override  void OnDisappearing()
    {
        Item = (Models.InspEquip)BindingContext;
        if (!Item.Equals(Orig))
        {
            if (await DisplayAlert("Unsaved changes", "Data on previous page has been modified", "Save", "Don't Save"))
            {
                await SaveData();
            }
            //return;
        }
        base.OnDisappearing();
    }
 

    public void NewID(int id)
    {
        ;
    }
    IInspPhotoRepository ip = new InspPhotoServices();
    Models.InspEquip Item; Models.InspEquip Orig;
    List<Models.InspPhoto> Items;
    bool Loading = true;
    public async Task<bool> RefreshDataAsync()
    {
        try
        {
            Loading = true;
            Indi.IsRunning = true;
            Indi.IsVisible = true;
  
            if (_id == 0)
            {
                Item = new Models.InspEquip();
                Item.InspectionID = _InspectionID;
            }
            else
                Item = await _inspitems.InspItem(_id);
            //if (Orig ==null)
                Orig = (Models.InspEquip)Item.Clone();
            List<EquipType> equiptype = await (new EquipTypeServices()).EquipTypes();
            List<Models.InspEquipTypeTestRpt> It;
            Item.Et = (from ins in equiptype orderby ins.EquipTypeDesc select new SelectListItem { Text = ins.EquipTypeDesc, Value = ins.id }).ToList();
            if (_id == 0)
            {
                Item.SelEquipType = Item.Et.FirstOrDefault();
                EquipTypeName = Item.SelEquipType.Text;
                Items = new List<InspPhoto>();
                InspPhotosCarousel.IsVisible = false;
                InspPhotosCarousel.ItemsSource = Items;
                It = new List<InspEquipTypeTestRpt>();
                Item.InspEquipTypeTests = It;
                InspectionList.ItemsSource = It;
                InspectionList.IsVisible = false;
                btnAddTest.IsVisible = false;
                btnAddPhoto.IsVisible = false;
                AddNewTestRow.IsVisible = false;
            }
            else
            {
                Item.SelEquipType = Item.Et.Where(i => i.Value == Item.EquipTypeID).FirstOrDefault();
                EquipTypeName = Item.SelEquipType.Text;
                Items = await ip.InspPhotos(_id);
                foreach (var item in Items)
                {
                    item.photoname = "https://rssblob.blob.core.windows.net/rssimage/" + item.photoname;
                }
                InspPhotosCarousel.IsVisible = true;
                InspPhotosCarousel.ItemsSource = Items;
                It = await _client.InspTests(_id);
                Item.InspEquipTypeTests = It;
                InspectionList.ItemsSource = It;
                InspectionList.IsVisible = true;
                btnAddTest.IsVisible = true;
                btnAddPhoto.IsVisible = true;
                AddNewTestRow.IsVisible = true;
            }
            BindingContext = Item;
            Indi.IsRunning = false;
            Indi.IsVisible = false;
            //lblUpload.IsVisible = false;
            return true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error",ex.Message,"Cancel");
            return false;
        }
        finally
        {
            Loading = false;
        }
    }
    IInspEquipTestRepository _client = new InspEquipTestService();
    async void Button_PhotoAdd(System.Object sender, System.EventArgs e)
    {
        try { 
        await Navigation.PushAsync(new Photo(_id, this));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Cancel");
            
        }
    }

    async void Image_Clicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Photo(_id, this));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Cancel");

        }
    }
        async void Gallery_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Photo(_id, this));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Cancel");

        }
    }
    string EquipTypeName;
    //async void Photos_Clicked(System.Object sender, System.EventArgs e)
    //{
    //    await Navigation.PushAsync(new Views.InspEquipPhotos(_id));// Convert.ToInt32(_ID)));
    //}
    async void Button_PhotoDelete(object sender, EventArgs e)
    {
        try { 
        ImageButton btn = (ImageButton)sender;
        await ip.Delete(Convert.ToInt32(btn.CommandParameter));
       await RefreshDataAsync();
        }
        catch (Exception ex)
        {
           await  DisplayAlert("Error - Button_PhotoDelete", ex.Message, "OK");
        }
    }
    

    async void SaveClose_Clicked(System.Object sender, System.EventArgs e)
    {
       await SaveData();
        Orig = (Models.InspEquip)Item.Clone();
        await Application.Current.MainPage.Navigation.PopAsync();
    }

    async Task SaveData()
        {
        Indi.IsVisible = true;
        Indi.IsRunning = true;
        try
        {
            Models.InspEquip Item = (Models.InspEquip)BindingContext;
            Item.EquipTypeID = Item.SelEquipType.Value;
            if (Item.id != 0)
                await _inspitems.Update(Item);
            else
                await _inspitems.AddNew(Item);
            BindingContext = Item;
            await _par.RefreshDataAsync();
           
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error - SaveClose_Clicked", ex.Message, "OK");
        }
        finally
        {
            Indi.IsVisible = false;
            Indi.IsRunning = false;
        }
    }

    async void Button_InspTest(System.Object sender, System.EventArgs e)
    {
        try { 
        Button btn = (Button)sender;
        var id = btn.CommandParameter;
        var insp = Item.InspEquipTypeTests.Where(i => i.id == Convert.ToInt32(id)).FirstOrDefault();
        await Navigation.PushAsync(new Views.InspEquipTestDet(this, Convert.ToInt32(id)/*insp.InspEquipID*/, _client, Item.EquipTypeID, _id, Item.SelEquipType.Text));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error - Button_InspTest", ex.Message, "OK");
        }
    }

    //add new




    async void Button_InspTestDelete(System.Object sender, System.EventArgs e)
    {
        try
        {
            Button btn = (Button)sender;
            var id = btn.CommandParameter;
            await _client.Delete(Convert.ToInt32(id));
            await RefreshDataAsync();
        }
        catch (Exception ex)
        {
            var ff = ex;
        }
    }

    async void Button_InspTestAddNew(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new Views.InspEquipTestDet(this, Convert.ToInt32(0), _client, Item.SelEquipType.Value, _id,Item.SelEquipType.Text));

    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {

    }

    private void ImageButton_Clicked_1(object sender, EventArgs e)
    {

    }

    private void ImageButton_Clicked_2(object sender, EventArgs e)
    {

    }

    private void ImageButton_Clicked_3()
    {

    }

    private void Location_TextChanged(object sender, TextChangedEventArgs e)
    {
        
        GetSN();
        //if (Item.SerialNo != null)
        {
            //if (Item.SerialNo != "")
            {
            }
        }
    }

    private void Entry_Completed(object sender, EventArgs e)
    {

    }

    void GetSN()
    {
        if (Loading) return;//loading
                            //if (Item.id != 0)
                            //  return;

        if ((Item.SelEquipType == null) && (Item.Location == null || Item.Location == "" || Item.Location.Length == 0))
            return;
        Item.SerialNo = "";
        if (Item.SelEquipType == null)
            ;
        else
        {
            if (!Item.SelEquipType.Text.Contains(" "))
                Item.SerialNo = Item.SelEquipType.Text.Substring(0, 1);
            else
            {
                // Item.SerialNo = "";
                string[] words = Item.SelEquipType.Text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    if (word.Length > 0)
                        Item.SerialNo = Item.SerialNo + word.Substring(0, 1);
                }
            }
        }
        if (Item.Location == null || Item.Location == "" || Item.Location.Length == 0)
            ;
        else
        {
            if (!Item.Location.Contains(" "))
                Item.SerialNo = Item.Location.Substring(0, 1);
            else
            {
                //Item.SerialNo = "";
                string[] words = Item.Location.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    if (word.Length > 0)
                        Item.SerialNo = Item.SerialNo + word.Substring(0, 1);
                }
            }
        }
        this.SerialNo.Text = Item.SerialNo;
    }

    private void EquipTypeID_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        GetSN();
    }
}