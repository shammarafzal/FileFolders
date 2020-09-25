using System;
using System.Collections.Generic;
using FileFolders.AppConstants;
using Xamarin.Forms;
using Xamarin.Auth;
using FileFolders.AuthHelpers;
using System.Linq;
using System.Diagnostics;
using Newtonsoft.Json;
using SQLite;
using FileFolders.Model;
using Xamarin.Essentials;
namespace FileFolders.View
{
    public partial class Login_Screen : ContentPage
    {
		private SQLiteConnection con;
		public UserEmail userEmail;
		public UserFolder userFolder;
		String isEmail;
		String isPassword;
		Account account;
		AccountStore store;
		public Login_Screen()
        {
			
			InitializeComponent();

			
			con = DependencyService.Get<ISQLite>().GetConnectionWithCreateDatabase();
			store = AccountStore.Create();
			if (Application.Current.Properties.ContainsKey("saveEmail") || Application.Current.Properties.ContainsKey("savePassword")) 
			{
				String email = Application.Current.Properties["saveEmail"].ToString();
				String password = Application.Current.Properties["savePassword"].ToString();
				txtEmail.Text = email;
				txtPassword.Text = password;
			}
            else
            {
				txtEmail.Text = "";
				txtPassword.Text = "";
			}
			var firstLaunch = VersionTracking.IsFirstLaunchEver;
			if (firstLaunch)
			{
				userFolder = new UserFolder();
				userFolder.Name = "Files";
				userFolder.Image = "folder.png";
				con.Insert(userFolder);

			}
		}
		async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
			isEmail = txtEmail.Text;
			isPassword = txtPassword.Text;
			Application.Current.Properties["saveEmail"] = isEmail;
			Application.Current.Properties["savePassword"] = isPassword;
			if (isEmail == "" && String.IsNullOrEmpty(isPassword))
			{
				await DisplayAlert("Login Failed", "Check Your Credentails", "Ok");
			}
			else
			{
				userEmail = new UserEmail();
				userEmail.Email = Application.Current.Properties["saveEmail"].ToString();
				con.Insert(userEmail);
				Application.Current.Properties["IsLoggedIn"] = Boolean.TrueString;
				Application.Current.MainPage = new MainPage();
			}
		}
		void btn_Google_Clicked(System.Object sender, System.EventArgs e)
        {
			string clientId = null;
			string redirectUri = null;
			switch (Device.RuntimePlatform)
			{
				case Device.iOS:
					clientId = Constants.iOSClientId;
					redirectUri = Constants.iOSRedirectUrl;
					break;

				case Device.Android:
					clientId = Constants.AndroidClientId;
					redirectUri = Constants.AndroidRedirectUrl;
					break;
			}
			account = store.FindAccountsForService(Constants.AppName).FirstOrDefault();
			var authenticator = new OAuth2Authenticator(
				clientId,
				null,
				Constants.Scope,
				new Uri(Constants.AuthorizeUrl),
				new Uri(redirectUri),
				new Uri(Constants.AccessTokenUrl),
				null,
				true);
			authenticator.Completed += OnAuthCompleted;
			authenticator.Error += OnAuthError;
			AuthenticationState.Authenticator = authenticator;
			var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
			presenter.Login(authenticator);																
		}
		async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
		{
			var authenticator = sender as OAuth2Authenticator;
			if (authenticator != null)
			{
				authenticator.Completed -= OnAuthCompleted;
				authenticator.Error -= OnAuthError;
			}
			User user = null;
			if (e.IsAuthenticated)
			{
				var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, e.Account);
				var response = await request.GetResponseAsync();
				if (response != null)
				{
					string userJson = await response.GetResponseTextAsync();
					user = JsonConvert.DeserializeObject<User>(userJson);
				}
				if (user != null)
				{
					var email = user.Email;
					
					Application.Current.Properties["IsLoggedIn"] = Boolean.TrueString;
					await DisplayAlert("alert", email, "ok");
					Application.Current.MainPage = new MainPage();
				}
			}
		}
		void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
		{
			var authenticator = sender as OAuth2Authenticator;
			if (authenticator != null)
			{
				authenticator.Completed -= OnAuthCompleted;
				authenticator.Error -= OnAuthError;
			}
			Debug.WriteLine("Authentication error: " + e.Message);
		}   
    }
}
