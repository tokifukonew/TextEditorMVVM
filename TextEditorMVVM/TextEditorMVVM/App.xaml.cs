using System;
using TextEditorMVVM.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TextEditorMVVM
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new TextEditor();
            //MainPage = new MainPage();
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
