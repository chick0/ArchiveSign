namespace ArchiveSign
{
    class TempfileManager : IDisposable
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

        ~TempfileManager() { Clear(); }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Clear();
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
