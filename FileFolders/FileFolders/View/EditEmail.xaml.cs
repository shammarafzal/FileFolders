using System;
using System.Collections.Generic;
using FileFolders.Model;
using Xamarin.Forms;
using SQLite;
namespace FileFolders.View
{
    public partial class EditEmail : ContentPage
    { 
        private SQLiteConnection conn;
        UserEmail userEmail; 
        public EditEmail(UserEmail editUser)
        {
            InitializeComponent();
            if (editUser != null)
            {
                userEmail = editUser;
                PopulateDetails(userEmail);
            }
            userEmail = editUser;
            PopulateDetails(userEmail);
        }
        private void PopulateDetails(UserEmail userEmail)
        {
            editEmailtxt.Text = userEmail.Email;
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            userEmail.Email = editEmailtxt.Text;
            bool res = DependencyService.Get<ISQLite>().UpdateEmail(userEmail);
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
