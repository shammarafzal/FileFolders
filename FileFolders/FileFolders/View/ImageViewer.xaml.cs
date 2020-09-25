using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
namespace FileFolders.View
{
    public partial class ImageViewer : ContentPage
    {
        public ImageViewer(string name, string image_source)
        {
            InitializeComponent();
            var file = image_source + name;
            Application.Current.Properties["file"] = file;
            image.Source = ImageSource.FromFile(image_source+name);
        }
       async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
       {
            if (Application.Current.Properties.ContainsKey("file"))
            {
                String file = Application.Current.Properties["file"].ToString();
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = Title,
                    File = new ShareFile(file),
                    PresentationSourceBounds = DeviceInfo.Platform == DevicePlatform.iOS && DeviceInfo.Idiom == DeviceIdiom.Tablet
                            ? new System.Drawing.Rectangle(0, 20, 0, 0)
                            : System.Drawing.Rectangle.Empty
                });
            }
       }
    }
}
