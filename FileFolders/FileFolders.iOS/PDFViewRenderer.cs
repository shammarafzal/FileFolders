using System;
using FileFolders.Controls;
using FileFolders.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(PDFView), typeof(PDFViewRenderer))]
namespace FileFolders.iOS
{
    public class PDFViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (NativeView != null && e.NewElement != null)
            {
                var pdfControl = NativeView as UIWebView;

                if (pdfControl == null)
                    return;
                pdfControl.ScalesPageToFit = true;
            }
        }
    }
}