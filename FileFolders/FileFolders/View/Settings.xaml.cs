using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.IO.Compression;
using Plugin.FilePicker;
using System.Collections.ObjectModel;
using FileFolders.Model;
using Plugin.Media.Abstractions;
using ICSharpCode.SharpZipLib.Zip;
namespace FileFolders.View
{
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
        }
        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
			if (Device.RuntimePlatform == Device.Android)
			{
                var destination = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures/");
               
                CompressAndExportFolder(destination);
              
            }
            else
            {
               
               var patth1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
               
                CompressAndExportFolder(patth1);
            
            }
		}
            public static async Task<bool> CompressAndExportFolder(string folderPath)
            {
                var exportZipTempDirectory = Path.Combine(FileSystem.CacheDirectory, "Export");
                try
                {
                    if (Directory.Exists(exportZipTempDirectory))
                    {
                        Directory.Delete(exportZipTempDirectory, true);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                var exportZipFilename = $"MyAppData_{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.zip";
                Directory.CreateDirectory(exportZipTempDirectory);
                var exportZipFilePath = Path.Combine(exportZipTempDirectory, exportZipFilename);
                if (File.Exists(exportZipFilePath))
                {
                    File.Delete(exportZipFilePath);
                }
       
                System.IO.Compression.ZipFile.CreateFromDirectory(folderPath, exportZipFilePath, CompressionLevel.Fastest, true);
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Files Backup",
                    File = new ShareFile(exportZipFilePath),
                });
                var destination = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures/Files/");
                System.IO.File.Copy(exportZipFilePath, destination);
                return true;
            }

    



        async void Button_Clicked_2(System.Object sender, System.EventArgs e)
        {
            restoreFiles();
        }
        async void restoreFiles()
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
                        var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        var directoryfolder = Path.Combine(documents, "Files");
                        var saveDestination = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                        UnzipFileAsync(file.FilePath, saveDestination);
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
                            var destination = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/Pictures/Files/");
                            var saveDestination = System.IO.Path.Combine(Environment.CurrentDirectory, "sdcard/Android/data/com.arumsolution.filefolders/files/");
                            UnzipFileAsync(file.FilePath, saveDestination);

                        }
                    }                  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private async Task<bool> UnzipFileAsync(string zipFilePath, string unzipFolderPath)
        {
            try
            {
                var entry = new ZipEntry(Path.GetFileNameWithoutExtension(zipFilePath));
                var fileStreamIn = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read);
                var zipInStream = new ZipInputStream(fileStreamIn);
                entry = zipInStream.GetNextEntry();
                while (entry != null && entry.CanDecompress)
                {
                    var outputFile = unzipFolderPath + @"/" + entry.Name;
                    var outputDirectory = Path.GetDirectoryName(outputFile);
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    if (entry.IsFile)
                    {
                        var fileStreamOut = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
                        int size;
                        byte[] buffer = new byte[4096];
                        do
                        {
                            size = await zipInStream.ReadAsync(buffer, 0, buffer.Length);
                            await fileStreamOut.WriteAsync(buffer, 0, size);
                        } while (size > 0);
                        fileStreamOut.Close();
                    }
                    entry = zipInStream.GetNextEntry();
                }
                zipInStream.Close();
                fileStreamIn.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }      
    }
}
