using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class FileManager
    {
        public static List<File> GetAllFiles()
        {
            DataClassesDataContext db = new DataClassesDataContext();
            return db.Files.ToList();
        }

        public static void AddFile(File file)
        {
            DataClassesDataContext db = new DataClassesDataContext();

            file.id = GetNextId();
            db.Files.InsertOnSubmit(file);
            db.SubmitChanges();
        }

        public static void DeleteFile(int fileid)
        {
            DataClassesDataContext db = new DataClassesDataContext();

            db.Files.Where(u => u.id == fileid)
                .ToList()
                .ForEach(u => db.Files.DeleteOnSubmit(u));
            db.SubmitChanges();
        }

        private static int GetNextId()
        {
            DataClassesDataContext db = new DataClassesDataContext();
            int i = 0;
            db.Files.ToList().ForEach(u => { if (u.id > i) i = u.id; });
            return i + 1;
        }
    }

    public class FileType
    {
        public const int LEXIC = 0;
        public const int GRAMMAR = 1;
        public const int DLL = 2;
    }
}
