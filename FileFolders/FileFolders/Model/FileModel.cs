using System;
namespace FileFolders.Model
{
    public class FileModel
    {
        private string file;
        public string Files
        {
            get { return file; }
            set { file = value; }
        }
        private DateTime datecreated;
        public DateTime Dates
        {
            get { return datecreated; }
            set { datecreated = value; }
        }
    }
}
