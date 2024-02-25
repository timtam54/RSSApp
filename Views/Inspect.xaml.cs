using RssMob.Models;
using RssMob.Services;

namespace RssMob.Views;

public partial class Inspect : ContentPage,iRefreshData
{

    readonly IInspectionRepository _insp;
    readonly IEmployeeRepository _emp = new EmployeeServices();
    readonly IBuildingRepository _bui = new BuildingServices();
    int _InspectionID;
    readonly IInspEquipRepository _inspitems = new InspEquipServices();
    iRefreshData _par;
    int _LoginID;
   // string _BuildingID;
    public Inspect(int InspectionID,  IInspectionRepository insp,iRefreshData par,int LoginID)
    {
        try
        {
            _LoginID = LoginID;
            _insp = insp;
            InitializeComponent();
         
            _InspectionID = InspectionID;
            uploadImage = new UploadImage();
            _par = par;
            RefreshDataAsync();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error.Inspect", "Error.Inspect:" + ex.Message, "OK");
        }
    }

    protected async override void OnDisappearing()
    {
        try {
            if (BindingContext != null)
            {
                Item = (Models.Inspection)BindingContext;
                if (!Item.Equals(Orig) || imagefile != null)
                {
                    if (await DisplayAlert("Unsaved changes", "Data on previous page has been modified", "Save", "Don't Save"))
                    {
                        await SaveData();
                    }
                    //return;
                }
            }
        }
        catch (Exception ex)
        {
           await DisplayAlert("Error", "Error:" + ex.Message, "OK");
        }
        base.OnDisappearing();
    }

    public void NewID(int id)
    {
        Item.SelectBuidingID = Item.Buildings.Where(i => i.Value == id).FirstOrDefault();

    }
    UploadImage uploadImage { get; set; }

    Models.Inspection Item;Models.Inspection Orig;

    public async Task<bool> RefreshDataAsync()
    {
        try { 
        Loading = true;
        Indi.IsVisible = true;
        Indi.IsRunning = true;
        List<Models.Employee> Insps = await _emp.Employees();

        if (_InspectionID == 0)
        {
            Item = new Inspection();
            Item.InspectionDate = DateTime.Now;
                Item.InspectorID = _LoginID;
        }
        else
        {
            Item = await _insp.Inspection(_InspectionID);
        }


       // if (Orig == null)
            Orig = (Models.Inspection)Item.Clone();

        Item.Insps = (from ins in Insps select new SelectListItem { Text = ins.Given + " " + ins.Surname, Value = ins.id }).ToList();
       

  
        //if (_InspectionID == 0)
        //{
        //    Item.InspectorID = Item.Insps.FirstOrDefault().Value;

        //        Item.Inspector2ID = Item.Insps.FirstOrDefault().Value;
        //    }
        Item.SelectInspectorID = Item.Insps.Where(i => i.Value == Item.InspectorID).FirstOrDefault();
            if (Item.Inspector2ID==null)
                Item.Inspector2ID = Item.Insps.FirstOrDefault().Value;
//            else
                Item.SelectInspector2ID = Item.Insps.Where(i => i.Value == Item.Inspector2ID).FirstOrDefault();

            Items = (await _inspitems.InspItems(_InspectionID)).OrderByDescending(i=>i.id).ToList();

        foreach (var item in Items)
        {
            item.Photos = "https://rssblob.blob.core.windows.net/rssimage/" + item.Photos;
        }
        InspEquipList.ItemsSource = Items;
        if (_InspectionID==0)
        {
            Views.SearchClientBuildingAddress searchClientView = new Views.SearchClientBuildingAddress(this, _insp);
            await Navigation.PushAsync(searchClientView);
            searchClientView.Unloaded += SearchClientView_Unloaded;
            //Item.BuildingID = Convert.ToInt32(_BuildingID);
        }
        else
        {
            await SetBuildingID(Item.BuildingID);
        }
     //   BindingContext = Item;
       // Loading = false;
        return true;
        }
        catch (Exception ex)
        {
          await  DisplayAlert("Inspect.Error", "Error.Inspect.RefreshData:" + ex.Message, "OK");
        }
        return false;
    }
    bool BuildingSet = false;
    

    private async void SearchClientView_Unloaded(object sender, EventArgs e)
    {
        if (!BuildingSet)
      
        {
           await Navigation.PopAsync();
         await   Navigation.PopAsync();

        }
    }

   

    public async Task SetBuildingID(int BuildingID)
    {
        try { 
        BuildingSet = true;
        Item.BuildingID = BuildingID;
        List<Models.Building> Bui = new List<Models.Building>();
        Bui.Add(await _bui.Building(BuildingID));
        Title = "Inspection of " + Bui.FirstOrDefault().Address;
        Item.Buildings = (from bd in Bui select new SelectListItem { Text = bd.BuildingName, Value = bd.id }).ToList();
        Item.SelectBuidingID = Item.Buildings.Where(i => i.Value == Item.BuildingID).FirstOrDefault();


        BindingContext = Item;
        Loading = false;
        Indi.IsVisible = false;
        Indi.IsRunning = false;
        }
        catch (Exception ex)
        {
          await  DisplayAlert("Error", "Error:" + ex.Message, "OK");
        }
    }
    List<Models.InspEquipView> Items;
  

