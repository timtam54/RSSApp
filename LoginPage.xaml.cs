using Microsoft.Maui.ApplicationModel.Communication;
using RssMob.Models;
using RssMob.Services;
using RssMob.Views;

namespace RssMob;

public partial class LoginPage : ContentPage
{
    readonly EmployeeDatabase _rssDatabase;

    readonly ILoginRepository _loginRepository = new LoginServices();

    public LoginPage(EmployeeDatabase rssDatabase)//, Dashboard dashboard
    {
        //_dashboard = dashboard;
        InitializeComponent();
        DteTo = DateTime.Now.AddMonths(1);
        DteFrom = DateTime.Now.AddMonths(-3);
        Loaded += LoginPage_Loaded;
        _rssDatabase = rssDatabase;

    }
  public  async void logout()
    {
        if (emps == null)
            return;
        if (emps.Count() == 0)
            return;
       await _rssDatabase.DeleteItemAsync(emps.FirstOrDefault());
    }
    List<Employee> emps;
    private async void LoginPage_Loaded(object sender, EventArgs e)
    {
        emps = await _rssDatabase.GetItemsAsync();
        if (emps != null)
        {
            if (emps.Count != 0)
            {
                emps.FirstOrDefault().id = 1;
                txtEmail.Text = emps.FirstOrDefault().Email;
                txtPassword.Text = emps.FirstOrDefault().Password;
                Employee employee = await _loginRepository.Login(txtEmail.Text, txtPassword.Text);
                if (employee != null)
                {
                    await RefreshDataAsync();
                    await Navigation.PushAsync(new RssMap(this,employee.id));
                }
            }
        }
    }
    public async void OpenMap()
    {
         await Navigation.PushAsync(new Views.RssMap(this,employee.id));
    }

    public async Task Logout()
    {
        logout();
        await Navigation.PopAsync();
    }

    public async void OpenInsp(int LoginID)
    {
        // await Navigation.PushAsync(new Views.RssMap(this));
        await Navigation.PushAsync(new Views.Inspections(this, LoginID));
    }
    public readonly IInspectionRepository _client = new InspectionServices();
    public List<Models.InspectionView> Items;
    public async Task<bool> RefreshDataAsync()
    {
        Items = await _client.Inspections(search,DteFrom,DteTo);
        foreach (var item in Items)
        {
            item.Photo = "https://rssblob.blob.core.windows.net/rssimage/" + item.Photo;
        }
        return true;
    }
    
    public string search = "";
    public DateTime DteFrom;
    public DateTime DteTo;

    public Employee employee;
    async void btnLogin_Clicked(System.Object sender, System.EventArgs e)
    {
        string email = txtEmail.Text;
        string password = txtPassword.Text;
        if (email == null || password == null)
        {
            await DisplayAlert("Warning", "Please enter email and password", "ok");
            return;
        }
        employee = await _loginRepository.Login(email, password);
        if (employee != null)
        {
            var exists = emps.Where(i => i.Email == email).Count();
            if (exists == 0)
                //todo see if this exist in the database and if not then 
                employee.id = 0;
            await _rssDatabase.SaveItemAsync(employee);
            await RefreshDataAsync();
            await Navigation.PushAsync(new RssMap(this, employee.id));
        }
        else
        {
            await DisplayAlert("Warning", "Login failed", "ok");
        }
    }
}