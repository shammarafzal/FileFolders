using System;
using System.Diagnostics;
using Xamarin.Forms;
namespace FileFolders.ViewModel
{
    public class FileHandlerActionCell : ViewCell
    {
        public FileHandlerActionCell()
        {
			var label1 = new Label { Text = "Label 1", FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), FontAttributes = FontAttributes.Bold };
			label1.SetBinding(Label.TextProperty, new Binding("."));
			var hint = Device.RuntimePlatform == Device.iOS ? "Tip: swipe left for context action" : "Tip: long press for context action";
			var label2 = new Label { Text = hint, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
			var moreAction = new MenuItem { Text = "Rename" };
			moreAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
			moreAction.Clicked += (sender, e) => {
				var mi = ((MenuItem)sender);
				Debug.WriteLine("More Context Action clicked: " + mi.CommandParameter);
			};
			var deleteAction = new MenuItem { Text = "Delete", IsDestructive = true }; // red background
			deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
			deleteAction.Clicked += (sender, e) => {
				var mi = ((MenuItem)sender);
				Debug.WriteLine("Delete Context Action clicked: " + mi.CommandParameter);
			};
			ContextActions.Add(moreAction);
			ContextActions.Add(deleteAction);
			View = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Padding = new Thickness(15, 5, 5, 15),
				Children = {
					new StackLayout {
						Orientation = StackOrientation.Vertical,
						Children = { label1, label2 }
					}
				}
			};
		}
    }
}
