using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FileFolders.Model;
using Xamarin.Forms;
using SQLite;
using FileFolders.Model;
namespace FileFolders.View
{
    public partial class UserPreferences : ContentPage
    {
        ViewCell lastCell;
        private SQLiteConnection conn;
        public UserEmail userEmail;
        public UserPreferences()
        {
            InitializeComponent();
            conn = DependencyService.Get<ISQLite>().GetConnectionWithCreateDatabase();
            var data = (from usr in conn.Table<UserEmail>() select usr);
            MainListView.ItemsSource = data;
        }
        protected override void OnAppearing()
        {
            PopulateEmailList();
        }
        public void PopulateEmailList()
        {
            MainListView.ItemsSource = null;
            MainListView.ItemsSource = DependencyService.Get<ISQLite>().GetUsers();
        }


        void MenuItem_Clicked(System.Object sender, System.EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var t = mi.CommandParameter as UserEmail;
            if (t != null)
            {
                Navigation.PushAsync(new View.EditEmail(t));
            }
        }
       async void MenuItem_Clicked_1(System.Object sender, System.EventArgs e)
        {
            bool res = await DisplayAlert("Message", "Do you want to delete Email?", "Ok", "Cancel");
            if (res)
            {
                var menu = sender as MenuItem;
                UserEmail details = menu.CommandParameter as UserEmail;
                DependencyService.Get<ISQLite>().DeleteEmail(details.ID);
                PopulateEmailList();
            }
        }
        void ViewCell_Tapped(System.Object sender, System.EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.Yellow;
                lastCell = viewCell;
            }
        }
    }  
}
