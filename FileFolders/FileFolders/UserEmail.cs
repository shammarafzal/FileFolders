using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
namespace FileFolders
{
    public class UserEmail
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public String Email { get; set; }
    }
}
