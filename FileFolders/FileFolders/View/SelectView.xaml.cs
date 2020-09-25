using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FileFolders.Model;
using Xamarin.Forms;
using SQLite;
namespace FileFolders.View
{
    public partial class SelectView : ContentPage
    {
        private SQLiteConnection con;
        public UserFolder userFolder;
        public SelectView()
        {         
            InitializeComponent();
            con = DependencyService.Get<ISQLite>().GetConnectionWithCreateDatabase();
            this.BindingContext = this;
            loadList();
        }
        void SelectListView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var name = e.Item as UserFolder;
            if (Device.RuntimePlatform == Device.iOS)
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryname = documents + "/" + name.Name;
                string foldername = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", directoryname);
                Application.Current.Properties["foldername"] = foldername;
                Move();
                Navigation.PopModalAsync();
            }
            else
            {
                var documents = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures");
                var foldername = documents + "/" + name.Name +"/";
                Application.Current.Properties["foldername"] = foldername;
                Move();
                Navigation.PopModalAsync();
            }
        }
        public void loadList()
        {
            var dataFolder = (from fld in con.Table<UserFolder>() select fld);
            SelectListView.ItemsSource = dataFolder;
        }
        public void Move()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                if (Application.Current.Properties.ContainsKey("movefile"))
                {
                    String movefile = Application.Current.Properties["movefile"].ToString();
                    FileInfo mFile = new FileInfo(movefile);
                    if (Application.Current.Properties.ContainsKey("foldername"))
                    {
                        String foldername = Application.Current.Properties["foldername"].ToString();
                        if (new FileInfo(foldername + "/" + mFile.Name).Exists == false)
                        {
                            mFile.MoveTo(foldername + "/" + mFile.Name);
                        }
                    }
                }
            }
            else
            {
                if (Application.Current.Properties.ContainsKey("movefile"))
                {
                    String movefile = Application.Current.Properties["movefile"].ToString();
                    FileInfo mFile = new FileInfo(movefile);
                    if (Application.Current.Properties.ContainsKey("foldername"))
                    {
                        String foldername = Application.Current.Properties["foldername"].ToString();
                        if (new FileInfo(foldername + "/" + mFile.Name).Exists == false)
                        {
                            mFile.MoveTo(foldername + "/" + mFile.Name);
                        }
                    }
                }
            }
        }
        ViewCell lastCell;
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
