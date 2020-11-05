using System;
using ToDoList.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace ToDoList
{
    public partial class App : Application
    {
        public static string FilePath;
        
        public App()
        {
            InitializeComponent();

            MainPage = new Views.ToDoList();
        }
        
        public App(string filePath)
        {
            InitializeComponent();

            MainPage = new Views.ToDoList();

            FilePath = filePath;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}