using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEditorMVVM.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TextEditorMVVM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectFile : ContentPage
    {
        public SelectFile()
        {
            InitializeComponent();
            //var vm = new FileManagerViewModel();
            BindingContext = new FileManagerViewModel();
        }
    }
}