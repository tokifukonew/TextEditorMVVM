using TextEditorMVVM.Views;
using Xamarin.Forms;

namespace TextEditorMVVM
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new TextEditor());
            //MainPage = new TextEditor();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
