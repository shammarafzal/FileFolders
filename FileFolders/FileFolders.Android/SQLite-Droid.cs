using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FileFolders.Droid;
using SQLite;
using Xamarin.Forms;
[assembly:Dependency(typeof(SQLite_Droid))]
namespace FileFolders.Droid
{
    public class SQLite_Droid : ISQLite
    {

        SQLiteConnection con;
        public SQLiteConnection GetConnectionWithCreateDatabase()
        {
            string fileName = "sampleDatabase.db3";
            string documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Path.Combine(documentPath, fileName);
            con = new SQLiteConnection(path);
            con.CreateTable<UserEmail>();
            //
            con.CreateTable<UserFolder>();
            //
            return con;
        }
        public bool SaveEmail(UserEmail userEmail)
        {
            bool res = false;
            try
            {
                con.Insert(userEmail);
                res = true;
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        //
        public bool SaveFolder(UserFolder userFolder)
        {
            bool res = false;
            try
            {
                con.Insert(userFolder);
                res = true;
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        //
        public List<UserEmail> GetUsers()
        {
            string sql = "SELECT * FROM UserEmail";
            List<UserEmail> userEmail = con.Query<UserEmail>(sql);
            return userEmail;
        }
        //
        public List<UserFolder> GetFolders()
        {
            string sql = "SELECT * FROM UserFolder";
            List<UserFolder> userFolder = con.Query<UserFolder>(sql);
            return userFolder;
        }
        //

        public bool UpdateEmail(UserEmail userEmail)
        {
            bool res = false;
            try
            {
                string sql = $"UPDATE UserEmail SET Email='{userEmail.Email}' WHERE ID={userEmail.ID}";
                con.Execute(sql);
                res = true;
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        //
        public bool UpdatePicture(UserFolder userFolder)
        {
            bool res = false;
            try
            {
                string sql = $"UPDATE UserFolder SET Image='{userFolder.Image}' WHERE ID={userFolder.ID}";
                con.Execute(sql);
                res = true;
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        //
        public bool UpdateName(UserFolder userFolder)
        {
            bool res = false;
            try
            {
                string sql = $"UPDATE UserFolder SET Name='{userFolder.Name}' WHERE ID={userFolder.ID}";
                con.Execute(sql);
                res = true;
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        //
        public void DeleteEmail(int Id)
        {
            string sql = $"DELETE FROM UserEmail WHERE ID={Id}";
            con.Execute(sql);
        }
        //
        public void DeleteFolder(int Id)
        {
            string sql = $"DELETE FROM UserFolder WHERE ID={Id}";
            con.Execute(sql);
        }
        //
    }
}
