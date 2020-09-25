using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
namespace FileFolders
{
    public class UserFolder
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public String Name { get; set; }
        public String Image { get; set; }
    }
}
