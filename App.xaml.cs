using Microsoft.Maui.Controls;
namespace EXAMEN_TERCER_PARCIAL
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }
    }
}
