using TextEditorMVVM.ViewModels;
using Xamarin.Forms;

namespace TextEditorMVVM.Views
{
    public partial class TextEditor : ContentPage
    {
        public TextEditor()
        {
            #region XAML
            //Label labelMain = new Label()
            //{
            //    Text = "Мой первый текстовый редактор на Xamarin MVVM",
            //    HorizontalOptions = LayoutOptions.Center,
            //    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            //    HorizontalTextAlignment = TextAlignment.Center,
            //    TextColor = Color.Black
            //};
            //Entry pathTextFile = new Entry()
            //{
            //    Placeholder = "Путь к текстовому файлу",
            //    HorizontalTextAlignment = TextAlignment.Center
            //};

            //Button openTextFileButton = new Button()
            //{
            //    Text = "Открыть файл",
            //    BackgroundColor = Color.Green
            //};
            //openTextFileButton.SetBinding(Button.CommandProperty, "OpenFileCommand");
            //Button closeTextFileButton = new Button()
            //{
            //    Text = "Закрыть файл",
            //    BackgroundColor = Color.Green
            //};
            //closeTextFileButton.SetBinding(Button.CommandProperty, "CloseFileCommand");
            //Button saveTextFileButton = new Button()
            //{
            //    Text = "Сохранить файл",
            //    BackgroundColor = Color.Green
            //};
            //saveTextFileButton.SetBinding(Button.CommandProperty, "SaveFileCommand");

            //Switch switcherCanEditInEditor = new Switch();
            //switcherCanEditInEditor.Behaviors.Add(new SwitchBehavior());
            ////switcherCanEditInEditor.SetBinding(Switch.CommandProperty, "ExecuteSwitchCommand");

            //StackLayout stackLayoutButtons = new StackLayout
            //{
            //    Orientation = StackOrientation.Horizontal,
            //    HorizontalOptions = LayoutOptions.Center,
            //    Children = { openTextFileButton, saveTextFileButton, closeTextFileButton }
            //};
            //var editor = new Editor
            //{
            //    HeightRequest = 200
            //};
            //editor.TextChanged += EditorTextChanged;


            //StackLayout stackLayout = new StackLayout
            //{
            //    Padding = new Thickness(20, 50),
            //    Children = { labelMain, pathTextFile, stackLayoutButtons, editor, switcherCanEditInEditor }
            //};

            //Content = stackLayout;

            #endregion
            InitializeComponent();
            BindingContext = new TextEditorViewModel()
            {
                IsReadOnly = "False",
                FileName = "Новый документ",
                Navigation = this.Navigation 
            };
        }
    }
}