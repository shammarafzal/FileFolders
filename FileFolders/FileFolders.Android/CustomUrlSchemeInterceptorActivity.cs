using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using FileFolders.AuthHelpers;
namespace FileFolders.Droid
{
	[Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
	[IntentFilter(
	new[] { Intent.ActionView },
	Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
	DataSchemes = new[] { "com.googleusercontent.apps.1045827721475-edmfb7qqg90fr3p0t0ma4ktocu7knrcd" },
	DataPath = "/oauth2redirect")]
	
	public class CustomUrlSchemeInterceptorActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			
			/*base.OnCreate(savedInstanceState);
			
			// Convert Android.Net.Url to Uri
			var uri = new Uri(Intent.Data.ToString());

			// Load redirectUrl page
			AuthenticationState.Authenticator.OnPageLoading(uri);

			Finish();*/
			base.OnCreate(savedInstanceState);
			global::Android.Net.Uri uri_android = Intent.Data;

			Uri uri_netfx = new Uri(uri_android.ToString());

			// load redirect_url Page
			AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

			var intent = new Intent(this, typeof(MainActivity));
			intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
			StartActivity(intent);

			this.Finish();

			return;
		}
	}
}
