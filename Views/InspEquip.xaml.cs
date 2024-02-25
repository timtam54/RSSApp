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
            DisplayAlert("Error.InspEquip", "Error.InspEquip:" + ex.Message,"OK");
        }
    }
    protected async override  void OnDisappearing()
    {
        try { 
        Item = (Models.InspEquip)BindingContext;
        if (!Item.Equals(Orig))
        {
            if (await DisplayAlert("Unsaved changes", "Data on previous page has been modified", "Save", "Don't Save"))
            {
                await SaveData();
            }
            
        }
        }
        catch (Exception ex)
        {
           await DisplayAlert("Error.InspEquip", "Error.InspEquip.OnDisappearing:" + ex.Message, "OK");
        }
        base.OnDisappearing();
    }
 

    public void NewID(int id)
    {
        Item.EquipTypeID=id;
        setEquipTypeID(id);
      //  Item.SelEquipType=text;
    }
    IInspPhotoRepository ip = new InspPhotoServices();
    Models.InspEquip Item; Models.InspEquip Orig;
    List<Models.InspPhoto> Items;
    bool Loading = true;
    List<EquipType> equiptype;
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

                Orig = (Models.InspEquip)Item.Clone();
            equiptype = await (new EquipTypeServices()).EquipTypes();
            List<Models.InspEquipTypeTestRpt> It;
            //Item.Et = (from ins in equiptype orderby ins.EquipTypeDesc select new SelectListItem { Text = ins.EquipTypeDesc, Value = ins.id }).ToList();
            if (_id == 0)
            {
                Item.SelEquipType =equiptype.FirstOrDefault().EquipTypeDesc;
             //   EquipTypeName = Item.SelEquipType.Text;
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
                Item.SelEquipType = equiptype.Where(i => i.id == Item.EquipTypeID).FirstOrDefault().EquipTypeDesc;
//                EquipTypeName = Item.SelEquipType.Text;
                Items = await ip.InspPhotos(_id, "I");
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
            await DisplayAlert("Error.InspEquip", "Error.InspEquip.RefreshDataAsync:" + ex.Message,"Cancel");
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
        await Navigation.PushAsync(new Photo(_id, this, Photo.PhotoType.InspEquip));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.InspEquip", "Error.InspEquip.Button_PhotoAdd:"+ex.Message, "Cancel");
            
        }
    }

    async void Image_Clicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Photo(_id, this, Photo.PhotoType.InspEquip));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error.InspEquip", "Error.InspEquip.Image_Clicked:" + ex.Message, "Cancel");

        }
    }
        async void Gallery_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Photo(_id, this, Photo.PhotoType.InspEquip));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Cancel");

        }
    }
   // string EquipTypeName;
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
          
            Item = (Models.InspEquip)BindingContext;
            //Item.EquipTypeID = Item.SelEquipType.Value;
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
        await Navigation.PushAsync(new Views.InspEquipTestDet(this, Convert.ToInt32(id)/*insp.InspEquipID*/, _client, Item.EquipTypeID, _id, Item.SelEquipType));
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
        await Navigation.PushAsync(new Views.InspEquipTestDet(this, Convert.ToInt32(0), _client, Item.EquipTypeID, _id,Item.SelEquipType));

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
            if (!Item.SelEquipType.Contains(" "))
                Item.SerialNo = Item.SelEquipType.Substring(0, 1);
            else
            {
                // Item.SerialNo = "";
                string[] words = Item.SelEquipType.Split(" ", StringSplitOptions.RemoveEmptyEntries);
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

    //private void EquipTypeID_SelectedIndexChanged(object sender, EventArgs e)
    //{
       
    //    GetSN();
    //}
    public void setEquipTypeID(int id)
    {
      //  Item.SelEquipType = Item.Et.Select(o => o.Value == id).FirstOrDefault();
        Item.SelEquipType = equiptype.Where(i => i.id == id).FirstOrDefault().EquipTypeDesc;
        SelEquipType.Text = Item.SelEquipType;
    }
    private async void btnSelEquipType_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EquipTypeSelect(this, equiptype));
        //await Navigation.PushAsync(new EquipTypeView( this ));
    }
}