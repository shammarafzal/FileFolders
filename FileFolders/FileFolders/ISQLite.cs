using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
namespace FileFolders
{
    public interface ISQLite
    {
        SQLiteConnection GetConnectionWithCreateDatabase();

        bool SaveEmail(UserEmail userEmail);
        List<UserEmail> GetUsers();
        bool UpdateEmail(UserEmail userEmail);
        void DeleteEmail(int Id);

        bool SaveFolder(UserFolder userFolder);
        List<UserFolder> GetFolders();
        bool UpdatePicture(UserFolder userFolder);
        void DeleteFolder(int Id);
        bool UpdateName(UserFolder userFolder);
    }
}
