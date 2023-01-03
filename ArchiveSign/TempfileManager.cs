namespace ArchiveSign
{
    class TempfileManager
    {
        string path;

        public TempfileManager()
        {
            path = System.IO.Path.GetTempFileName();
        }

        public string Path
        {
            get { return path; }
        }

        public void Clear()
        {
            try
            {
                File.Delete(path);
            }
            catch { }
        }
    }
}
