using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using FileFolders.Model;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Reflection;
using System.Threading.Tasks;
using SQLite;

namespace FileFolders.View
{
    public partial class FileHandler : ContentPage
    {
        ViewCell lastCell;
        private SQLiteConnection con;
        public UserFolder userFolder;
        public FileHandler()
        {
          
            InitializeComponent();

            con = DependencyService.Get<ISQLite>().GetConnectionWithCreateDatabase();
            this.BindingContext = this;
            RootFolders();
            MoveFileToRoot();
            loadList();
            var firstLaunch = VersionTracking.IsFirstLaunchEver;
            if (firstLaunch)
            {
                userFolder = new UserFolder();
                userFolder.Name = "Pictures";
                userFolder.Image = "folder.png";
                con.Insert(userFolder);

            }
        }

        public async void Create_Directory(System.Object sender, System.EventArgs e)
        {
            string currentDirectoryName = await DisplayPromptAsync("Creating Directory", "Please Enter Folder Name");         
            if (currentDirectoryName == null || currentDirectoryName == "")
            {
                return;
            }
            else
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    Console.WriteLine(documents);
                    var directoryname = Path.Combine(documents, currentDirectoryName);
                    Directory.CreateDirectory(directoryname);
                    string foldername = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", directoryname);
                    FileInfo fInfo = new FileInfo(foldername);
                    //
                    userFolder = new UserFolder();
                    userFolder.Name = currentDirectoryName;
                    userFolder.Image = "folder.png";
                    con.Insert(userFolder);
                    //
                    string trimename = fInfo.Name;
                    string actualpath = foldername.Replace(trimename, "");
                    foreach (var di in System.IO.Directory.GetDirectories(actualpath))
                    {
                        FileInfo ff = new FileInfo(di);
                        string dirName = ff.Name;
                        
                        FolderListView.ItemsSource = DependencyService.Get<ISQLite>().GetFolders();
                    }
                }
                else
                {
                    var documents = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures");
                    var directoryname = documents +"/"+ currentDirectoryName;
                    Directory.CreateDirectory(directoryname);
                    //
                    userFolder = new UserFolder();
                    userFolder.Name = currentDirectoryName;
                    userFolder.Image = "folder.png";
                    con.Insert(userFolder);
                    foreach (var di in System.IO.Directory.GetDirectories(documents))
                    {
                        FileInfo ff = new FileInfo(di);
                        string dirName = ff.Name;
                     
                        FolderListView.ItemsSource = DependencyService.Get<ISQLite>().GetFolders(); ;
                    }
                }
            }

        }
        void OnBtnPressed(object sender, EventArgs ea)
        {
            var keyword = MainSearchBar.Text;
            if (keyword != null)
            {
                FolderListView.BeginRefresh();
                FolderListView.ItemsSource = DependencyService.Get<ISQLite>().GetFolders().Where(i => i.Name.ToLower().Contains(keyword.ToLower()));
                FolderListView.EndRefresh(); 
            }
            else
            {
                MainSearchBar.Text = "";
            }
        }
        public async void OnMore(object sender, EventArgs e)
        {
            var menu = sender as MenuItem;
            UserFolder details = menu.CommandParameter as UserFolder;
            if (Device.RuntimePlatform == Device.iOS)
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryname = documents + "/";
                string foldername = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", directoryname);
                var cantrenamepic = directoryname + "Pictures";
                var cantrenamefile = directoryname + "Files";
                var cantdeletetemp = directoryname + "temp";
                var cantdeleteconf = directoryname + ".config"; 
                var sourceFolderName = foldername + details.Name;
                if (sourceFolderName == cantrenamepic || sourceFolderName == cantrenamefile || sourceFolderName == cantdeletetemp || sourceFolderName == cantdeleteconf)
                {
                    await DisplayAlert("Access Denied", "Cannot Renmae Root Directory", "Abort");
                    return;
                }
                else
                {
                    string getName = await DisplayPromptAsync("Rename Directory", "Please Enter Folder Name");
                    if (getName == "" || getName == null)
                    {
                        return;
                    }
                    else
                    {
                        try
                        {
                            string destFolderName = foldername + getName;
                            Debug.WriteLine("Rona" + destFolderName);
                            Debug.WriteLine("Sona" + sourceFolderName);
                            Directory.Move(sourceFolderName, destFolderName);
                            userFolder.Name = getName;
                            bool res = DependencyService.Get<ISQLite>().UpdateName(userFolder);
                            if (res)
                            {
                                loadList();
                            }
                            else
                            {
                                DisplayAlert("Message", "Data Failed To Update", "Ok");
                            }
                            loadList();
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Access Denied", "Same Folder or File already exist", "Abort");
                        }
                    }
                }
            }
            else
            {
                var documents = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures/");
                var cantrenamepic = documents + "Pictures";
                var cantrenamefile = documents + "Files";
                var cantdeletetemp = documents + "temp";
                var cantdeleteconf = documents + ".config";
                var sourceFolderName = documents + details.Name;
                if (sourceFolderName == cantrenamepic || sourceFolderName == cantrenamefile || sourceFolderName == cantdeletetemp || sourceFolderName == cantdeleteconf)
                {
                    await DisplayAlert("Access Denied", "Cannot Renmae Root Directory", "Abort");
                    return;
                }
                else
                {
                    string getName = await DisplayPromptAsync("Rename Directory", "Please Enter Folder Name");
                    if (getName == "" || getName == null)
                    {
                        return;
                    }
                    else
                    {
                        try
                        {
                            string destFolderName = documents + getName;
                           
                            Directory.Move(sourceFolderName, destFolderName);
                            userFolder.Name = getName;
                            bool res = DependencyService.Get<ISQLite>().UpdateName(userFolder);
                            if (res)
                            {
                                loadList();
                            }
                            else
                            {
                                DisplayAlert("Message", "Data Failed To Update", "Ok");
                            }
                            loadList();
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Access Denied", "Same Folder or File already exist", "Abort");
                        }
                    }
                }
            }         
        }
        public async void OnDelete(object sender, EventArgs e)
        {
            var menu = sender as MenuItem;
            UserFolder details = menu.CommandParameter as UserFolder;
            if (Device.RuntimePlatform == Device.iOS)
            {
                try
                {
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var directoryname = documents + "/";
                    var cantdeletepic = directoryname + "Pictures";
                    var cantdeletefile = directoryname + "Files";
                    var cantdeletetemp = directoryname + "temp";
                    var cantdeleteconf = directoryname + ".config";
                    string foldername = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", directoryname);
                    Debug.WriteLine(details);
                    var deletedir = foldername + details.Name;
                    if (deletedir == cantdeletepic || deletedir == cantdeletefile || deletedir == cantdeletetemp || deletedir == cantdeleteconf)
                    {
                        await DisplayAlert("Access Denied", "Cannot Delete Root Directory", "Abort");
                        return;
                    }
                    else
                    {
                        bool permission = await DisplayAlert("Alert", "Are you sure you want to delete?", "Yes", "No");
                        if (permission)
                        {
                            Directory.Delete(deletedir);
                            if (permission)
                            {
                                
                                DependencyService.Get<ISQLite>().DeleteFolder(details.ID);
                                
                            }
                            //
                            loadList();
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Access Denied", ex.Message, "Abort");
                }
            }
            else
            {
                try
                {
                    var directoryname = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures/");
                    var cantdeletepic = directoryname + "Pictures";
                    var cantdeletefile = directoryname + "Files";
                    var cantdeletetemp = directoryname + "temp";
                    var cantdeleteconf = directoryname + ".config";                 
                    var deletedir = directoryname + details.Name;
                    if (deletedir == cantdeletepic || deletedir == cantdeletefile || deletedir == cantdeletetemp || deletedir == cantdeleteconf)
                    {
                        await DisplayAlert("Access Denied", "Cannot Delete Root Directory", "Abort");
                        return;
                    }
                    else
                    {
                        var permission = await DisplayAlert("Alert", "Are you sure you want to delete?", "Yes", "No");
                        if (permission)
                        {
                            Directory.Delete(deletedir);
                          
                            if (permission)
                            {

                                DependencyService.Get<ISQLite>().DeleteFolder(details.ID);

                            }
                            loadList();
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Access Denied", ex.Message, "Abort");
                }
            }
        }
        //
        protected override void OnAppearing()
        {
            PopulateEmailList();
        }
        public void PopulateEmailList()
        {
            FolderListView.ItemsSource = null;
            FolderListView.ItemsSource = DependencyService.Get<ISQLite>().GetFolders();
        }
        //
        public void loadList()
        {
            var dataFolder = (from fld in con.Table<UserFolder>() select fld);
            FolderListView.ItemsSource = dataFolder;
        }
        public void RootFolders()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                Console.WriteLine(documents);
                var directorypics = Path.Combine(documents, "Pictures");
                var directoryfolder = Path.Combine(documents, "Files");
                Console.WriteLine(directorypics);
                Console.WriteLine(directoryfolder);
                Directory.CreateDirectory(directorypics);
                Directory.CreateDirectory(directoryfolder);
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                var documents = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures");
                var directorypics = Path.Combine(documents, "Pictures");
                var directoryfolder = Path.Combine(documents, "Files");
                Console.WriteLine(directorypics);
                Console.WriteLine(directoryfolder);
                Directory.CreateDirectory(directorypics);
                Directory.CreateDirectory(directoryfolder);
 
            }
        }
        public  void MoveFileToRoot()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                Console.WriteLine(documents);
                var path = Path.Combine(documents, "Pictures");
                var destinationpics = path + "/";
                var pathx = Path.Combine(documents, "temp");
                var sourcepics = pathx + "/";
                try
                { 
                List<String> MyMusicFiles = Directory
                                    .GetFiles(sourcepics, "*.*", SearchOption.AllDirectories).ToList();
                foreach (string file in MyMusicFiles)
                {
                    FileInfo mFile = new FileInfo(file);
                    if (new FileInfo(destinationpics + mFile.Name).Exists == false)
                    {
                        mFile.MoveTo(destinationpics + mFile.Name);
                    }
                }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                var documents = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures");
                var path = Path.Combine(documents, "temp");
                var sourcepics = path + "/";
                var destinationpics = documents + "/Pictures/";
                try
                {
                    List<String> MyMusicFiles = Directory
                                        .GetFiles(sourcepics, "*.*", SearchOption.AllDirectories).ToList();
                    foreach (string file in MyMusicFiles)
                    {
                        FileInfo mFile = new FileInfo(file);
                        if (new FileInfo(destinationpics + mFile.Name).Exists == false)
                        {
                            mFile.MoveTo(destinationpics + mFile.Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        public async void FolderListView_ItemTapped(Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            MoveFileToRoot();  
            var name = e.Item as UserFolder;
            await Navigation.PushAsync(new ShowFiles(name));
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
        void MenuItem_Clicked(System.Object sender, System.EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var t = mi.CommandParameter as UserFolder;
            if (t != null)
            {
                Navigation.PushAsync(new View.PictureChange(t));
            }
        }

       
    }
}
