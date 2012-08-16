using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lang
{
    public partial class FileManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAddFile_Click(object sender, EventArgs evArgs)
        {
            Data.File f = new Data.File();
            f.name = fileName.Text;
            if (String.IsNullOrWhiteSpace(f.name))
                f.name = "New file";
            f.date_created = DateTime.Now;
            f.owner = Data.UserManager.GetUser().id;
            f.is_public = f.name.Length % 2;
            
            if (null != fileAdd.PostedFile)
            {
                try
                {
                    f.filename = fileAdd.PostedFile.FileName;

                    Data.FileManager.AddFile(f);
                }
                catch (Exception e)
                {

                }
            }
        }

        [WebMethod]
        public static void DeleteFile(int fileid)
        {
            Data.FileManager.DeleteFile(fileid);
        }
    }
}