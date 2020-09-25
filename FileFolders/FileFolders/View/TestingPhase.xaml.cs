using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FileFolders.Model;
using Xamarin.Forms;
using SQLite;
using Xamarin.Essentials;
using FileFolders.Model;

namespace FileFolders.View
{
    public partial class TestingPhase : ContentPage
    {
        private SQLiteConnection conn;
        public UserFolder userFolder;
        public TestingPhase()
        {
            InitializeComponent();
            conn = DependencyService.Get<ISQLite>().GetConnectionWithCreateDatabase();
            var dataFolder = (from fld in conn.Table<UserFolder>() select fld);

            FolderListView.ItemsSource = dataFolder;
        }
        protected override void OnAppearing()
        {
            PopulateEmailList();
        }
        public void PopulateEmailList()
        {
            FolderListView.ItemsSource = null;
            FolderListView.ItemsSource = DependencyService.Get<ISQLite>().GetFolders();
        }
 
    }
}
