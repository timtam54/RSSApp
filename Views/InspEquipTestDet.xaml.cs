using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using RssMob.Models;
using RssMob.Services;
using System.Collections.Generic;

namespace RssMob.Views;

public partial class InspEquipTestDet : ContentPage, iRefreshData
{
    iRefreshData _par;
    int _id;
    int _EquipTypeID;
    IInspEquipTestRepository _client;
    int _InspEquipID;
    string _EquipTypeName;
    private async void entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try { 
        search = e.NewTextValue;
        if (search == null)
            return;
        List<SelectListItem> _Items = Item.Ett.Where(i=>i.Text.ToLower().Contains(search.ToLower())).ToList();

        InspectionList.ItemsSource = _Items;

        }
        catch (Exception ex)
        {
            lblFail.Text = ex.Message;
        }
    }

    string search = "";

    public InspEquipTestDet(iRefreshData par, int id, IInspEquipTestRepository client, int EquipTypeID, int InspEquipID, string EquipTypeName)//, int? EquipTypeTestID)
    {
        _par = par;
        _id = id;
        _client = client;
        _InspEquipID = InspEquipID;
        _EquipTypeID = EquipTypeID;
        _EquipTypeName = EquipTypeName;
        InitializeComponent();
       // chkPass.SelectedIndexChanged += ChkPass_SelectedIndexChanged;
        Title = "Tests for " + EquipTypeName;

        RefreshDataAsync();
        
    }

   

    IEquipTypeTestFailRepository _clientETTFail = new EquipTypeTestFailServices();
    IInspEquipTestFailRepository _clientIETTFail = new InspEquipTestFailService();
    IETTestHazardRepository _clientETTH = new ETTestHazardService();
    List<Models.EquipTypeTestHazards> Its;
    List<Models.EquipTypeTestFail> fails;
    List<Models.InspEquipTypeTestFail> inspfails;
    bool Loading = true;
    IEquipTypeRepository equiptypeservice = new EquipTypeServices();
    IEquipTypeTestRepository equiptypetestservice = new EquipTypeTestServices();
    Models.InspEquipTypeTest Item; Models.InspEquipTypeTest Orig;
    public async Task<bool> RefreshDataAsync()
    {
        try
        {
            Loading = true;
            Indi.IsVisible = true;
            Indi.IsRunning = true;
            if (_id == 0)
            {
                Item = new InspEquipTypeTest();
                Item.InspEquipID = _InspEquipID;
                Item.id = 0;
                //Item.Pass = 1;
                Its = new List<EquipTypeTestHazards>();
                // btnAddHaz.IsVisible = false;
            }
            else
            {
                Item = await _client.InspTest(_id);
                PopFailReasons();
            }

            //if (Orig == null)
                Orig = (Models.InspEquipTypeTest)Item.Clone();


            /*SelectListItem slFail = new SelectListItem();
            slFail.Value = 0; slFail.Text = "Fail";
            Item.PFCSs.Add(slFail);
            SelectListItem slPass = new SelectListItem();
            slPass.Value = 1; slPass.Text = "Pass";
            Item.PFCSs.Add(slPass);
            SelectListItem slRec = new SelectListItem();
            slRec.Value = 2; slRec.Text = "Recommendation";
            Item.PFCSs.Add(slRec);
            if (Item.Pass == 0)
                Item.SelPass = slFail;
            else if (Item.Pass == 1)
                Item.SelPass = slPass;
            else
                Item.SelPass = slRec;*/

            List<Models.EquipTypeTest> equiptypetest = await equiptypetestservice.EquipTypeTests(_EquipTypeID);
            Item.Ett = (from ins in equiptypetest orderby ins.Test select new SelectListItem { Text = ins.Test, Value = ins.id }).ToList();
            SetVisSelectETT(equiptypetest);
            SelectListItem sli = new SelectListItem();
            sli.Text = "-Add New-";
            sli.Value = 0;
            Item.Ett.Add(sli);
          
            List<SelectListItem> _Items = Item.Ett.ToList();

            InspectionList.ItemsSource = _Items;

            BindingContext = Item;
            setvis(0);// Item.Pass);
           // Item.Ps = (object)Item.Pass;
           // if (Item.Pass == 0)
           //     chkFail.IsChecked = true;
           // else if (Item.Pass == 1)
           //     chkPass.IsChecked = true;
           // else
            //    chkRec.IsChecked = true;
        }
        catch (Exception ex)
        {
            lblFail.Text = ex.Message;
        }
        finally
        {
            Loading = false;
            Indi.IsVisible = false;
            Indi.IsRunning = false;
        }
        return true;
    }
    protected async override void OnDisappearing()
    {
        Item = (Models.InspEquipTypeTest)BindingContext;
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
    private void SetVisSelectETT(List<Models.EquipTypeTest> equiptypetest)
    {
        try { 
        if (_id != 0)
        {
            Item.EquipTypeTestDesc = GetEquipTypeTestDesc(Item.EquipTypeTestID, equiptypetest);
            lblett.IsVisible = true;
            ddlett.IsVisible = false;
            ddlett2.IsVisible = false;
            ddlett3.IsVisible = false;

        }
        else
        {
            Item.EquipTypeTestDesc = "not selected yet";
            lblett.IsVisible = false;
            ddlett.IsVisible = true;
            ddlett2.IsVisible = true;
            ddlett3.IsVisible = true;
        }
        }
        catch (Exception ex)
        {
            lblFail.Text = ex.Message;
        }
    }
    public void UncheckAllFails()
    {
        List<Models.EquipTypeTestFailInsp> Items = Item.EETFs;
       // InspectionList
    }
    string GetEquipTypeTestDesc(int SelEquipTypeTestID, List<Models.EquipTypeTest> Items)
    {
        try { 
        foreach (Models.EquipTypeTest item in Items)
        {
            if (item.id == SelEquipTypeTestID)
                return item.Test;
        }
        }
        catch (Exception ex)
        {
            lblFail.Text = ex.Message;
        }
        return "Not Found";
    }

  //  List<EquipTypeTestFailInsp> xx;
    async void PopFailReasons()
    {
        try
        {
            fails = await _clientETTFail.EquipTypeTestFails(Item.EquipTypeTestID);
            inspfails = await _clientIETTFail.InspEquipTypeTestFails(Item.id);
            Item.EETFs = new List<EquipTypeTestFailInsp>();
            foreach (var fl in fails)
            {
                var x = new EquipTypeTestFailInsp();
                x.id = fl.id;
                x.EquipTypeTestID = fl.EquipTypeTestID;
                x.FailReason = fl.FailReason;
                var infa = inspfails.Where(i => i.EquipTypeTestFailID == fl.id).Count();
             /*   if (infa == 0)
                {
                    if (x.Failed==true)
                        x.Failed = false;
                    //                    x.Failed = false;
                }
                else
                {
                    if (x.Failed == false)
                        x.Failed =true;
//                    x.Failed = true;
                }*/
                Item.EETFs.Add(x);
            }
            failList.ItemsSource = Item.EETFs;
            //failList2.ItemsSource = Item.EETFs;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error - pop fail reasons",ex.Message,"Cancel");
            lblFail.Text=ex.Message;
        }
    }
    public void NewID(int id)
    {
       var xx = Item.Ett.Where(i => i.Value == id).FirstOrDefault();
        //Item.SelEquipTypeTest = xx;
    }
    async void Button_Save(System.Object sender, System.EventArgs e)
    {
        await SaveData();
        await _par.RefreshDataAsync();
        await Application.Current.MainPage.Navigation.PopAsync();
    }

    private async Task SaveData()
    {
        ddlett3.IsVisible = false;
        Indi.IsVisible = true;
        Indi.IsRunning = true;
        Item = (Models.InspEquipTypeTest)BindingContext;

       // Item.Pass = Convert.ToInt32(Item.Ps);
       /* if (Item.Pass==0)
        {
            if (!AnyTestFailed())
            {
                await DisplayAlert("Issue","The test is marked as failed yet there are no fail reasons selected - please select a fail reason or mark test as pass/recommendation","ok");
                return;
            }
        }*/
        if (Item.id != 0)
            await _client.Update(Item);
        else
            Item = await _client.AddNew(Item);
        if (failList.ItemsSource != null)
        {

            List<EquipTypeTestFailInsp> xx = (List<EquipTypeTestFailInsp>)Item.EETFs;// failList.ItemsSource;
            foreach (var x in xx)
            {
               // if (Item.Pass==0)
                   // x.Failed = true;
              //  else
                //    x.Failed = false;   

                var infa = inspfails.Where(i => i.EquipTypeTestFailID == x.id).FirstOrDefault();
                if (infa == null)
                {
                   // if (!x.Failed == true)
                    //{
                        ; //do nothing
                   // }
                  //  else
                   // {
                        InspEquipTypeTestFail add = new InspEquipTypeTestFail();
                        add.EquipTypeTestFailID = x.id;
                        add.InspEquipTypeTestID = Item.id;// _id;
                        await _clientIETTFail.AddNew(add);
                        ;//do somthing
                   // }
                }
                else
                {
                   // if (!x.Failed == true)
                   // {
                  //      await _clientIETTFail.Delete(infa.id);
                   // }
                   // else
                   // {
                        ; //do nothing
                  //  }
                }
            }
        }
        BindingContext = Item;
        Orig = (Models.InspEquipTypeTest)Item.Clone();
        Indi.IsVisible = false;
        Indi.IsRunning = false;
    }


    /*  async void EquipTypeTestID_SelectedIndexChanged(System.Object sender, System.EventArgs e)
      {
          if (Loading)
              return;
          Picker pk = (Picker)sender;
          if (pk.SelectedItem == null)
              return;
          var si = ((SelectListItem)pk.SelectedItem);
          var ss=si.Text;
          pk.Unfocus();
          if (ss == "-Add New-")
          {
              await Navigation.PushAsync(new Views.EquipTypeTest(this, _EquipTypeName, _EquipTypeID, Convert.ToInt32(Item.EquipTypeTestID)));
         }
          else
          {
              Item.EquipTypeTestID = Convert.ToInt32( si.Value);
              //= _EquipTypeTestID.Value=Item.EquipTypeTestID;
              PopFailReasons();
          }
          setvis(chkPass);
      }*/

    async void AllTest_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.TestsforEquipType( _EquipTypeID, _EquipTypeName));

        // await equiptypetestservice.EquipTypeTests(_EquipTypeID);
    }

    async void btnETT_Clicked(object sender, EventArgs e)
    {
        Models.InspEquipTypeTest Item = (Models.InspEquipTypeTest)BindingContext;
        await Navigation.PushAsync(new Views.EquipTypeTest(this, _EquipTypeName, _EquipTypeID, Convert.ToInt32(Item.EquipTypeTestID)));

    }

    async void Button_Clicked(object sender, EventArgs e)
    {
        if (Loading)
            return;
        Button pk = (Button)sender;
        var si = ((SelectListItem)pk.CommandParameter);
        var ss = si.Text;
        pk.Unfocus();
        if (ss == "-Add New-")
        {
            await Navigation.PushAsync(new Views.EquipTypeTest(this, _EquipTypeName, _EquipTypeID, Convert.ToInt32(Item.EquipTypeTestID)));
        }
        else
        {
            Item.EquipTypeTestID = Convert.ToInt32(si.Value);
            await SaveData();
            await _par.RefreshDataAsync();
            await Application.Current.MainPage.Navigation.PopAsync();
        }
       // setvis(chkPass);
       

    }

    

    private async void FailReasonAdded(object sender, EventArgs e)
    {
        try
        {
            EquipTypeTestFail fl = new EquipTypeTestFail();
            fl.FailReason = txtFailReason.Text;
            fl.EquipTypeTestID = Item.EquipTypeTestID;// _EquipTypeTestID;

            await _clientETTFail.AddNew(fl);
            PopFailReasons();// RefreshDataAsync();
            txtFailReason.Text = "";
        }
        catch (Exception ex)
        {
            lblFail.Text = ex.Message;
        }
    }

    private async Task FailReasonAddedTask(object sender, EventArgs e)
    {
        try { 
        EquipTypeTestFail fl = new EquipTypeTestFail();
        fl.FailReason = txtFailReason.Text;
        fl.EquipTypeTestID = Item.EquipTypeTestID;//Item.EquipTypeTestID

        await _clientETTFail.AddNew(fl);
        await RefreshDataAsync();
        txtFailReason.Text = "";
        }
        catch (Exception ex)
        {
            lblFail.Text = ex.Message;
        }
    }



  /*  bool AnyTestFailed()
    {
        try {
            if (failList.ItemsSource == null) return false;
        List<EquipTypeTestFailInsp> xx = (List<EquipTypeTestFailInsp>)failList.ItemsSource;
        foreach (var x in xx)
        {
           // if (Item.Pass)
           //     x.Failed = false;
          //  var infa = inspfails.Where(i => i.EquipTypeTestFailID == x.id).FirstOrDefault();
         ///   if (infa == null)
           // {
               // if (!x.Failed == true)
               // {
              //      ; //do nothing
              //  }
              //  else if (x.Failed == null)
              //  {
             //       ; //do nothing
            //    }
             //   else
                {
                    return true;
                }
          //  }
          //  else
          //  {
          //      if (!x.Failed == true)
          //      {
          //          ;
           //     }
           //     else
           //     {
           //         return true;
          //      }
         //   }
        }
        }
        catch (Exception ex)
        {
            lblFail.Text = ex.Message;
        }
        return false;
    }*/

    void setvis(int cb)
    {
        try { 
        bool EquipTypeTestIDExists = true;
        if (Item == null)
            EquipTypeTestIDExists = false;
        else
        {
            EquipTypeTestIDExists = (Item.EquipTypeTestID != 0);
        }
           /* if (cb.Equals(0))
                lblFail.Text = "Fail Reason";
            else if (cb.Equals(1))
                lblFail.Text = "Comment";
            else
                lblFail.Text = "Recommendation";*/
            //AddFail.IsVisible = cb.Equals(0) && EquipTypeTestIDExists;
            failList.IsVisible = true;// AddFail.IsVisible;
            //failList2.IsVisible = !AddFail.IsVisible;
        }
        catch (Exception ex)
        {
            lblFail.Text = ex.Message;
        }
    }
    private void CheckBox_Focused(object sender, FocusEventArgs e)
    {

    }

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {

      //  private void ChkPass_SelectedIndexChanged(object sender, System.EventArgs e)
   
        if (Loading) return;
        try
        {
               Item = (Models.InspEquipTypeTest)BindingContext;
            //Item.Pass = Convert.ToInt32(Item.Ps);
                //  CheckBox cb = sender as CheckBox;
              //  Item.Pass = ((SelectListItem)chkPass.SelectedItem).Value;
            /*if (Item.Pass!=0)
            {
                if (AnyTestFailed())
                {
                     DisplayAlert("Alert", "Some fail reasons are checked - these will be unchecked as test is marked as passed", "OK");

                }


                if (Item.EETFs != null)
                {
                    foreach (var item in Item.EETFs)
                    {
                        item.Failed = false;
                    }
                }
                failList.ItemsSource = Item.EETFs;
                
                //failList.BindingContext = Item.EETFs;
                if (failList.ItemsSource != null)
                {
                    List<EquipTypeTestFailInsp> xx = (List<EquipTypeTestFailInsp>)failList.ItemsSource;
                    foreach (var x in xx)
                    {
                        x.Failed = false; ;
                    }
                }
            }*/
            setvis(0);// Item.Pass);
        }
        catch (Exception ex)
        {
           
            lblFail.Text = ex.Message;
        }
    
    }

   

    
}