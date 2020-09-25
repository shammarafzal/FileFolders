using System;
namespace FileFolders.AppConstants
{
	public class Constants
	{
		public static string AppName = "FileFolderTest";
		// OAuth
		// For Google login, configure at https://console.developers.google.com/
		public static string iOSClientId = "1045827721475-i1atm655d8f7ags3319vqosg2ci7v3fd.apps.googleusercontent.com";
		public static string AndroidClientId = "1045827721475-edmfb7qqg90fr3p0t0ma4ktocu7knrcd.apps.googleusercontent.com";
		// These values do not need changing
		public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
		public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
		public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
		public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";
		// Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
		public static string iOSRedirectUrl = "com.googleusercontent.apps.1045827721475-i1atm655d8f7ags3319vqosg2ci7v3fd:/oauth2redirect";
		public static string AndroidRedirectUrl = "com.googleusercontent.apps.1045827721475-edmfb7qqg90fr3p0t0ma4ktocu7knrcd:/oauth2redirect";
	}
}