    void Image_Clicked(System.Object sender, System.EventArgs e)
    {
        TakePhoto();
    }
    async void Button_ItemEdit(System.Object sender, System.EventArgs e)
    {
        Button btn = (Button)sender;
        var InspEquipID = Convert.ToInt32(btn.CommandParameter);
        var ie=Items.Where(i => i.id == InspEquipID).FirstOrDefault();
        await Navigation.PushAsync(new Views.InspEquip(this, InspEquipID, _inspitems, _InspectionID,ie.EquipDesc));
    }
    async void Button_ItemAddNew(System.Object sender, System.EventArgs e)
    {
        if (_InspectionID == 0)
        {
            ;//message press save first
        }
        else
        {
            await Navigation.PushAsync(new Views.InspEquip(this, 0, _inspitems, _InspectionID, ""));
        }
    }
    async void Button_ItemDelete(System.Object sender, System.EventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        var InspEquipID = btn.CommandParameter;
        await _inspitems.Delete(Convert.ToInt32(InspEquipID));
        await RefreshDataAsync();
    }

    FileResult img = null;
    public async void TakePhoto()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            img = await MediaPicker.Default.CapturePhotoAsync();
            if (img != null)
            {
                imagefile = await uploadImage.GetImageFile(img,true);
                Image_Upload.Source = ImageSource.FromStream(() => uploadImage.ByteArrayStream(uploadImage.StringToByteBase64(imagefile.byteBase64)));
            }
        }
    }

    ImageFile imagefile;
    async void Gallery_Clicked(object sender, EventArgs e)
    {
         img = await uploadImage.OpenMediaPickerAsync();
        if (img != null)
        {
            imagefile = await uploadImage.GetImageFile(img,false);
            Image_Upload.Source = ImageSource.FromStream(() => uploadImage.ByteArrayStream(uploadImage.StringToByteBase64(imagefile.byteBase64)));
         }
    }
    async void Button_Save(System.Object sender, System.EventArgs e)
    {
        ActivityIndicator activityIndicator = (ActivityIndicator)Indi;
        activityIndicator.IsRunning = true;
        await SaveData();
        await Application.Current.MainPage.Navigation.PopAsync();
        //  RefreshDataAsync();
    }

    private async Task SaveData()
    {
        try { 
        Indi.IsVisible = true;
        Indi.IsRunning = true;
        Models.Inspection Item = (Models.Inspection)BindingContext;
        Item.InspectorID = Item.SelectInspectorID.Value;
         if (Item.SelectInspector2ID!=null)
            Item.Inspector2ID = Item.SelectInspector2ID.Value;

            Item.BuildingID = Item.SelectBuidingID.Value;
        if (Item.id != 0)
            await _insp.Update(Item);
        else
        {
            await SaveNewInspDetails();
        }
        if (imagefile != null)
        {
            Indi.IsRunning = true;
            Indi.IsVisible = true;
            lblUpload.IsVisible = true;
            imagefile = await uploadImage.UploadToServer(imagefile, 0, _InspectionID);
            await _par.RefreshDataAsync();
            Indi.IsRunning = false;
            Indi.IsVisible = false;
            lblUpload.IsVisible = false;
                imagefile = null;
        }
        BindingContext = Item;
        Orig = (Models.Inspection)Item.Clone();
        Indi.IsVisible = false;
        Indi.IsRunning = false;
        }
        catch (Exception ex)
        {
          await  DisplayAlert("Error", "Error:" + ex.Message, "OK");
        }
    }

    public  async Task SaveNewInspDetails()
    {
        _InspectionID = (await _insp.AddNew(Item)).id;
    }
    async void Button_Versions(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new Views.InspVersions(Convert.ToInt32(Item.BuildingID),_insp));
    }

    bool Loading = true;
   
    async void BuildingID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Loading)
            return;
        Picker pk = (Picker)sender;
        var ss = ((SelectListItem)pk.SelectedItem).Text;
        pk.Unfocus();
        if (ss == "-Add New-")
        {
            await Navigation.PushAsync(new Views.Building(this, 0,_bui,_insp));
        }
    }

    async void Print_Clicked(object sender, EventArgs e)
    {
        try
        {
            string id = ((ImageButton)sender).CommandParameter.ToString();
            Uri uri = new Uri("https://roofsafetysolutions.azurewebsites.net/InspectionEquipments/EquipForInspectionsAll/" +id);
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            // An unexpected error occurred. No browser may be installed on the device.
        }
    }

    async void Delete_Clicked(object sender, EventArgs e)
    {
         await _insp.Delete(_InspectionID);
      await  _par.RefreshDataAsync();
     
       await Navigation.PopAsync();
    }

    private async void Search(object sender, EventArgs e)
    {
        Views.SearchClientBuildingAddress searchClientView = new Views.SearchClientBuildingAddress(this, _insp);
        await Navigation.PushAsync(searchClientView);
    }



    private void ImageButtonDelete_Clicked(object sender, EventArgs e)
    {
        Item.Photo = null;
        Image_Upload.Source = null;
        //Item.PhotoURL = null;
    }
}