using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using System.Net.Http;
using System.Reflection;
using System.IO;
namespace FileFolders.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PDFViewer : ContentPage
    {  
        public PDFViewer(string name, string image_source)
        {
            InitializeComponent();
            string pdfPath = image_source + name;
            Console.WriteLine(pdfPath);
            if (Device.RuntimePlatform == Device.Android)
            {
               pdfView.Uri = pdfPath;
              // pdfView.On<Android>().EnableZoomControls(true);
              // pdfView.On<Android>().DisplayZoomControls(false);
            }
            else
                  pdfView.Source = pdfPath;           
        }
    }
}
