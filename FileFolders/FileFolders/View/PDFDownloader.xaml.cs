using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using Xamarin.Essentials;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using System.Threading.Tasks;
namespace FileFolders.View
{
    public partial class PDFDownloader : ContentPage
    {
        public IDownloadFile File;
        bool isDownloading = true;
        public PDFDownloader()
        {
            InitializeComponent();
            CrossDownloadManager.Current.CollectionChanged += (sender, e) =>
            System.Diagnostics.Debug.WriteLine(
                "[DownloadManager] " + e.Action +
                " -> New items: " + (e.NewItems?.Count ?? 0) +
                " at " + e.NewStartingIndex +
                " || Old Items: " + (e.OldItems?.Count ?? 0) +
                " at " + e.OldStartingIndex
                );
        }
        public async void DownloadFile(String FileName)
        {
            await Task.Yield();
            await Task.Run(() =>
            {
                var dowmloadManager = CrossDownloadManager.Current;
                var file = dowmloadManager.CreateDownloadFile(FileName);
                dowmloadManager.Start(file, true);
                while (isDownloading)
                {
                    isDownloading = IsDownloading(file);
                }
            });
            if (!isDownloading)
            {
                await DisplayAlert("Status", "File Downloaded", "Ok");
            }
        }
        public void AbortDownloading()
        {
            CrossDownloadManager.Current.Abort(File);
        }
        public bool IsDownloading(IDownloadFile File)
        {
            if (File == null) return false;
            switch (File.Status)
            {
                case DownloadFileStatus.INITIALIZED:
                case DownloadFileStatus.PAUSED:
                case DownloadFileStatus.PENDING:
                case DownloadFileStatus.RUNNING:
                    return true;
                case DownloadFileStatus.COMPLETED:
                case DownloadFileStatus.CANCELED:
                case DownloadFileStatus.FAILED:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        protected void webOnNavigating(object sender, WebNavigatingEventArgs e)
        {
            var link = e.Url;
            Application.Current.Properties["link"] = link;
        }
        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("link"))
            {
                var limk = Application.Current.Properties["link"].ToString();
                DownloadFile(limk);
                Application.Current.Properties.Clear();
            }          
        }
    }  
}
