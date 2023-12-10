namespace RssMob;

public partial class App : Application
{
    public App(LoginPage loginpage)
    {
		InitializeComponent();

        MainPage = new NavigationPage(loginpage);
    }
}
