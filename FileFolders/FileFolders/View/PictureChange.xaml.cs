using System;
using System.Collections.Generic;
using FileFolders.Model;
using Xamarin.Forms;
using SQLite;
using System.Collections.ObjectModel;

namespace FileFolders.View
{
    public partial class PictureChange : ContentPage
    {
        private ObservableCollection<ChangeImageList> myList;
        public ObservableCollection<ChangeImageList> MyList
        {
            get { return myList; }
            set { myList = value; }
        }
        private ObservableCollection<ChangeImageList> myList1;
        public ObservableCollection<ChangeImageList> MyList1
        {
            get { return myList1; }
            set { myList1 = value; }
        }
        UserFolder userFolder;
        public PictureChange(UserFolder editFolder)
        {
            InitializeComponent();
            this.BindingContext = this;
            MyList = new ObservableCollection<ChangeImageList>();
            MyList.Add(new ChangeImageList() { Name = "blackFolder" });
            MyList.Add(new ChangeImageList() { Name = "yellowFolder" });
            MyList.Add(new ChangeImageList() { Name = "shadeFolder" });
            MyList.Add(new ChangeImageList() { Name = "rgbFolder" });
            MyList.Add(new ChangeImageList() { Name = "tFolder" });
            MyList.Add(new ChangeImageList() { Name = "lockFolder" });

            MyList1 = new ObservableCollection<ChangeImageList>();
            MyList1.Add(new ChangeImageList() { Name = "multiFolder" });
            MyList1.Add(new ChangeImageList() { Name = "docFolder" });
            MyList1.Add(new ChangeImageList() { Name = "blueFolder" });
            MyList1.Add(new ChangeImageList() { Name = "theftFolder" });
            MyList1.Add(new ChangeImageList() { Name = "programmingFolder" });
            MyList1.Add(new ChangeImageList() { Name = "designFolder" });

            if (editFolder != null)
            {
                userFolder = editFolder;
                PopulateDetails(userFolder);
            }
            userFolder = editFolder;
            PopulateDetails(userFolder);
             MainListView.ItemsSource = MyList;
            MainListView1.ItemsSource = MyList1;

        }
        private void PopulateDetails(UserFolder userFolder)
        {
            string folderName = userFolder.Name;
        }

        void ViewCell_Tapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var d = e.Item as ChangeImageList;
            userFolder.Image = d.Name;
            bool res = DependencyService.Get<ISQLite>().UpdatePicture(userFolder);
            if (res)
            {
                Navigation.PopAsync();
            }
            else
            {
                DisplayAlert("Message", "Data Failed To Update", "Ok");
            }
        }

        void ViewCell_Tapped_1(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var d = e.Item as ChangeImageList;
            userFolder.Image = d.Name;
            bool res = DependencyService.Get<ISQLite>().UpdatePicture(userFolder);
            if (res)
            {
                Navigation.PopAsync();
            }
            else
            {
                DisplayAlert("Message", "Data Failed To Update", "Ok");
            }
        }
    }
}
