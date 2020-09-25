using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using FFImageLoading.Forms;
using FileFolders.Model;
using Plugin.FilePicker;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using System.Threading.Tasks;
namespace FileFolders.View
{
    public partial class ImagePicker : ContentPage
    {
        ObservableCollection<MediaFile> files = new ObservableCollection<MediaFile>();
        private ObservableCollection<FileModel> myList;
        public ObservableCollection<FileModel> MyList
        {
            get { return myList; }
            set { myList = value; }
        }      
        public ImagePicker()
        {
            InitializeComponent();         
            files.CollectionChanged += Files_CollectionChanged;
            this.BindingContext = this;
            MyList = new ObservableCollection<FileModel>();
     
        }
        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            files.Clear();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var picked = await CrossMedia.Current.PickPhotosAsync();
            if (picked == null)
            {
                await DisplayAlert("STATUS", "Please choose image.", "Abort");
                return;
            }
            else if (picked != null)
            {
                foreach (var file in picked)
                {
                    files.Add(file);
                }
            }
        }
        private  void Files_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (files.Count == 0)
            {
                ListImages.Children.Clear();
                return;
            }
            if (e.NewItems.Count == 0)
                return;
            var file = e.NewItems[0] as MediaFile;
            var image2 = new CachedImage { WidthRequest = 300, HeightRequest = 300, Aspect = Aspect.AspectFit };
            image2.Source = ImageSource.FromFile(file.Path);
            Console.WriteLine(file.Path);
            string foldername = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", file.Path);
            FileInfo fInfo = new FileInfo(foldername);
            string trimename = fInfo.Name;
            string actualpath = foldername.Replace(trimename, "");
            Console.WriteLine(actualpath);
            FileInfo[] fileInfoArr;
            StringBuilder sbrfname = new StringBuilder();
            DirectoryInfo dir = new DirectoryInfo(actualpath);
            fileInfoArr = dir.GetFiles("*.jpg");
            MyList.Clear();
            foreach (FileInfo f in fileInfoArr)
            {
                Console.WriteLine(sbrfname.AppendLine(f.FullName));
                MyList.Add(new FileModel() { Files = f.Name,  Dates = f.CreationTime });
                FileListView.ItemsSource = MyList;
            }
            ListImages.Children.Add(image2);
        }
        async void File_Picked(System.Object sender, System.EventArgs e)
        {
            try
            {
                var file = await CrossFilePicker.Current.PickFile();
                if (file == null)
                {
                    await DisplayAlert("STATUS", "Please pick file.", "Abort");
                    return;
                }
                else
                {
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        string foldername = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", file.FilePath);
                        Console.WriteLine(foldername);
                        FileInfo fInfo = new FileInfo(foldername);
                        string trimename = fInfo.Name;
                        string actualpath = foldername.Replace(trimename, "");
                        FileInfo[] fileInfoArr;
                        DirectoryInfo dir = new DirectoryInfo(actualpath);
                        fileInfoArr = dir.GetFiles("*.*");
                        MyList.Clear();
                        foreach (FileInfo f in fileInfoArr)
                        {
                            MyList.Add(new FileModel() { Files = f.Name, Dates = f.CreationTime });
                            FileListView.ItemsSource = MyList;
                        }
                        var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        var directoryfolder = Path.Combine(documents, "Files");
                        System.IO.File.Copy(file.FilePath, directoryfolder);
                    }
                    else
                    {
                        var foldername = file.FilePath;
                        FileInfo fInfo = new FileInfo(foldername);
                        string trimename = fInfo.Name;
                        string actualpath = foldername.Replace(trimename, "");
                        if (actualpath == "content://com.android.providers.downloads.documents/document/")
                        {
                            await DisplayAlert("Invalid File Path", "Please pick file from orginal directory", "Abort");
                            return;
                        }
                        else
                        {
                            FileInfo[] fileInfoArr;
                            DirectoryInfo dir = new DirectoryInfo(actualpath);
                            fileInfoArr = dir.GetFiles("*.pdf");
                            MyList.Clear();
                            foreach (FileInfo f in fileInfoArr)
                            {
                                MyList.Add(new FileModel() { Files = f.Name, Dates = f.CreationTime });
                                FileListView.ItemsSource = MyList;
                            }
                            var destination = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures/Files/");
                            System.IO.File.Copy(file.FilePath, destination);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        void FileListView_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {

        }
        async void Auto_Copy(System.Object sender, System.EventArgs e)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {        
            }
            else
            {
                copy_from_download();
                copy_from_whatsapp();
            }
        }
            async void copy_from_download()
            {
                string actualpath = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Download/");
                var destination = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures/Files/");
                var checkkfiles = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures/");
                FileInfo[] fileInfoArr;
                DirectoryInfo dir = new DirectoryInfo(actualpath);
                fileInfoArr = dir.GetFiles("*.pdf");
                FileInfo[] filesArr;
                DirectoryInfo dirs = new DirectoryInfo(destination);
                filesArr = dirs.GetFiles("*.pdf");
                try
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                    if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                        {
                            await DisplayAlert("Permission Denied", "Gonna need that Storage Permission", "OK");
                        }
                        status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                    }
                    if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                    {
                    foreach (FileInfo f in fileInfoArr)
                    {
                        foreach (FileInfo fss in filesArr)
                        {
                            foreach (string fx in Directory.EnumerateFiles(checkkfiles, f.Name, SearchOption.AllDirectories))
                            { 
                                if (System.IO.File.Exists(fx) || (fx.Length == f.Length) || (fss.CreationTime == f.CreationTime))
                                {
                                    await DisplayAlert("Alert", fx + " already exist", "OK");
                                    continue;
                                }
                            }
                            MyList.Add(new FileModel() { Files = f.Name, Dates = f.CreationTime });
                            FileListView.ItemsSource = MyList;
                            System.IO.File.Copy(f.FullName, destination, true);
                        }
                    }
                        await DisplayAlert("Alert", "Files from downloads" + " Copied Successfully", "OK");
                    }
                    else if (status != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        async void copy_from_whatsapp()
        {
            string actualpath = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/WhatsApp/Media/WhatsApp Documents/");
            var destination = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures/Files/");
            var checkkfiles = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures/");
            FileInfo[] fileInfoArr;
            DirectoryInfo dir = new DirectoryInfo(actualpath);
            fileInfoArr = dir.GetFiles("*.pdf");
            FileInfo[] filesArr;
            DirectoryInfo dirs = new DirectoryInfo(destination);
            filesArr = dirs.GetFiles("*.pdf");
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        await DisplayAlert("Permission Denied", "Gonna need that Storage Permission", "OK");
                    }
                    status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }
                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    foreach (FileInfo f in fileInfoArr)
                    {
                        foreach (FileInfo fss in filesArr)
                        {
                            foreach (string fx in Directory.EnumerateFiles(checkkfiles, f.Name, SearchOption.AllDirectories))
                            {   
                                if (System.IO.File.Exists(fx) || (fx.Length == f.Length) || (fss.CreationTime == f.CreationTime))
                                {
                                    await DisplayAlert("Alert", fx + " already exist", "OK");
                                    continue;
                                }
                            }
                            MyList.Add(new FileModel() { Files = f.Name, Dates = f.CreationTime });
                            FileListView.ItemsSource = MyList;
                            System.IO.File.Copy(f.FullName, destination, true);
                        }
                    }
                    await DisplayAlert("Alert", "Files from whatsapp" + " Copied Successfully", "OK");
                }
                else if (status != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new PDFDownloader());
        }

        void Button_Clicked_2(System.Object sender, System.EventArgs e)
        {
            Application.Current.Properties["IsLoggedIn"] = Boolean.FalseString;
            Navigation.PushAsync(new View.Login_Screen());
        }

    }
}

