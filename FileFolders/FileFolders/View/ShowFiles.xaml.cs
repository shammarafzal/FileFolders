using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FileFolders.Model;
using Xamarin.Forms;
namespace FileFolders.View
{
    public partial class ShowFiles : ContentPage
    {
        UserFolder userFolder;
        private ObservableCollection<FileModel> myList;
        public ObservableCollection<FileModel> MyList
        {
            get { return myList; }
            set { myList = value; }
        }       
        public ShowFiles(UserFolder editFolder)
        {
            InitializeComponent();
            if (editFolder != null)
            {
                userFolder = editFolder;
                PopulateDetails(userFolder);
            }
            userFolder = editFolder;
            PopulateDetails(userFolder);
            this.BindingContext = this;
            MyList = new ObservableCollection<FileModel>();
            //this.Title = name;
            load();
        }
        private void PopulateDetails(UserFolder userFolder)
        {
            string folderName = userFolder.Name;
        }
        void MainSearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var keyword = MainSearchBar.Text;
            if (keyword != null)
            {
                FolderListView.BeginRefresh();
                FolderListView.ItemsSource = MyList.Where(i => i.Files.ToLower().Contains(keyword.ToLower()));
                FolderListView.EndRefresh();
            }
            else
            {
                MainSearchBar.Text = "";
            }
        }
        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            var sorted = MyList.OrderBy(x => x.Files)
                          .ToList();
            FolderListView.ItemsSource = sorted;
        }
        public void load()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryfolder = documents + "/" + userFolder.Name;
                Console.WriteLine(directoryfolder);
                FileInfo[] fileInfoArr;
                DirectoryInfo dir = new DirectoryInfo(directoryfolder);
                fileInfoArr = dir.GetFiles("*.*");
                MyList.Clear();
                foreach (FileInfo f in fileInfoArr)
                {
                    MyList.Add(new FileModel() { Files = f.Name, Dates = f.CreationTime });
                    FolderListView.ItemsSource = MyList;
                }
            }
            else if (Device.RuntimePlatform == Device.Android)
             {
                 var documents = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures");
                 var directoryfolder = documents +"/"+ userFolder.Name;
                 FileInfo[] fileInfoArr;
                 DirectoryInfo dir = new DirectoryInfo(directoryfolder);
                 fileInfoArr = dir.GetFiles("*.*");
                 MyList.Clear();
                 foreach (FileInfo f in fileInfoArr)
                 {
                     MyList.Add(new FileModel() { Files = f.Name, Dates = f.CreationTime });
                     FolderListView.ItemsSource = MyList;
                 }
             }
        }
        public async void OnMove(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var t = mi.CommandParameter as FileModel;
            if (Device.RuntimePlatform == Device.iOS)
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryname = documents + "/" + userFolder.Name + "/";
                string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", directoryname);
                var movefile = filename + t.Files;
                Application.Current.Properties["movefile"] = movefile;
                await Navigation.PushModalAsync(new SelectView());        
            }
            else
            {
                var documents = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures");
                var filename = documents + "/" + userFolder.Name + "/";
                var movefile = filename + t.Files;
                Application.Current.Properties["movefile"] = movefile;
                MyList.Remove(t);
                await Navigation.PushModalAsync(new SelectView());
            }
        }
        protected override void OnAppearing()
        {
            load();
        }
        public async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var t = mi.CommandParameter as FileModel;
            if (Device.RuntimePlatform == Device.iOS)
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryname = documents + "/" + userFolder.Name + "/";
                string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", directoryname);
                var deletefile = filename + t.Files;
                var permission = await DisplayAlert("Alert", "Are you sure you want to delete?", "Yes", "No");
                if (permission)
                {
                    File.Delete(deletefile);
                    load();
                }
                else
                {
                    return;
                }
            }
            else
            {
                var documents = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures");
                var filename = documents + "/" + userFolder.Name + "/";
                var deletefile = filename + t.Files;
                var permission = await DisplayAlert("Alert", "Are you sure you want to delete?", "Yes", "No");
                if (permission)
                {
                    File.Delete(deletefile);
                    load();
                }
                else
                {
                    return;
                }
            }
        }
        async void FolderListView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryname = documents + "/" + userFolder.Name + "/";
                var name = e.Item as FileModel;
                await Navigation.PushAsync(new PDFViewer(name.Files, directoryname));
            }
             else if (Device.RuntimePlatform == Device.Android)
             {
                 var documents = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures");
                 var directoryfolder = documents + "/" + userFolder.Name +"/";
                 var name = e.Item as FileModel;
                 await Navigation.PushAsync(new ImageViewer(name.Files, directoryfolder));
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
