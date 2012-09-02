using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using Data;

namespace Lang
{
    public partial class Register : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            Data.User user = new User();
            user.name = UserName.Text;
            user.password = Password.Text;

            UserManager.AddUser(user);
        }

    }
}
